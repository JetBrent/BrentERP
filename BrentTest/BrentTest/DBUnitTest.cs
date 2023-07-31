using BrentSQLDB;

namespace BrentTest
{
    [TestClass]
    public class DBUnitTest
    {
        [TestMethod]
        public void The_Query_Of_Account_Number_10000_Is_10000()
        {
            int input = 10000;
            string expected = input.ToString();
            var db = new BrentDB();
            var con = db.ConnectToDB("localhost", "root", "MYBRENT.sql!");
            var result = db.QueryAccountCode(con, input);
            Assert.AreEqual(expected, result, $"Query for {input} resulted to {result}");

        }

        [TestMethod]
        public void The_Query_Of_Account_Number_00000_Is_Null()
        {
            int input = 0000;
            string expected = null;
            var db = new BrentDB();
            var con = db.ConnectToDB("localhost", "root", "MYBRENT.sql!");
            var result = db.QueryAccountCode(con, input);
            Assert.AreEqual(expected, result, $"Query for {input} resulted to {result}");

        }

        [TestMethod]
        public void The_Query_Of_Document_Number_0_In_The_Journal_Ledger_Is_Null()
        {
            string input = "0";
            string expected = null;
            var db = new BrentDB();
            var con = db.ConnectToDB("localhost", "root", "MYBRENT.sql!");
            var result = db.QueryFromJournalLedger(con, input);
            Assert.AreEqual(expected, result, $"Query for {input} resulted to {result}");
        }

        [TestMethod]
        public void The_Query_Of_Document_Number_1_In_The_Journal_Ledger_Is_Not_Null()
        {
            string input = "1";
            bool expectedbool = true;
            var db = new BrentDB();
            var con = db.ConnectToDB("localhost", "root", "MYBRENT.sql!");
            var result = db.QueryFromJournalLedger(con, input);
            bool resultbool = false;
            if (result != null)
            {
                resultbool = true;
            }
            Assert.AreEqual(expectedbool, resultbool, $"Query for {input} resulted to {result}");
        }
    }
}