using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrentERP
{


    public class JELine
    {
        public string AccountNo { get; set; }
        public string DrCr { get; set; }
        public decimal Amount { get; set; }
        public DateTime? JELineDate { get; set; }
        public string? JELineDesc { get; set; }
        public int? LineDocNum { get; set; }


        public JELine(string accountNo, string drCr, decimal amount)
        {
            try
            {

                if (amount == 0 || amount < 0)
                {
                    throw new ArgumentException("Amount must not be equal or less than 0!");
                }
                else if (DrCr != "Debit" || DrCr != "Credit")
                {
                    throw new ArgumentException("Please input either \"Debit\" or \"Credit\"!");
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
