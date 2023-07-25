using MySql.Data.MySqlClient;

namespace BrentSQLDB
{
    public class BrentDB
    {
        public void Init(string server, string uid, string password)
        {
            string connstring = string.Format("server={0};uid={1};password={2};",server, uid, password);
            MySqlConnection con = new MySqlConnection(connstring);
            con.ConnectionString = connstring;

            con.Open();
            string dbcreate = "CREATE DATABASE IF NOT EXISTS brentdb";
            MySqlCommand cmd = new MySqlCommand(dbcreate, con);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Database Created");

            string usedb = "USE brentdb";
            cmd = new MySqlCommand(usedb, con);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Database brentdb Used");

            string slcreate = "CREATE TABLE IF NOT EXISTS subsidiary_ledger(id INT PRIMARY KEY AUTO_INCREMENT, document_number VARCHAR(256) NOT NULL, account_number VARCHAR(256) NOT NULL, drcr VARCHAR(10) NOT NULL, amount DECIMAL(65,5) NOT NULL, add_date VARCHAR(100) NOT NULL, post_date VARCHAR(100) NOT NULL, description VARCHAR(1000) )";
            cmd = new MySqlCommand(slcreate, con);
            cmd.ExecuteNonQuery();
            Console.WriteLine("SL Done");

            string glcreate = "CREATE TABLE IF NOT EXISTS general_ledger(id INT PRIMARY KEY AUTO_INCREMENT, document_number VARCHAR(256) NOT NULL, account_number VARCHAR(256) NOT NULL, drcr VARCHAR(10) NOT NULL, amount DECIMAL(65,5) NOT NULL, add_date VARCHAR(100) NOT NULL, post_date VARCHAR(100) NOT NULL, description VARCHAR(1000) )";
            cmd = new MySqlCommand(glcreate, con);
            cmd.ExecuteNonQuery();
            Console.WriteLine("GL Done");

            string accountcreate = "CREATE TABLE IF NOT EXISTS account_code(account_name VARCHAR(256), account_number VARCHAR(256) PRIMARY KEY)";
            cmd = new MySqlCommand(accountcreate, con);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Account Code Done");
            con.Close();

            Console.WriteLine("Init finished.");
        }
        
        public MySqlConnection Connect(string server, string uid, string password)
        {
            string connstring = string.Format("server={0};uid={1};password={2};", server, uid, password);
            MySqlConnection con = new MySqlConnection(connstring);
            con.ConnectionString = connstring;
            return con;
        }

        public List<object[]> ReadFromDatabase(MySqlConnection con, string table)
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
    }
}
