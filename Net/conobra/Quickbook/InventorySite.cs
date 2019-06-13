using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace Quickbook
{
    public class InventorySite : ItemAbstract
    {

        public DateTime TimeCreated;
        public DateTime TimeModified;
        public string EditSequence = string.Empty;
        public string Name = string.Empty;
        public bool IsActive;

        public InventorySite()
        {
        }

        public string toXmlRef()
        {
            return "<InventorySiteRef>" +
                "<ListID >" + ListID + "</ListID>" +
                "</InventorySiteRef>";
        }

        public List<InventorySite> getList(ref string err)
        {
            List<InventorySite> list = new List<InventorySite>();
            string xml = "" +
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<?qbxml version=\"13.0\"?>" +
            "<QBXML>" +
            "<QBXMLMsgsRq onError=\"stopOnError\">" +
            "<InventorySiteQueryRq>" +
            "<ActiveStatus>ActiveOnly</ActiveStatus>" +
            "</InventorySiteQueryRq>" +
            "</QBXMLMsgsRq>" +
            "</QBXML>";

            var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

            if (qbook.Connect())
            {

                string response = qbook.sendRequest(xml);

                XmlDocument res = new XmlDocument();
                res.LoadXml(response);

                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["InventorySiteQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["InventorySiteQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {
                    var root = res["QBXML"]["QBXMLMsgsRs"]["InventorySiteQueryRs"];

                    XmlNodeList __extras = root.SelectNodes("InventorySiteRet");

                    foreach (XmlNode node in __extras)
                    {
                        InventorySite IS = new InventorySite();
                        IS.ListID = "" + node["ListID"].InnerText;
                        IS.TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                        IS.TimeModified = DateTime.Parse("" + node["TimeModified"].InnerText);
                        IS.EditSequence = "" + node["EditSequence"].InnerText;
                        IS.Name = "" + node["Name"].InnerText;

                        if (node["IsActive"] != null)
                            IS.IsActive = ("" + node["IsActive"].InnerText == "true" ? true : false);

                        list.Add(IS);

                    }

                }
                else
                {
                    err = statusMessage;
                    return null;
                }
                qbook.Disconnect();
            }
            else
            {
                err = "QuickBook no conecto";
            }

           

            return list;
        }

    }
}
