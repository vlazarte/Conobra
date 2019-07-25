using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Quickbook
{
    public class Terms : Abstract
    {
        public static int TermsType_StandardTermsRet = 0;
        public static int TermsType_DateDrivenTermsRet = 1;
        public int TermsType = 0;

        public DateTime TimeCreated;
        public DateTime TimeModified;
        public string EditSequence;
        public string Name;
        public bool IsActive;
        public int StdDueDays;
        public int StdDiscountDays;
        public decimal DiscountPct;

        public string FullName{get;set;}


        public static List<Terms> getTerms(ref string err)
        {
            List<Terms> list = new List<Terms>();

            XmlDocument doc = new XmlDocument();

            if (Config.IsProduction == true)
            {
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xml += "<?qbxml version=\"13.0\"?>";
                xml += "<QBXML>";
                xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
                xml += "<TermsQueryRq>";
                xml += "</TermsQueryRq>";
                xml += "</QBXMLMsgsRq>";
                xml += "</QBXML>";
                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {
                    string response = qbook.sendRequest(xml);

                    if (Config.SaveLogXML == true)
                    {
                        string path = Directory.GetCurrentDirectory() + "\\samples\\TermsList.xml";
                        System.IO.File.WriteAllText(path, response);
                    }

                    doc.LoadXml(response);
                    qbook.Disconnect();
                }
                else
                {
                    err = "QuickBook no conecto";
                }
            }
            else
            {
                string path = Directory.GetCurrentDirectory() + "\\samples\\TermsList.xml";
                doc.Load(@path);
            }

            string code = "";
            string statusMessage = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["TermsQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["TermsQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {
                var data = doc["QBXML"]["QBXMLMsgsRs"]["TermsQueryRs"];

                var nodeList = data.SelectNodes("StandardTermsRet");

                foreach (XmlNode node in nodeList)
                {
                    Terms T = new Terms();
                    T.TermsType = TermsType_StandardTermsRet;
                    T.ListID = "" + node["ListID"].InnerText;
                    T.TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                    T.TimeModified = DateTime.Parse("" + node["TimeModified"].InnerText);
                    T.EditSequence = "" + node["EditSequence"].InnerText;
                    T.Name = "" + node["Name"].InnerText;
                    if (node["IsToBeEmailed"] != null)
                        T.IsActive = ("" + node["IsToBeEmailed"].InnerText) == "true" ? true : false;

                    if (node["StdDueDays"] != null)
                        T.StdDueDays = Int32.Parse("" + node["StdDueDays"].InnerText);
                    if (node["StdDiscountDays"] != null)
                        T.StdDiscountDays = Int32.Parse("" + node["StdDiscountDays"].InnerText);

                    //T.DiscountPct 

                    list.Add(T);

                }



            }


            return list;
        }

        public bool LoadByListID(string ListID, ref string err)
        {
            List<Terms> list = new List<Terms>();

            XmlDocument doc = new XmlDocument();

            if (Config.IsProduction == true)
            {
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xml += "<?qbxml version=\"13.0\"?>";
                xml += "<QBXML>";
                xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
                xml += "<TermsQueryRq>";
                xml += "<ListID >" + ListID + "</ListID>";
                xml += "</TermsQueryRq>";
                xml += "</QBXMLMsgsRq>";
                xml += "</QBXML>";
                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {
                    string response = qbook.sendRequest(xml);

                    if (Config.SaveLogXML == true)
                    {
                        string path = Directory.GetCurrentDirectory() + "\\samples\\T_" + ListID + ".xml";
                        System.IO.File.WriteAllText(path, response);
                    }

                    doc.LoadXml(response);
                    qbook.Disconnect();

                }
                else
                {
                    err = "QuickBook no conecto";
                }
            }
            else
            {
                string path = Directory.GetCurrentDirectory() + "\\samples\\T_" + ListID + ".xml";
                doc.Load(@path);
            }

            string code = "";
            string statusMessage = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["TermsQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["TermsQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {
                var data = doc["QBXML"]["QBXMLMsgsRs"]["TermsQueryRs"];

                var nodeList = data.SelectNodes("StandardTermsRet");

                foreach (XmlNode node in nodeList)
                {
                    TermsType = TermsType_StandardTermsRet;
                    ListID = "" + node["ListID"].InnerText;
                    TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                    TimeModified = DateTime.Parse("" + node["TimeModified"].InnerText);
                    EditSequence = "" + node["EditSequence"].InnerText;
                    Name = "" + node["Name"].InnerText;
                    if (node["IsToBeEmailed"] != null)
                        IsActive = ("" + node["IsToBeEmailed"].InnerText) == "true" ? true : false;

                    if (node["StdDueDays"] != null)
                        StdDueDays = Int32.Parse("" + node["StdDueDays"].InnerText);
                    if (node["StdDiscountDays"] != null)
                        StdDiscountDays = Int32.Parse("" + node["StdDiscountDays"].InnerText);

                    //T.DiscountPct 

                }

            }
            else
            {
                // err
                return false;
            }


            return true;
        }

        public string toXmlRef()
        {
            StringBuilder xml = new StringBuilder();
            
            xml.Append("<TermsRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {
                
                string value = Functions.htmlEntity(FullName);
                xml.Append("<FullName>" + value + "</FullName>");
            }

            xml.Append("</TermsRef>");

            return xml.ToString();
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet Terms";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Terms";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Terms";
            return new List<Abstract>();
        }
    }
}
