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
            EntryPostDate = EntryAddDate; // Post date would be added using the InsertPostDate method
            DocumentNumber = documentNumber;
            Description = description;
            JournalEntryLines = new List<JournalEntryLine> { };
        }

        public List<JournalEntryLine> GetJE()
        {
            return JournalEntryLines;

        }

        public bool CheckAmountBalance()
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
            line.LinePostDate = EntryPostDate; // Post date would be added using the constructor
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
        public bool CheckLineComplete() //Check the number of lines in the journal entry
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
                Console.WriteLine(" {0} | {1} | {2} | {3} | {4} | {5}", j.LineDocumentNumber, j.LineAccountNumber, j.DrCr, Decimal.Round(j.Amount,2), j.LineAddDate, j.LineDescription);
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
        
        public List<object[]> CheckEntryForPosting() // Perform record and total checks and converts the journal entry into a List<object>
        {
            var balanced = CheckAmountBalance();
            var complete = CheckLineComplete();
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


            DocumentNumber = int.Parse(je[0][0].ToString());

            Description = je[0][6].ToString();

            EntryAddDate = DateTime.Parse(je[0][4].ToString());

            EntryPostDate = DateTime.Parse(je[0][5].ToString());

            if (EntryPostDate == EntryAddDate)
            {
                EntryPostDate = DateTime.Now;
            }

            JournalEntryLines = new List<JournalEntryLine> { };

            for (int i = 0; i < je.Count; i++)
            {
                var acc = int.Parse(je[i][1].ToString());
                var drcr = je[i][2].ToString();
                var amount = decimal.Parse(je[i][3].ToString());
                var resultline = new JournalEntryLine(acc, drcr, amount); 
                AddLine(resultline); //Uses the constructor for the AddLine method that gets the values of the current Journal Entry and adds them to every line
            }

        }
    }
}
