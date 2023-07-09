using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrentERP
{
    internal class JELine
    {
        public int AccountNo { get; set; }
        public string DrCr { get; set; }
        public decimal Amount { get; set; }
        public DateTime? JELineDate { get; set; }
        public string? JELineDesc { get; set; }
        public int? LineDocNum { get; set; }


        public JELine(AccountCode accountNo, string drCr, decimal amount)
        {
            {
                Amount = amount;
                AccountNo = accountNo.AccountNumber;
                DrCr = drCr;
            }
        }
    }
}
