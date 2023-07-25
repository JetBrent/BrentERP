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
        public int AccountNumber { get; set; }
        public string DrCr { get; set; } // Debit or Credit
        public decimal Amount { get; set; }
        public DateTime? LineAddDate { get; set; }
        public DateTime? LinePostDate { get; set; }
        public string? LineDesc { get; set; }
        public int? LineDocumentNumber { get; set; }

        // Nullable properties are to be provided by the journal entry once posted
        public JournalEntryLine(AccountCode accountNo, string drCr, decimal amount)
        {
            {
                Amount = amount;
                AccountNumber = accountNo.AccountNumber;
                DrCr = drCr;
            }
        }
    }
}
