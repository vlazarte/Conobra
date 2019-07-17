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
    public class Account:Abstract
    {



        public string ListID { get; set; }
        public string TimeCreated { get; set; }
        public string EditSequence { get; set; }
        public string Name { get; set; }
        public string FullName{get;set;}
        public bool IsActive { get; set; }
        public Account ParentRef{get;set;}        
        public string Sublevel { get; set; }
        public string AccountType { get; set; }
        public string SpecialAccountType { get; set; }
        public string BankNumber{get;set;}
        public string Desc{get;set;}
        public string AccountNumber { get; set; }
        public Decimal Balance { get; set; }
        public Decimal TotalBalance { get; set; }
        public TaxLineInfo TaxLineInfoRet{get;set;}

        public string CashFlowClassification { get; set; }
        public Currency CurrencyRef { get; set; }
        public Account() {

            ListID = string.Empty;
            FullName = string.Empty;
        }
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
                    ac.AccountNumber = node["AccountNumber"].InnerText;
                    ac.Balance =Convert.ToDecimal(node["Balance"].InnerText );
                    ac.TotalBalance = Convert.ToDecimal(node["TotalBalance"].InnerText);
                    ac.CashFlowClassification = node["CashFlowClassification"].InnerText;
                    if (node.SelectNodes("CurrencyRef").Count > 0)
                    {
                        ac.CurrencyRef = new Currency();
                        if (node["CurrencyRef"]["ListID"] != null)
                        {
                            ac.CurrencyRef.ListID = node["CurrencyRef"]["ListID"].InnerText;
                        }
                        if (node["CurrencyRef"]["FullName"] != null)
                        {
                            ac.CurrencyRef.FullName = node["CurrencyRef"]["FullName"].InnerText;
                        }
                    }                    

                    //list.Add(node["AccountNumber"].InnerText, ac);
                    list[node["AccountNumber"].InnerText] = ac ;

                }

            }

            return list;
        }


        public string toXmlRef()
        {
            StringBuilder xml = new StringBuilder();
            XmlElement ele = (new XmlDocument()).CreateElement("test");
            xml.Append("<AccountRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {
                ele.InnerText = FullName + "";
                xml.Append("<FullName>" + ele.InnerXml + "</FullName>"); //-- required -->
            }

            xml.Append("</AccountRef>");


            return xml.ToString();
        }

         public string toXmlRefBill()
        {
            StringBuilder xml = new StringBuilder();
            XmlElement ele = (new XmlDocument()).CreateElement("test");
            xml.Append("<APAccountRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {
                ele.InnerText = FullName + "";
                xml.Append("<FullName>" + ele.InnerXml + "</FullName>"); //-- required -->
            }

            xml.Append("</APAccountRef>");
            return xml.ToString();
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet Account";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Account";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Account";
            return new List<Abstract>();
        }
        

    }
}
