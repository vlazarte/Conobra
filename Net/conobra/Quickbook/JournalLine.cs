using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class JournalLine
    {
        private Account account;
        private float amount;
        private string company;
        private string type;

        public JournalLine(Account _account, float _amount, string _company, string _type = "credit")
        {
            this.account = _account;
            this.amount = _amount;
            this.company = _company;
            this.type = _type;
        }

        public Account getAccount()
        {
            return account;
        }

        public string toXml()
        {
            string xml = "";

            if ( type == "credit" )
                xml += "<JournalCreditLine>" + Environment.NewLine;
            else
                xml += "<JournalDebitLine>" + Environment.NewLine;

            xml += "<AccountRef>" + Environment.NewLine;
            xml += "<ListID>" + account.ListID + "</ListID> " + Environment.NewLine;
            xml += "</AccountRef>" + Environment.NewLine;
            xml += "<Amount>" + amount.ToString("0.00") + "</Amount>" + Environment.NewLine;
            //xml += "<Memo>" + "ASIENTO FISCAL AUTOM&#193;TICO + FACTURA #" + refNumber + "</Memo>";
            xml += "<EntityRef>" + Environment.NewLine;
            xml += "<FullName>" + Functions.htmlEntity("" + company) + "</FullName> " + Environment.NewLine;
            xml += "</EntityRef>" + Environment.NewLine;

            if (type == "credit")
                xml += "</JournalCreditLine>" + Environment.NewLine;
            else
                xml += "</JournalDebitLine>" + Environment.NewLine;

            return xml;
        }
    }
}
