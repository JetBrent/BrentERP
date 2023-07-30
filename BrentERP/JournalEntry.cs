using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
            EntryPostDate = null; // Post date would be added using the InsertPostDate method
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
                return true;
            }
            else
            {
                return false;
            } 
        }

        public void AddLine(JournalEntryLine line) 
        {
            line.LineAddDate = EntryAddDate;
            line.LinePostDate = EntryPostDate; // Post date would be added using the InsertPostDate method
            line.LineDescription = Description;
            line.LineDocumentNumber = DocumentNumber;
            JournalEntryLines.Add(line);
        }

        public void AddLine(int accountNumber, string drCr, decimal amount) // Constructor with properties as input. Will be used for update JE functions
        {
            var line = new JournalEntryLine(accountNumber, drCr, amount);
            line.LineAddDate = EntryAddDate;
            line.LineDescription = Description;
            line.LinePostDate = EntryPostDate; // Post date would be added using the InsertPostDate method
            line.LineDocumentNumber = DocumentNumber;
            JournalEntryLines.Add(line);
        }
        public bool CheckJELines() //Check the number of lines in the journal entry
        {
            int numberoflines = 0;
            foreach (JournalEntryLine line in JournalEntryLines) numberoflines++;
            if (numberoflines < 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void PrintJE()
        {
            Console.WriteLine(" Document Number | Account Number | Debit/ Credit | Date | Description");
            Console.WriteLine("----------------------------------------------------------------------------");
            foreach (JournalEntryLine j in JournalEntryLines)
            {
                Console.WriteLine(" {0} | {1} | {2} | {3} | {4} | {5}", j.LineDocumentNumber, j.LineAccountNumber, j.DrCr, j.Amount, j.LineAddDate, j.LineDescription);
            }
        }

        public List<object[]> EntryToList() // Convert Journal Entry to List of arrays for posting to database
        {
            var list = new List<object[]>();
            foreach (JournalEntryLine line in JournalEntryLines)
            {
                var array = line.LineToArray(line);
                list.Add(array);
            }
            return list;
        }
        
        public List<object[]> PrepareEntryForPosting() // TODO: Implement conversion of datetimes
        {
            EntryPostDate = DateTime.Now;
            foreach (JournalEntryLine line in JournalEntryLines)
            {
                line.LinePostDate = EntryPostDate;
            }
            var balanced = CheckEqual();
            var complete = CheckJELines();
            if (complete && balanced)
            {
                var list = this.EntryToList();
                return list;
            }
            else
            {
                return null;
            }
        }

        public JournalEntry(List<object[]> je) // To be used for posting or when the entry was read from the database and casted to a JournalEntry class
        {
            EntryPostDate = DateTime.Now;
            DocumentNumber = int.Parse(je[0][0].ToString()); // Read Only Data Type was detected. The value was first converted to string before it was parsed
            Description = je[0][6].ToString();
            var linestart = new List<JournalEntryLine> { };
            JournalEntryLines = linestart;
            for (int i = 0; i < je.Count; i++)
            {
                var acc = int.Parse(je[i][1].ToString()); // TODO: Implement registration of account code to SQL database and apply the application of the account code here
                var line = new JournalEntryLine(acc, je[i][2].ToString(), decimal.Parse(je[i][3].ToString()));
                linestart.Add(line);
            }
        }
    }
}
