using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrentERP
{
    public static class utils
    {
        public static void MainMenu() 
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
                
                Press q to quit.

                *************************
                """);
                string userinput = Console.ReadLine();

                if (userinput.ToLower() == "q" || userinput.ToUpper() == "Q")
                {
                    break;
                }

                else if (userinput.Equals(null))
                {
                    Console.Clear();
                    continue;
                }
            }
        }

        public static void CreateJE()
        {
            try
            {
                bool JEcreationsuccess = false;
                while (!JEcreationsuccess)
                {
                    bool docnumparse = false;
                    int intdocnum = 0;
                    Console.WriteLine("Type the journal entry number of the new journal entry.");
                    while (!docnumparse)
                    {
                        var docnum = Console.ReadLine(); //TODO: Replace with code to add value taken from last JE number from SQL JE database
                        docnumparse = Int32.TryParse(docnum, out intdocnum);
                        if (!docnumparse)
                        {
                            Console.WriteLine("Invalid journal entry number number. Please input a valid JE number.");
                        }
                    }
                    Console.WriteLine("Please input the journal entry description.");
                    var jedesc = Console.ReadLine();
                    JournalEntry je = new JournalEntry(intdocnum, jedesc);

                    bool JElinecreationsuccess = false;
                    while (!JElinecreationsuccess) // Adds the journal entry line to journal entry
                    {
                        Console.WriteLine("Please input the account number of journal entry line."); //TOD: Add code to check whether the account is registered through the SQL database
                        var accountno = Console.ReadLine();

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

                        JELine line = new JELine(accountno, drcr, amount);
                        je.AddJELine(line);

                        Console.WriteLine("Do you want to add more lines? Press y if yes. Press any key to exit.");
                        var usercontinue = Console.ReadLine();
                        if (usercontinue.ToLower()  == "y" || usercontinue.ToUpper() == "Y")
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Journal line creation is succesful! \n Here is the created journal entry: \n");
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
            if (amountequal)
            {
                // TODO: Implement function to post journal entry to SQL database
            }
            else
            {
                Console.WriteLine("Posting will not procede due to unbalanced journal entry.");
            }
            
        }
    }
}
