using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
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

        public static void PrintGeneralLedger(MySqlConnection con)
        {
            try
            {
                var db = new BrentDB();
                var gl = db.ReadAllEntriesFromGeneralLedger(con);
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

        public static void PrintJournalEntry(MySqlConnection con)
        {
            try
            {
                var db = new BrentDB();
                Console.WriteLine("Please input the document number of the journal entry you want to query.");
                var response = Console.ReadLine();
                var gl = db.QueryFromGeneralLedger(con, response);
                if (gl != null)
                {
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
                    Console.WriteLine("Finished printing the queried journal entry in the general ledger.");
                }
                var jl = db.QueryFromJournalLedger(con, response);
                if (jl != null)
                {
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
                    Console.WriteLine("Finished printing the queried journal entry in the journal ledger.");
                }
                if (jl == null && gl == null)
                {
                    Console.WriteLine($"Journal entry number {response} was not found in the journal ledger or general ledger.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred on printing the journal entry: {ex.Message}");
            }
        }


        public static void RegisterAccountCode(MySqlConnection con)
        {
            try
            {
                var db = new BrentDB();
                Console.WriteLine("Please input the account number you want to register.");
                bool accountunique = false;
                int accountinput = 0;
                while (!accountunique)
                {
                    var response = Console.ReadLine();
                    bool intaccount = Int32.TryParse(response, out accountinput);
                    if (!intaccount)
                    {

                        Console.WriteLine("Invalid account input. Please input a number");
                    }
                    else
                    {
                        var verifyaccount = db.QueryAccountCode(con, accountinput);
                        if (accountinput.ToString() == verifyaccount)
                        {
                            Console.WriteLine("Account number already exists! Please try again.");
                        }
                        else
                        {
                            accountunique = true;
                        }
                    }
                }
                Console.WriteLine($"Please input the account name for account number: {accountinput}");
                bool accountnamesuccess = false;
                string accountname = "";
                while (!accountnamesuccess)
                {
                    accountname = Console.ReadLine();
                    if (accountname == null)
                    {
                        Console.WriteLine("Please input a valid account name!");
                    }
                    else
                    {
                        accountnamesuccess = true;
                    }
                }
                db.AddGLAccount(con, accountinput, accountname);
                Console.WriteLine("Account Registration Finished.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception has occured on registering account code {ex}");
            }
        }
        public static void PrintRegisteredAccountCodes(MySqlConnection con)
        {
            try
            {
                Console.WriteLine("The registered account codes are listed below: \n");
                var db = new BrentDB();
                var acc = db.ReadAllRegisteredAccountCodes(con);
                foreach (var line in acc)
                {
                    Console.Write("|");
                    foreach (var item in line)
                    {
                        Console.Write(item);
                        Console.Write("|");

                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("Finished printing the registered account codes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception has occured on verifying account code {ex}");
            }
        }
        public static void PrintJournalLedger(MySqlConnection con)
        {
            try
            {
                Console.WriteLine("\nPrinting Journal Ledger...\n");
                var db = new BrentDB();
                var jl = db.ReadAllEntriesFromJournalLedger(con);
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

        public static void PrintTrialBalance(MySqlConnection con)
        {
            try
            {
                Console.WriteLine("\nPrinting Trial Balance...\n");
                var db = new BrentDB();
                var tb = db.ReadTrialBalance(con);
                foreach (var line in tb)
                {
                    Console.Write("|");
                    foreach (var item in line)
                    {
                        Console.Write(item);
                        Console.Write("|");

                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("\nTrial Balance Printing Successful!\n");
                Console.WriteLine("Press enter to return to the main menu...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred on printing the Trial Balance: {ex.Message}");
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

        public static void CreateJournalEntry(MySqlConnection con)
        {
            try
            {
                var db = new BrentDB();
                bool JEcreationsuccess = false;
                while (!JEcreationsuccess)
                {
                    int intdocnum = db.GetUniqueDocumentNumber(con);
                    Console.WriteLine($"Please input the journal entry description for journal entry number: {intdocnum}.");
                    string jedesc;
                    while (true)
                    {
                        jedesc = Console.ReadLine();
                        if (jedesc != string.Empty)
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
                    int jeline = 0;
                    while (!JElinecreationsuccess) // Adds the journal entry line to journal entry
                    {
                        jeline = jeline + 1;
                        Console.WriteLine($"Please input the account number of line no. {jeline} of journal entry number: {intdocnum}."); //TOD: Add code to check whether the account is registered through the SQL database
                        bool accountinputsuccess = false;
                        int accountinput = 0;
                        while (!accountinputsuccess)
                        {
                            var accinputraw = Console.ReadLine();
                            bool accounttryparsesuccess = Int32.TryParse(accinputraw, out accountinput);
                            if (accounttryparsesuccess)
                            {
                                var queryresult = db.QueryAccountCode(con, accountinput);
                                if (queryresult != null)
                                {
                                    accountinputsuccess = true;
                                }
                                else
                                {
                                    Console.WriteLine("Please input a registered account code!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Please input a valid account code!");
                            }
                            // Implement the ff: If accountinput not in SQL database, continue;
                        }

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

                        JournalEntryLine line = new JournalEntryLine(accountinput, drcr, amount); // Add new journal entry line
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

        public static void PostJournalEntry(MySqlConnection con)
        {
            try
            {
                var db = new BrentDB();
                Console.WriteLine("Please input the journal entry number that you want to post.");
                var response = Console.ReadLine();
                List<object[]> result = db.QueryFromJournalLedger(con, response);
                if (result != null)
                {
                    var je = new JournalEntry(result);
                    var jetest2 = je.EntryToList();
                    var postje = je.CheckEntryForPosting();
                    // Performs Check Equal and Check Balance checks and converts the Journal Entry class to List<object> for posting to the general_ledger table
                    if (postje != null)
                    {
                        db.PostJournalEntryToGeneralLedger(con, postje, response);
                    }
                    else
                    {
                        Console.WriteLine($"Journal Entry Number {response} is either unbalanced or incomplete. Please recheck the journal entry.");
                    }
                }
                else { Console.WriteLine("Invalid document number input. Please try again."); }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Exception has occured on posting journal entry: {ex}");
            }
        }
    }
}
