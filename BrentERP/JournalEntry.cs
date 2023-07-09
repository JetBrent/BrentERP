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
        public DateTime JEDate;
        public int DocumentNo;
        public List<JELine> JELines;
        public string JEDescription;

        public JournalEntry(int documentNo, string jEDescription)
        {
            JEDate = DateTime.Now;
            DocumentNo = documentNo;
            JEDescription = jEDescription;
            var linestart = new List<JELine> { };
            JELines = linestart;
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
                if (j.DrCr == "Dr")
                {

                    DebitAmount += j.Amount;
                }
                else if (j.DrCr == "Cr")
                {
                    CreditAmount += j.Amount;
                }

            }
            if (DebitAmount == CreditAmount)
            {
                Console.WriteLine($"Document Number {DocumentNo} is equal!");
                return true;
            }
            else
            {
                Console.WriteLine($"Unbalanced Document Number {DocumentNo}. \nDebit amount is : {DebitAmount}. Credit Amount is {CreditAmount}.");
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

        public void AddLine(AccountCode accountNo, string drCr, decimal amount) // Constructor with properties as input. Will be used for update JE functions
        {
            var line = new JELine(accountNo, drCr, amount);
            line.JELineDate = JEDate;
            line.JELineDesc = JEDescription;
            line.LineDocNum = DocumentNo;
            JELines.Add(line);
        }
        public bool CheckJELines() //Check the number of lines in the journal entry
        {
            int numberoflines = 0;
            foreach (JELine line in JELines)
            {
                numberoflines++;
            }
            if (numberoflines < 1)
            {
                Console.WriteLine($"The Document Number {DocumentNo} has only 1 line!");
                return false;
            }
            else
            {
                Console.WriteLine($"The Document Number {DocumentNo} has {numberoflines} lines.");
                Console.WriteLine();
                return true;
            }
        }
        public void PrintJE()
        {
            Console.WriteLine(" Journal Entry Number | Account Number | Debit/ Credit | Date | Description");
            Console.WriteLine("----------------------------------------------------------------------------");
            foreach (JELine j in JELines)
            {
                Console.WriteLine(" {0} | {1} | {2} | {3} | {4} | {5}", j.LineDocNum, j.AccountNo, j.DrCr, j.Amount, j.JELineDate, j.JELineDesc);
            }
        }
    }
}
