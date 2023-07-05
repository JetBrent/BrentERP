using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BrentERP
{
    internal class JournalEntry
    {
        private protected DateTime JEDate;
        private protected int DocumentNo;
        private protected List<JELine> JELines;
        private protected string JEDescription;

        internal JournalEntry(int documentNo, string jEDescription)
        {
            JEDate = DateTime.Now;
            DocumentNo = documentNo;
            JEDescription = jEDescription;

        }

        internal List<JELine> GetJE()
        {
            return JELines;
        }

        internal void AddLine(JELine line)
        {
            line.JELineDate = JEDate;
            line.JELineDesc = JEDescription;
            line.LineDocNum = DocumentNo;
            JELines.Add(line);
        }

        internal void AddLine(string accountNo, string drCr, decimal amount)
        {
            var line = new JELine(accountNo, drCr, amount);
            line.JELineDate = JEDate;
            line.JELineDesc = JEDescription;
            line.LineDocNum = DocumentNo;
            JELines.Add(line);
        }



    }
}
