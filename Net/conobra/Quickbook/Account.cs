using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;

namespace Quickbook
{
    /// <summary>
    /// 
    /// </summary>
    public class Account
    {
        public string ListID;
        public string TimeCreated;
        public string EditSequence;
        public string Name;
        public string FullName;
        public bool IsActive;
        public string Sublevel;
        public string AccountType;
        public int AccountNumber;
        public float Balance;
        public float TotalBalance;
        public string CashFlowClassification;
        public string Currency;

        public override string ToString()
        {
            return AccountNumber + " - " + ListID;
        }

        public static Hashtable getAccounts()
        {
            Hashtable list = new Hashtable();

            string xml = "<?xml version=\"1.0\" ?>";
            xml += "<?qbxml version=\"12.0\"?>";
            xml += "<QBXML>";
            xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
            xml += "<AccountQueryRq requestID=\"13\">";
            xml += "</AccountQueryRq>";
            xml += "</QBXMLMsgsRq>";
            xml += "</QBXML>";

            if (Vars.qb != null && Vars.qb.isOpen())
            {
                var response = "" ;
                if (Vars.isLocalhost() == true)
                {
                    response = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "/xml-debug/accounts.xml");
                } 
                else
                    response = Vars.qb.sendRequest(xml);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNodeList rets = doc.SelectNodes("/QBXML/QBXMLMsgsRs/AccountQueryRs/AccountRet");
                string message = string.Empty;

                foreach (XmlNode node in rets)
                {
                    Account ac = new Account();
                    ac.ListID = node["ListID"].InnerText;
                    ac.TimeCreated = node["TimeCreated"].InnerText;
                    ac.EditSequence = node["EditSequence"].InnerText;
                    ac.Name = node["Name"].InnerText;
                    ac.FullName = node["FullName"].InnerText;
                    ac.IsActive = (bool)(node["IsActive"].InnerText == "true");
                    ac.Sublevel = node["Sublevel"].InnerText;
                    ac.AccountType = node["AccountType"].InnerText;
                    ac.AccountNumber = Int32.Parse(node["AccountNumber"].InnerText);
                    ac.Balance = float.Parse(node["Balance"].InnerText);
                    ac.TotalBalance = float.Parse(node["TotalBalance"].InnerText);
                    ac.CashFlowClassification = node["CashFlowClassification"].InnerText;
                    ac.Currency = node["CurrencyRef"]["FullName"].InnerText;

                    //list.Add(node["AccountNumber"].InnerText, ac);
                    list[node["AccountNumber"].InnerText] = ac ;

                }

            }

            return list;
        }

    }
}
