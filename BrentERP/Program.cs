using BrentSQLDB;

namespace BrentERP
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Utils.InitDB();
            Console.WriteLine("Program finished.");
            Console.ReadKey();



            // TODO: Create Account Number Class
            // TODO: Add SQL Code to add journal entry details to the journal entry database
        }
    }
}