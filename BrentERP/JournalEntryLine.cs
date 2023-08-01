using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrentERP
{
    internal class JournalEntryLine
    {
        public int LineAccountNumber;
        public string DrCr; // Debit or Credit
        public decimal Amount;
        public DateTime? LineAddDate;
        public DateTime? LinePostDate;
        public string? LineDescription;
        public int? LineDocumentNumber;

        // Nullable properties are to be provided by the journal entry once posted
        public JournalEntryLine(int accountNo, string drCr, decimal amount)
        {
            {
                Amount = amount;
                LineAccountNumber = accountNo;
                DrCr = drCr;
            }
        }

        public object[] LineToArray(JournalEntryLine line) // Converts the line elements from the insides of the array to either string or decimal
        {
            DateTime? addDate = line.LineAddDate;
            string formatAddDate = addDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime? postDate = line.LinePostDate;
            string formatPostDate = postDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var returnarray = new object[] {line.LineDocumentNumber, line.LineAccountNumber, line.DrCr, line.Amount, formatAddDate, formatPostDate, line.LineDescription };
            return returnarray;
        }
    }
}
