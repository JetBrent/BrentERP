using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BrentERP
{
    public class JournalEntry
    {
        public DateTime JEDate;
        public int DocumentNo;
        public List<JELine> JELines;
        public string JEDescription;

        public JournalEntry(int documentNo, string jEDescription)
        {
            JEDate = DateTime.Now;
            DocumentNo = documentNo;
            JEDescription = jEDescription;

        }

        public List<JELine> GetJE()
        {
            return JELines;

        }

        public bool CheckEqual()
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
                return true;
            }
            else
            {
                Console.WriteLine($"Unbalanced Document Number {DocumentNo}. \n Debit amount is : {DebitAmount}. Credit Amount is {CreditAmount}.");
                return false;
            }
        }

        public void AddLine(JELine line)
        {
            line.JELineDate = JEDate;
            line.JELineDesc = JEDescription;
            line.LineDocNum = DocumentNo;
            JELines.Add(line);
        }

        public void AddLine(string accountNo, string drCr, decimal amount)
        {
            var line = new JELine(accountNo, drCr, amount);
            line.JELineDate = JEDate;
            line.JELineDesc = JEDescription;
            line.LineDocNum = DocumentNo;
            JELines.Add(line);
        }
    }
}
