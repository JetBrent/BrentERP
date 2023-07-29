using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BrentSQLDB;
using MySql.Data.MySqlClient;

namespace BrentERP
{
    internal static class BrentUtils
    {
        public static void InitDB() 
        {
            var db = new BrentDB();
            db.Init("localhost","root","MYBRENT.sql!");
            Console.WriteLine("Press enter to continue to the main program...");
            Console.ReadKey();
            Console.Clear();
        }

        public static MySqlConnection ConnectToDB()
        {
            var db = new BrentDB();
            var con = db.ConnectToDB("localhost", "root", "MYBRENT.sql!");
            return con;
        }

        public static void PrintGeneralLedger()
        {
            try
            {
                var db = new BrentDB();
                var con = ConnectToDB();
                var gl = db.ReadAllFromTable(con, "general_ledger");
                foreach (var line in gl)
                {
                    Console.Write("|");
                    foreach (var item in line)
                    {
                        Console.Write(item);
                        Console.Write("|");

                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("Finished printing the general ledger.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred on printing the general ledger: {ex.Message}");
            }
        }

        public static void ViewJournalLedger(MySqlConnection con)
        {
            try
            {
                Console.WriteLine("\nPrinting Journal Ledger...\n");
                var db = new BrentDB();
                var jl = db.ReadAllFromTable(con, "journal_ledger");
                foreach (var line in jl)
                {
                    Console.Write("|");
                    foreach (var item in line)
                    {
                        Console.Write(item);
                        Console.Write("|");

                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("\nJournal Ledger Printing Successful!\n");
                Console.WriteLine("Press enter to return to the main menu...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred on printing the journal ledger: {ex.Message}");
            }

        }

        public static void DeleteJournalLedgerEntry(MySqlConnection con)
        {
            try
            {
                Console.WriteLine("Please input the document number you want to edit.");
                string response = "";
                while (true)
                {
                    response = Console.ReadLine();
                    if (response == null)
                    {
                        Console.WriteLine("Please enter a valid journal entry.");
                    }
                    else
                    {
                        break;
                    }
                }
                var db = new BrentDB();
                db.DeleteSavedJournalEntry(con, response);
                Console.WriteLine("\nPress enter to return to the main menu...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred on deleting journal entry {ex.Message}");
            }

        } 

        public static void CreateJE(MySqlConnection con)
        {

            try
            {
                var db = new BrentDB();
                bool JEcreationsuccess = false;
                while (!JEcreationsuccess)
                {
                    int intdocnum = db.GetUniqueDocumentNumber(con);
                    Console.WriteLine($"Please input the journal entry description for journal entry number {intdocnum}.");
                    string jedesc;
                    while (true)
                    {
                        jedesc = Console.ReadLine();
                        if (jedesc != null)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Please write a non-blank journal entry description!");
                        }
                    }
                    JournalEntry je = new JournalEntry(intdocnum, jedesc);

                    bool JElinecreationsuccess = false;
                    while (!JElinecreationsuccess) // Adds the journal entry line to journal entry
                    {
                        Console.WriteLine("Please input the account number of the journal entry line."); //TOD: Add code to check whether the account is registered through the SQL database
                        bool accountinputsuccess = false;
                        int accountinput = 0;
                        while (!accountinputsuccess)
                        {
                            var accinputraw = Console.ReadLine();
                            bool accounttryparsesuccess = Int32.TryParse(accinputraw, out accountinput);
                            if (accounttryparsesuccess)
                            {
                                accountinputsuccess = true;
                            }
                            else
                            {
                                Console.WriteLine("Please input a valid account code!");
                            }
                            // Implement the ff: If accountinput not in SQL database, continue;
                        }

                        AccountCode accountno = new AccountCode(accountinput, ""); //TODO: Implement get of account description from account code database

                        var drcrcheck = false;
                        var drcr = "";
                        Console.WriteLine("Please input \"Dr\" if the amount is a debit or \"Cr\" if the amount is a credit.");
                        while (!drcrcheck)
                        {
                            drcr = Console.ReadLine();
                            if (drcr != "Dr" && drcr != "Cr")
                            {
                                Console.WriteLine("Please input either debit or credit!");
                            }
                            else
                            {
                                drcrcheck = true;
                            }
                        }

                        Console.WriteLine("Please input the amount");
                        decimal amount = 0;
                        var amountcheck = false;

                        while (!amountcheck) // Checks the amount
                        {
                            var amountinput = Console.ReadLine();
                            amountcheck = decimal.TryParse(amountinput, out amount);
                            if (!amountcheck)
                            {
                                Console.WriteLine("Invalid input amount. Please try again.");
                            }
                            if (amount < 0)
                            {
                                Console.WriteLine("Amount must not be less than zero! Please try again.");
                            }
                        }

                        JournalEntryLine line = new JournalEntryLine(accountno, drcr, amount); // Add new journal entry line
                        je.AddLine(line);

                        Console.WriteLine("Do you want to add more lines? Press y if yes. Press any key to exit.");
                        var usercontinue = Console.ReadLine();
                        if (usercontinue.ToLower() == "y" || usercontinue.ToUpper() == "Y")
                        {
                            continue;
                        }
                        else
                        {
                            db.AddJournalEntry(con, je.EntryToList());
                            Console.WriteLine("Here is the created journal entry: \n");
                            je.PrintJE();
                            JElinecreationsuccess = true;
                        }
                    }
                    JEcreationsuccess = true;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception has occured on creating journal entry: {ex}");

            }
            // TODO: Implement Create JE function
        }

        public static void PostJE(MySqlConnection con)
        {
            try
            {
                var db = new BrentDB();
                Console.WriteLine("Please input the journal entry number that you want to post.");
                var response = Console.ReadLine();
                var result = db.QueryTableOnCondition(con, "journal_ledger", "document_number", response);
                if (result != null)
                {
                    var je = new JournalEntry(result);
                    var postje = je.PrepareEntryForPosting();
                    if (postje != null)
                    {
                        db.PostJournalEntryToGeneralLedger(con, postje);
                    }
                    else
                    {
                        Console.WriteLine($"Journal Entry Number {je.DocumentNumber} is either unbalanced or incomplete. Please recheck the journal entry.");
                    }
                }
                else { Console.WriteLine("Invalid document number input. Please try again."); }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Exception has occured on posting journal entry: {ex}");
            }
        }

        public static void RegisterAccountCode()
        {
            try
            {
                Console.WriteLine("Please enter the account number of the accountcode.");
                bool accountnumbersuccess = false;
                int accountnumber = 0;
                while (!accountnumbersuccess)
                {
                    var accountnumberinput = Console.ReadLine();
                    var accnotryparse = Int32.TryParse(accountnumberinput, out accountnumber);
                    if (accnotryparse)
                    {
                        accountnumbersuccess = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid account number!");
                    }
                }

                Console.WriteLine($"Please enter the account name of account number: {accountnumber}");
                bool accountnamesuccess = false;
                string accountname = "";
                while (!accountnamesuccess)
                {
                    accountname = Console.ReadLine();
                    if (accountname == null)
                    {
                        Console.WriteLine("Please enter a valid account name! The account name cannot be null!");
                    }
                    else
                    {
                        accountnamesuccess = true;
                    }
                }
                var acccode = new AccountCode(accountnumber, accountname);
                // TODO: Implement registration of account code to SQL database here
                Console.WriteLine($"Registration of account number: {accountnumber} with the account name {accountname} is successful!");
            }
            catch ( Exception ex )
            {
                Console.WriteLine($"Exception has occured on registering account code: {ex}");
            }
  
        }
    }
}
