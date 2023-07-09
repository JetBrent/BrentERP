using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BrentERP
{
    internal class AccountCode
    {
        public int AccountNumber { get; set; }

        public string AccountName { get; set; }

        public AccountCode(int accountNumber, string accountName)
        {
            AccountNumber = accountNumber;
            AccountName = accountName;
        }
    }
}
