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
        internal string AccountNo { get; set; }
        internal string DrCr { get; set; }
        internal decimal Amount { get; set; }
        internal DateTime? JELineDate { get; set; }
        internal string? JELineDesc { get; set; }
        internal int? LineDocNum { get; set; }


        public JELine(string accountNo, string drCr, decimal amount)
        {
            try
            {

                if (amount == 0 || amount < 0)
                {
                    throw new ArgumentException($"Amount must not be equal or less than 0!");
                }
                else
                {
                    Amount = amount;
                    AccountNo = accountNo;
                    DrCr = drCr;
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("ArgumentException Error : {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception on JE Class Constructor: {e}");
            }
        }

    }
}
