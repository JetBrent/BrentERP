using BrentSQLDB;
using MySql.Data.MySqlClient;

namespace BrentERP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BrentUtils.InitDB();
            MainMenu(BrentUtils.ConnectToDB());
            Console.ReadKey();
            // TODO: Create Account Number Class
            // TODO: Add SQL Code to add journal entry details to the journal entry database
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
                if (userinput != null)
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
                        BrentUtils.CreateJournalEntry(con);
                        Console.WriteLine("Press enter to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }

                    else if (parseinput == 2)
                    {
                        Console.Clear();
                        BrentUtils.PostJournalEntry(con);
                        Console.WriteLine("Press enter to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }

                    else if (parseinput == 3)
                    {
                        Console.Clear();
                        BrentUtils.PrintJournalEntry(con);
                        Console.WriteLine("Press enter to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

                    else if (parseinput == 4)
                    {
                        Console.Clear();
                        BrentUtils.PrintRegisteredAccountCodes(con);
                        Console.WriteLine("\nPress enter to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }

                    else if (parseinput == 5)
                    {
                        Console.Clear();
                        BrentUtils.RegisterAccountCode(con);
                        Console.WriteLine("\nPress enter to continue...");
                        Console.ReadKey();
                        Console.Clear();
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
                        Console.Clear();
                        BrentUtils.PrintGeneralLedger(con);
                        Console.ReadKey();
                        Console.Clear();
                    }

                    else if (parseinput == 9)
                    {
                        Console.Clear();
                        BrentUtils.PrintJournalLedger(con);
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else if (parseinput == 10)
                    {

                        Console.Clear();
                        BrentUtils.DeleteJournalLedgerEntry(con);
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
    }
}