using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Quickbook
{
    public class Currency
    {
        public string ListID {get; set;}
        public string FullName { get; set; }

        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        public string EditSequence { get; set; }
        public string Name { get; set; }
        public bool IsActive {get; set;}
        public string CurrencyCode { get; set; }

        public string Format_ThousandSeparator { get; set; }
        public string Format_ThousandSeparatorGrouping { get; set; }
        public string Format_DecimalPlaces { get; set; }
        public string Format_DecimalSeparator { get; set; }

        public bool? IsUserDefinedCurrency { get; set; }
        public float? ExchangeRate { get; set; }
        public DateTime? AsOfDate { get; set; }

        public Currency() {
            ListID = string.Empty;
            FullName = string.Empty;
        }

        /*
         <CurrencyRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</CurrencyRef>
         */
        public string toXmlRef()
        {
            StringBuilder xmlAdd= new StringBuilder();
            xmlAdd.Append( "<CurrencyRef>");
            if (ListID != ""){
            xmlAdd.Append( " <ListID>" + ListID + "</ListID>" );
            }
            if (FullName != ""){
            xmlAdd.Append( " <FullName>"+FullName+"</FullName>");
            }
            xmlAdd.Append( " </CurrencyRef>");

            return xmlAdd.ToString();
        }

        public static List<Currency> getList()
        {
            List<Currency> list = new List<Currency>();

            string path = Directory.GetCurrentDirectory() + "\\samples\\CurrencyList.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(@path);

            string code = "";
            string statusMessage = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["CurrencyQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["CurrencyQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {
                var res = doc["QBXML"]["QBXMLMsgsRs"]["CurrencyQueryRs"];
                XmlNodeList data = null;

                data = res.SelectNodes("CurrencyRet");
                foreach (XmlNode node in data)
                {

                    Currency C = new Currency();
                    C.ListID = node["ListID"].InnerText;
                    C.TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                    C.TimeModified = DateTime.Parse("" + node["TimeModified"].InnerText);
                    C.EditSequence = "" + node["EditSequence"].InnerText;
                    C.Name = node["Name"].InnerText;
                    if (node["IsActive"] != null)
                        C.IsActive = Boolean.Parse("" + node["IsActive"].InnerText);

                    C.CurrencyCode = node["CurrencyCode"].InnerText;

                    if (node["CurrencyFormat"] != null)
                    {
                        if (node["CurrencyFormat"]["ThousandSeparator"] != null)
                            C.Format_ThousandSeparator = node["CurrencyFormat"]["ThousandSeparator"].InnerText;
                        if (node["CurrencyFormat"]["ThousandSeparatorGrouping"] != null)
                            C.Format_ThousandSeparatorGrouping = node["CurrencyFormat"]["ThousandSeparatorGrouping"].InnerText;
                        if (node["CurrencyFormat"]["DecimalPlaces"] != null)
                            C.Format_DecimalPlaces = node["CurrencyFormat"]["DecimalPlaces"].InnerText;
                        if (node["CurrencyFormat"]["DecimalSeparator"] != null)
                            C.Format_DecimalSeparator = node["CurrencyFormat"]["DecimalSeparator"].InnerText;
                    }

                    if (node["IsUserDefinedCurrency"] != null)
                        C.IsUserDefinedCurrency = Boolean.Parse("" + node["IsUserDefinedCurrency"].InnerText);

                    if (node["ExchangeRate"] != null)
                        C.ExchangeRate = Functions.ParseFloat("" + node["ExchangeRate"].InnerText);

                    if (node["AsOfDate"] != null)
                        C.AsOfDate = DateTime.Parse("" + node["AsOfDate"].InnerText);

                    list.Add(C);

                    
                }

            }
            return list;
        }


    }
}
