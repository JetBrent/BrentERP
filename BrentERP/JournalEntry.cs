using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrentERP
{
    internal class JournalEntry
    {
        private protected DateTime JEDate;
        private protected double Debit;
        private protected double Credit;
        private protected int DebitAccountNumber;
        private protected int CreditAccountNumber;
        private protected int JENumber;
        private protected string DebitAccountName;
        private protected string CreditAccountName;
        private protected string JEDescription;

        internal JournalEntry(double debit, double credit, int debitAccountNumber, string debitAccountName, int creditAccountNumber ,string creditAccountName, string jEDescription, int jENumber)
        {
            try
            {
                DateTime JE = DateTime.Now;
                CreditAccountNumber = creditAccountNumber;
                DebitAccountNumber = debitAccountNumber;
                DebitAccountName = debitAccountName;
                CreditAccountName = creditAccountName;
                JEDescription = jEDescription;
                JENumber = jENumber;

                if (debit == 0)
                {
                    throw new ArgumentException("Debit must not equal to 0!");
                }
                else if (credit == 0)
                {
                    throw new ArgumentException("Credit must not equal to 0!");
                }
                else if (debit != credit)
                {
                    throw new ArgumentException("Debit must be equal to Credit!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception on JE Class Constructor: {e}");
            }
            
        }

        // TODO: Add SQL Code to add journal entry details to the journal entry database
        // TODO: Convert Debit, Credit, DebitAccountNo, CreditAccountNo, DebitAccountName, DebitAccountNo to Dict

    }
}
