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
    internal static class Utils
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
                var gl = db.ReadAllFromDatabase(con, "general_ledger");
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
                var jl = db.ReadAllFromDatabase(con, "journal_ledger");
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
        public static void MainMenu(MySqlConnection con)
        {
            while (true)
            {
                Console.WriteLine("""
                ******* MAIN MENU *******

                Choose an option:

                1. Create a Journal Entry   
                2. Post a Journal Entry
                3. View Journal Entry
                4. View GL Accounts
                5. Register GL Account
                6. Export Journal Entry Listing
                7. Export Trial Balance
                8. View General Ledger
                9. View Journal Ledger
                10. Delete Saved Journal Entry
                
                Press q to quit.

                *************************
                """);
                string userinput = Console.ReadLine();
                var boolparseinput = Int32.TryParse(userinput, out var parseinput);
                if(userinput != null)
                {
                    if (userinput.ToLower() == "q" || userinput.ToUpper() == "Q")
                    {
                        Console.Clear();
                        Console.WriteLine("Exiting application...");
                        break;
                    }

                    else if (parseinput == 1)
                    {
                        Console.Clear();
                        CreateJE(con);
                        Console.WriteLine("Press enter to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }

                    else if (parseinput == 2)
                    {
                        continue; //TODO: Implement posting of journal entry from JE listing from SL of non posted JEs
                    }

                    else if (parseinput == 3)
                    {
                        //TODO: Implement of either posted or not yet posted journal entry
                        Console.WriteLine("Viewing journal entry...");
                        continue;
                    }

                    else if (parseinput == 4)
                    {
                        Console.WriteLine("Viewing GL accounts...");
                        continue;
                    }

                    else if (parseinput == 5)
                    {
                        RegisterAccountCode();
                        //TODO: Registering GL account to SQL database
                    }

                    else if (parseinput == 6)
                    {
                        continue; //TODO: Implement export of journal entry listing here
                    }

                    else if (parseinput == 7)
                    {
                        continue; //TODO: Implement export of trial balance here
                    }

                    else if (parseinput == 8)
                    {
                        PrintGeneralLedger();
                        Console.ReadKey();
                    }

                    else if (parseinput == 9)
                    {
                        Console.Clear();
                        ViewJournalLedger(con);
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else if (parseinput == 10)
                    {

                        Console.Clear();
                        DeleteJournalLedgerEntry(con);
                        Console.ReadKey();
                        Console.Clear();

                    }

                    else
                    {
                        Console.Clear();
                        continue;
                    }
                }
                else
                {
                    Console.Clear();
                    continue;
                }

            }
        }

        public static void CreateJE(MySqlConnection con)
        {
            var db = new BrentDB();
            try
            {
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
            catch (Exception e)
            {
                Console.WriteLine($"Exception has occured on creating journal entry: {e}");

            }
            // TODO: Implement Create JE function
        }

        public static void PostJE(JournalEntry je)
        {
            bool amountequal = je.CheckEqual();
            bool balancedjelines = je.CheckJELines();
            if (amountequal && balancedjelines)
            {
                Console.WriteLine("Posting Completed!");
                // TODO: Implement function to post journal entry to SQL database
            }
            else
            {
                Console.WriteLine("Posting will not procede due to erroneous journal entry.");
            }

        }

        public static void RegisterAccountCode()
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
            while(!accountnamesuccess)
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
    }
}
