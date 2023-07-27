using BrentSQLDB;

namespace BrentERP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Utils.InitDB();
            var con = Utils.ConnectToDB();
            Utils.MainMenu(con);
            Console.ReadKey();



            // TODO: Create Account Number Class
            // TODO: Add SQL Code to add journal entry details to the journal entry database
        }
    }
}