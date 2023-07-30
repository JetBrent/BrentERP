using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Data.Common;
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
                    "amount DECIMAL(65,5) NOT NULL, add_date DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, post_date DATETIME DEFAULT NULL, description VARCHAR(1000) NOT NULL )";
                cmd = new MySqlCommand(jlcreate, con);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Journal Table Initialized...");

                string glcreate = "CREATE TABLE IF NOT EXISTS " +
                    "general_ledger(id INT PRIMARY KEY AUTO_INCREMENT, document_number VARCHAR(256) NOT NULL, account_number VARCHAR(256) NOT NULL, drcr VARCHAR(10) NOT NULL, " +
                    "amount DECIMAL(65,5) NOT NULL, add_date DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, post_date DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL, description VARCHAR(1000) NOT NULL )";
                cmd = new MySqlCommand(glcreate, con);
                cmd.ExecuteNonQuery();
                Console.WriteLine("General Ledger Table Initialized...");

                string accountcreate = "CREATE TABLE IF NOT EXISTS account_code(account_number VARCHAR(256) PRIMARY KEY, account_name VARCHAR(256))";
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

        public List<object[]> ReadAllEntriesFromJournalLedger(MySqlConnection con) // Reads all the records from an input table an returns a list of arrays
        {
            con.Open();
            string selectentries = "SELECT * FROM journal_ledger";
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

        public List<object[]> ReadAllEntriesFromGeneralLedger(MySqlConnection con) // Reads all the records from an input table an returns a list of arrays
        {
            con.Open();
            string selectentries = "SELECT * FROM general_ledger";
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
                var line = new object[] { document_number, account_number, drcr, amount, add_date, post_date, description };
                dblines.Add(line);
            }
            con.Close();
            return dblines;
        }

        public List<object[]> ReadAllRegisteredAccountCodes(MySqlConnection con) // Reads all the registered account codes and returns a list of arrays
        {
            con.Open();
            string selectentries = "SELECT * FROM account_code";
            MySqlCommand cmd = new MySqlCommand(selectentries, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            var dblines = new List<object[]>();
            while (reader.Read())
            {
                string account_name = reader.GetString("account_name");
                string account_number = reader.GetString("account_number");
                var line = new object[] { account_number, account_name };
                dblines.Add(line);
            }
            con.Close();
            return dblines;
        }
        public List<object[]> QueryFromJournalLedger(MySqlConnection con, string column, string condition)
        {
            con.Open();
            string querydb = "SELECT * FROM journal_ledger WHERE @column = @condition";
            MySqlCommand cmd = new MySqlCommand(querydb, con);
            cmd.Parameters.AddWithValue("@column", column);
            cmd.Parameters.AddWithValue("@condition", condition);
            MySqlDataReader reader = cmd.ExecuteReader();
            var dblines = new List<object[]>();
            while (reader.Read())
            {
                string document_number = reader.GetString("document_number");
                string account_number = reader.GetString("account_number");
                string drcr = reader.GetString("drcr");
                decimal amount = reader.GetDecimal("amount");
                DateTime add_date = DateTime.Parse(reader.GetString("add_date"));
                DateTime post_date = DateTime.Parse(reader.GetString("post_date"));
                string description = reader.GetString("description");
                var line = new object[] { document_number, account_number, drcr, amount, add_date, post_date, description };
                dblines.Add(line);
            }
            if (reader.HasRows)
            {
                Console.WriteLine($"Query for {condition} is successful!");
                con.Close();
                return dblines;
            }

            else
            {
                Console.WriteLine($"Query for {condition} has failed.");
                con.Close();
                dblines = null;
                return dblines;
            }
        }

        public List<object[]> QueryFromGeneralLedger(MySqlConnection con, string column, string condition)
        {
            con.Open();
            string querydb = "SELECT * FROM general_ledger WHERE @column = @condition";
            MySqlCommand cmd = new MySqlCommand(querydb, con);
            cmd.Parameters.AddWithValue("@column", column);
            cmd.Parameters.AddWithValue("@condition", condition);
            MySqlDataReader reader = cmd.ExecuteReader();
            var dblines = new List<object[]>();
            while (reader.Read())
            {
                string document_number = reader.GetString("document_number");
                string account_number = reader.GetString("account_number");
                string drcr = reader.GetString("drcr");
                decimal amount = reader.GetDecimal("amount");
                DateTime add_date = DateTime.Parse(reader.GetString("add_date"));
                DateTime post_date = DateTime.Parse(reader.GetString("post_date"));
                string description = reader.GetString("description");
                var line = new object[] { document_number, account_number, drcr, amount, add_date, post_date, description };
                dblines.Add(line);
            }
            if (reader.HasRows)
            {
                Console.WriteLine($"Query for {condition} is successful!");
                con.Close();
                return dblines;
            }

            else
            {
                Console.WriteLine($"Query for {condition} has failed.");
                con.Close();
                dblines = null;
                return dblines;
            }
        }

        public string QueryAccountCode(MySqlConnection con, int intaccountno)
        {
            con.Open();
            string accountno = intaccountno.ToString();
            string querydb = "SELECT account_number FROM account_code WHERE account_number = @accountno";
            MySqlCommand cmd = new MySqlCommand(querydb, con);
            cmd.Parameters.AddWithValue("@accountno", accountno);
            MySqlDataReader reader = cmd.ExecuteReader();
            string result = null; // Initialize to null

            if (reader.HasRows) // Check if the reader contains any rows
            {
                while (reader.Read())
                {
                    string account_number = reader.GetString("account_number");
                    result = account_number;
                }
            }
            con.Close();
            return result;
        }

        public void AddGLAccount(MySqlConnection con, int accountno, string accountname)
        {
            con.Open();
            string insertacc = "INSERT INTO account_code (account_number, account_name) VALUES (@accountno, @accountname)";
            MySqlCommand cmd = new MySqlCommand(insertacc, con);
            cmd.Parameters.AddWithValue("@accountno", accountno);
            cmd.Parameters.AddWithValue("@accountname", accountname);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine($"GL Account {accountno} {accountname} was successfully registered.");
            }
            else
            {
                Console.WriteLine($"Failed to register GL Account {accountno} {accountname}.");
            }
            con.Close();
            Console.ReadKey();
        }

        public void AddJournalEntry(MySqlConnection con, List<object[]> journalentry)
        {
            con.Open();
            var documentnumber = journalentry[0][0];
            for (int i=0; i<journalentry.Count; i++)
            {
                string insertentry = "INSERT INTO journal_ledger (document_number, account_number, drcr, amount, add_date, post_date, description) " +
                "VALUES (@document_number, @account_number, @drcr, @amount, @add_date, @post_date, @description)";
                MySqlCommand cmd = new MySqlCommand(insertentry, con);
                cmd.Parameters.AddWithValue("@document_number", journalentry[i][0]);
                cmd.Parameters.AddWithValue("@account_number", journalentry[i][1]);
                cmd.Parameters.AddWithValue("@drcr", journalentry[i][2]);
                cmd.Parameters.AddWithValue("@amount", journalentry[i][3]);
                cmd.Parameters.AddWithValue("@add_date", journalentry[i][4]);
                cmd.Parameters.AddWithValue("@post_date", journalentry[i][5]);
                cmd.Parameters.AddWithValue("@description", journalentry[i][6]);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Journal Entry Number {documentnumber} was successfully added to the journal ledger.");

                }
                else
                {
                    Console.WriteLine($"Failed to insert journal entry no. {documentnumber}. Please try again.");
                }
            }
            con.Close();
            Console.WriteLine($"Journal Entry Number {journalentry[0][0]} was successfully added.");
            Console.ReadKey();
        }

        public void PostJournalEntryToGeneralLedger(MySqlConnection con, List<object[]> journalentry)
        {
            con.Open();
            int insertrowsaffected = 0;
            var documentnumber = journalentry[0][0];
            for (int i = 0; i <= journalentry.Count; i++)
            {
                string insertentry = "INSERT INTO general_ledger (document_number, account_number, drcr, amount, add_date, post_date, description) " +
                "VALUES (@document_number, @account_number, @drcr, @amount, @add_date, @post_date, @description)";
                MySqlCommand cmd = new MySqlCommand(insertentry, con);
                cmd.Parameters.AddWithValue("@document_number", journalentry[i][0]);
                cmd.Parameters.AddWithValue("@account_number", journalentry[i][1]);
                cmd.Parameters.AddWithValue("@drcr", journalentry[i][2]);
                cmd.Parameters.AddWithValue("@amount", journalentry[i][3]);
                cmd.Parameters.AddWithValue("@add_date", journalentry[i][4]);
                cmd.Parameters.AddWithValue("@post_date", journalentry[i][5]);
                cmd.Parameters.AddWithValue("@description", journalentry[i][6]);
                insertrowsaffected += cmd.ExecuteNonQuery();
            }
            string deleteentry = "DELETE FROM journal_ledger WHERE document_number = '@document_number'";
            MySqlCommand cmd2 = new MySqlCommand(deleteentry, con);
            cmd2.Parameters.AddWithValue("@document_number", journalentry[0][0]);
            int deleterowsaffected = cmd2.ExecuteNonQuery();
            if (insertrowsaffected > 0 && deleterowsaffected > 0 )
            {
                Console.WriteLine($"Journal Entry Number {documentnumber} was successfully posted to the General Ledger.");

            }
            else
            {
                Console.WriteLine($"Journal entry no. {documentnumber} posting to General Ledger has failed.");
            }
            con.Close();
            Console.ReadKey();
        }

        public void DeleteSavedJournalEntry(MySqlConnection con, string documentnumber)
        {
            con.Open();
            string deleteentry = "DELETE FROM journal_ledger WHERE document_number = '@documentnumber'"; // Journal Entries posted to the General Ledger cannot be deleted.
            MySqlCommand cmd = new MySqlCommand(deleteentry, con);
            cmd.Parameters.AddWithValue("@documentnumber", documentnumber);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine($"Journal Entry Number {documentnumber} was successfully deleted from journal ledger.");

            }
            else
            {
                Console.WriteLine($"Failed to delete journal entry no. {documentnumber}. Journal entry is not found in the journal ledger.");
            }
            con.Close();
        }

        public int GetUniqueDocumentNumber(MySqlConnection con)
        {
            con.Open(); //Search for the latest document number in the journal ledger 
            string selectentries = "SELECT document_number FROM journal_ledger ORDER BY document_number DESC LIMIT 1";
            MySqlCommand cmd = new MySqlCommand(selectentries, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            int journaldocumentnumber = 0;
            while (reader.Read())
            {
                string document_number = reader.GetString("document_number");
                bool booldocumentnumber = Int32.TryParse(document_number, out journaldocumentnumber);
            }
            con.Close(); //MySqlDataReader must first be closed to initiate another SQL command

            con.Open(); //Search for the latest document number in the general ledger 
            selectentries = "SELECT document_number FROM general_ledger ORDER BY document_number DESC LIMIT 1";
            cmd = new MySqlCommand(selectentries, con);
            reader = cmd.ExecuteReader();
            int generaldocumentnumber = 0;
            while (reader.Read()) 
            {
                string document_number = reader.GetString("document_number");
                bool booldocumentnumber = Int32.TryParse(document_number, out generaldocumentnumber);
            }
            con.Close();
            if (journaldocumentnumber == 0) journaldocumentnumber = 1;
            else journaldocumentnumber++;

            if (journaldocumentnumber == generaldocumentnumber) journaldocumentnumber++; // Compares the latest document number from the journal ledger and from the general ledger. If they are equal, the journal ledger document number is incremented


            return journaldocumentnumber;
        }
    }
}
