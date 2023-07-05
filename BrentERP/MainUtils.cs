using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrentERP
{
    public static class MainUtils
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

                if (userinput.Equals("q") || userinput.Equals("Q"))
                {
                    break;
                }
            }
        }

        public static void CreateJE()
        {
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
