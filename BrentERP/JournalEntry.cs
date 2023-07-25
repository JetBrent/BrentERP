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
        public DateTime EntryAddDate;
        public DateTime? EntryPostDate;
        public int DocumentNumber;
        public List<JournalEntryLine> JournalEntryLines;
        public string Description;

        public JournalEntry(int documentNumber, string description)
        {
            EntryAddDate = DateTime.Now;
            DocumentNumber = documentNumber;
            Description = description;
            var linestart = new List<JournalEntryLine> { };
            JournalEntryLines = linestart;
        }

        public List<JournalEntryLine> GetJE()
        {
            return JournalEntryLines;

        }

        public bool CheckEqual()
        {
            decimal DebitAmount = 0;
            decimal CreditAmount = 0;
            foreach (JournalEntryLine j in JournalEntryLines)
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
                Console.WriteLine($"Document Number {DocumentNumber} is equal!");
                return true;
            }
            else
            {
                Console.WriteLine($"Unbalanced Document Number {DocumentNumber}. \nDebit amount is : {DebitAmount}. Credit Amount is {CreditAmount}.");
                return false;
            } 
        }

        public void AddLine(JournalEntryLine line) 
        {
            line.LineAddDate = EntryAddDate;
            line.LinePostDate = EntryPostDate;
            line.LineDesc = Description;
            line.LineDocumentNumber = DocumentNumber;
            JournalEntryLines.Add(line);
        }

        public void AddLine(AccountCode accountNumber, string drCr, decimal amount) // Constructor with properties as input. Will be used for update JE functions
        {
            var line = new JournalEntryLine(accountNumber, drCr, amount);
            line.LineAddDate = EntryAddDate;
            line.LineDesc = Description;
            line.LineDocumentNumber = DocumentNumber;
            JournalEntryLines.Add(line);
        }
        public bool CheckJELines() //Check the number of lines in the journal entry
        {
            int numberoflines = 0;
            foreach (JournalEntryLine line in JournalEntryLines) numberoflines++;
            if (numberoflines < 1)
            {
                Console.WriteLine($"The Document Number {DocumentNumber} has only 1 line!");
                return false;
            }
            else
            {
                Console.WriteLine($"The Document Number {DocumentNumber} has {numberoflines} lines.");
                Console.WriteLine();
                return true;
            }
        }
        public void PrintJE()
        {
            Console.WriteLine(" Document Number | Account Number | Debit/ Credit | Date | Description");
            Console.WriteLine("----------------------------------------------------------------------------");
            foreach (JournalEntryLine j in JournalEntryLines)
            {
                Console.WriteLine(" {0} | {1} | {2} | {3} | {4} | {5}", j.LineDocumentNumber, j.AccountNumber, j.DrCr, j.Amount, j.LineAddDate, j.LineDesc);
            }
        }
    }
}
