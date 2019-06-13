using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public class Journal
    {
        private List<JournalLine> debits;
        private List<JournalLine> credits;
        private DateTime txnDate;
        private string refNumber;
    
        public Journal(DateTime _txnDate, string _refNumber)
        {
            txnDate = _txnDate;
            refNumber = _refNumber;
            debits = new List<JournalLine>();
            credits = new List<JournalLine>();
        }

        public List<JournalLine> getDebits()
        {
            return debits;
        }

        public List<JournalLine> getCredits()
        {
            return credits;
        }

        public void addCreditLine(Account account, float amount, string company)
        {
            JournalLine line = new JournalLine(account, amount, company, "credit");
            credits.Add(line);
        }

        public void addDebitLine(Account account, float amount, string company)
        {
            JournalLine line = new JournalLine(account, amount, company, "debit");
            debits.Add(line);
        }

        public XmlDocument create()
        {
            string msg = Vars.qb.sendRequest(toXml());
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msg);
            return doc;
        }


        public string toXml()
        {
            string xml = "";
            xml += "<?xml version=\"1.0\" ?>" + Environment.NewLine;
            xml += "<?qbxml version=\"12.0\"?>" + Environment.NewLine;
            xml += "<QBXML>" + Environment.NewLine;
            xml += "<QBXMLMsgsRq onError = \"stopOnError\">" + Environment.NewLine;
            xml += "<JournalEntryAddRq requestID = \"1\">" + Environment.NewLine;
            xml += "<JournalEntryAdd>" + Environment.NewLine;
            xml += "<TxnDate>" + txnDate.ToString("yyyy-MM-dd") + "</TxnDate>" + Environment.NewLine;
            xml += "<RefNumber>" + refNumber + "</RefNumber>" + Environment.NewLine;

            foreach( JournalLine line in debits ){
                xml += line.toXml();
            }

            foreach( JournalLine line in credits ){
                xml += line.toXml();
            }

            xml += "</JournalEntryAdd>" + Environment.NewLine;
            xml += "</JournalEntryAddRq>" + Environment.NewLine;
            xml += "</QBXMLMsgsRq>" + Environment.NewLine;
            xml += "</QBXML>";

            return xml;
        }


        

    }

    
}
