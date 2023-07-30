using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BrentERP
{
    internal class GeneralLedgerAccount
    {
        public int AccountNumber { get; set; }

        public string AccountName { get; set; }

        public GeneralLedgerAccount(int accountNumber, string accountName)
        {
            AccountNumber = accountNumber;
            AccountName = accountName;
        }
    }
}
