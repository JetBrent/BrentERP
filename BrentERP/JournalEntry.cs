using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BrentERP
{
    internal class JournalEntry
    {
        private protected DateTime JEDate;
        private protected int DocumentNo;
        private protected List<JELine> JELines;
        private protected string JEDescription;

        internal JournalEntry(int documentNo, string jEDescription)
        {
            JEDate = DateTime.Now;
            DocumentNo = documentNo;
            JEDescription = jEDescription;

        }

        internal List<JELine> GetJE()
        {
            return JELines;

        }

        internal bool CheckEqual()
        {
            decimal DebitAmount = 0;
            decimal CreditAmount = 0;
            foreach (JELine j in JELines)
            {
                if (j.DrCr == "Debit")
                {

                    DebitAmount += j.Amount;
                }
                else if (j.DrCr == "Credit")
                {
                    CreditAmount += j.Amount;
                }

            }
            if (DebitAmount == CreditAmount)
            {
                Console.WriteLine($"Document Number {DocumentNo} is balanced.");
                return true;
            }
            else
            {
                Console.WriteLine($"Document Number {DocumentNo} is unbalanced! \n Debit amount is : {DebitAmount}. Credit Amount is {CreditAmount}");
                return false;
            }
        }

        internal void AddLine(JELine line)
        {
            line.JELineDate = JEDate;
            line.JELineDesc = JEDescription;
            line.LineDocNum = DocumentNo;
            JELines.Add(line);
        }

        internal void AddLine(string accountNo, string drCr, decimal amount)
        {
            var line = new JELine(accountNo, drCr, amount);
            line.JELineDate = JEDate;
            line.JELineDesc = JEDescription;
            line.LineDocNum = DocumentNo;
            JELines.Add(line);
        }



    }
}
