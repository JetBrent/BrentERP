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
        public int LineAccountNumber { get; set; }
        public string DrCr { get; set; } // Debit or Credit
        public decimal Amount { get; set; }
        public DateTime? LineAddDate { get; set; }
        public DateTime? LinePostDate { get; set; }
        public string? LineDescription { get; set; }
        public int? LineDocumentNumber { get; set; }

        // Nullable properties are to be provided by the journal entry once posted
        public JournalEntryLine(GeneralLedgerAccount accountNo, string drCr, decimal amount)
        {
            {
                Amount = amount;
                LineAccountNumber = accountNo.AccountNumber;
                DrCr = drCr;
            }
        }

        public object[] LineToArray(JournalEntryLine line)
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
