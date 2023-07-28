using MySql.Data.MySqlClient;
using System.Reflection.PortableExecutable;

namespace BrentSQLDB
{
    public class BrentDB
    {
        public void Init(string server, string uid, string password) // Intializing the database and tables needed for the functions
        {
            try
            {
                Console.WriteLine("BrentDB Database Initialization has started.");

                string connstring = string.Format("server={0};uid={1};password={2};", server, uid, password);
                MySqlConnection con = new MySqlConnection(connstring);
                con.ConnectionString = connstring;

                con.Open();
                string dbcreate = "CREATE DATABASE IF NOT EXISTS brentdb";
                MySqlCommand cmd = new MySqlCommand(dbcreate, con);
                cmd.ExecuteNonQuery();
                Console.WriteLine("BrentDB Database has been created...");

                string usedb = "USE brentdb";
                cmd = new MySqlCommand(usedb, con);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Database Connection Successful. Creating Tables...");

                string jlcreate = "CREATE TABLE IF NOT EXISTS " +
                    "journal_ledger(id INT PRIMARY KEY AUTO_INCREMENT, document_number VARCHAR(256) NOT NULL, account_number VARCHAR(256) NOT NULL, drcr VARCHAR(10) NOT NULL, " +
                    "amount DECIMAL(65,5) NOT NULL, add_date VARCHAR(100) NOT NULL, post_date VARCHAR(100) NOT NULL, description VARCHAR(1000) NOT NULL )";
                cmd = new MySqlCommand(jlcreate, con);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Journal Table Initialized...");

                string glcreate = "CREATE TABLE IF NOT EXISTS " +
                    "general_ledger(id INT PRIMARY KEY AUTO_INCREMENT, document_number VARCHAR(256) NOT NULL, account_number VARCHAR(256) NOT NULL, drcr VARCHAR(10) NOT NULL, " +
                    "amount DECIMAL(65,5) NOT NULL, add_date VARCHAR(100) NOT NULL, post_date VARCHAR(100) NOT NULL, description VARCHAR(1000) NOT NULL )";
                cmd = new MySqlCommand(glcreate, con);
                cmd.ExecuteNonQuery();
                Console.WriteLine("General Ledger Table Initialized...");

                string accountcreate = "CREATE TABLE IF NOT EXISTS account_code(account_name VARCHAR(256), account_number VARCHAR(256) PRIMARY KEY)";
                cmd = new MySqlCommand(accountcreate, con);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Account Code Table Initialized...");
                con.Close();

                Console.WriteLine("BrentERP Database Initialization Successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception has occurred on Databnase Initialization : {ex.Message}");
            }
        }
        
        public MySqlConnection ConnectToDB(string server, string uid, string password) // Connection to database will be successful if database initialization was successful
        {
            string connstring = string.Format("server={0};uid={1};password={2};database=brentdb;", server, uid, password);
            MySqlConnection con = new MySqlConnection(connstring);
            con.ConnectionString = connstring;
            return con;
        }

        public List<object[]> ReadAllFromDatabase(MySqlConnection con, string table) // Reads all the records from an input table an returns a list of arrays
        {
            con.Open();
            table = table.ToLower();
            string selectentries = string.Format("SELECT * FROM {0}", table);
            MySqlCommand cmd = new MySqlCommand(selectentries, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            var dblines = new List<object[]>();
            while (reader.Read())
            {
                string document_number = reader.GetString("document_number");
                string account_number = reader.GetString("account_number");
                string drcr = reader.GetString("drcr");
                decimal amount = reader.GetDecimal("amount");
                string add_date = reader.GetString("add_date");
                string post_date = reader.GetString("post_date");
                string description = reader.GetString("description");
                var line = new object[] { document_number, account_number, drcr, amount, add_date, post_date, description};
                dblines.Add(line);
            }
            con.Close();
            return dblines;
        }
        public List<object[]> QueryDB(MySqlConnection con, string table, string column, string condition )
        {
            con.Open();
            string querydb = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", table, column, condition);
            MySqlCommand cmd = new MySqlCommand(querydb, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            var dblines = new List<object[]>();
            while (reader.Read())
            {
                string document_number = reader.GetString("document_number");
                string account_number = reader.GetString("account_number");
                string drcr = reader.GetString("drcr");
                decimal amount = reader.GetDecimal("amount");
                string add_date = reader.GetString("add_date");
                string post_date = reader.GetString("post_date");
                string description = reader.GetString("description");
                var line = new object[] { document_number, account_number, drcr, amount, add_date, post_date, description };
                dblines.Add(line);
            }
            if (reader.RecordsAffected > 0) Console.WriteLine($"Query for {condition} is successful!");
            else
            {
                Console.WriteLine($"Query for {condition} has failed.");
                con.Close();
                return null;
            }

            con.Close();
            return dblines;
        }

        public void AddJournalEntry(MySqlConnection con, List<object[]> journalentry)
        {
            con.Open();
            for (int i=0; i<journalentry.Count; i++)
            {
                string insertentry = string.Format("INSERT INTO journal_ledger (document_number, account_number, drcr, amount, add_date, post_date, description) " +
                "VALUES ('{0}', '{1}', '{2}', {3}, '{4}', '{5}', '{6}')",
                journalentry[i][0], journalentry[i][1], journalentry[i][2], journalentry[i][3], journalentry[i][4], journalentry[i][5], journalentry[i][6]);
                MySqlCommand cmd = new MySqlCommand(insertentry, con);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.RecordsAffected <= 0)
                    {
                        Console.WriteLine($"Failed to insert journal entry no. {journalentry[0][0]}.");
                    }
                }
            }
            con.Close();
            Console.WriteLine($"Journal Entry Number {journalentry[0][0]} was successfully added.");
            Console.ReadKey();
        }

        public void PostJournalEntry(MySqlConnection con, List<object[]> journalentry)
        {
            con.Open();
            for (int i = 0; i <= journalentry.Count; i++)
            {
                string insertentry = string.Format("INSERT INTO general_ledger (document_number, account_number, drcr, amount, add_date, post_date, description) " +
                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
                journalentry[i][0], journalentry[i][1], journalentry[i][2], journalentry[i][3], journalentry[i][4], journalentry[i][5], journalentry[i][6]);
                MySqlCommand cmd = new MySqlCommand(insertentry, con);
            }

            for (int i = 0; i <= journalentry.Count; i++)
            {
                string insertentry = string.Format("DELETE FROM journal_ledger WHERE document_number = '{0}'", journalentry[0][0]);
                MySqlCommand cmd = new MySqlCommand(insertentry, con);
            }
            con.Close();
            Console.WriteLine($"Journal Entry Number {journalentry[0][0]} was successfully added.");
            Console.ReadKey();
        }
    }
}
