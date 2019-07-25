using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Quickbook
{
    public class ItemSalesTax : ItemAbstract
    {

        public DateTime TimeCreated;
        public DateTime TimeModified;
        public string EditSequence = string.Empty;
        public string Name = string.Empty;
        public string BarCodeValue = string.Empty;
        public bool IsActive;
        public Class ClassRef;
        public string ItemDesc = string.Empty;
        public float? TaxRate;
        public TaxVendor TaxVendorRef;

        public string ExternalGUID;



        public ItemSalesTax()
        {
        }

        public List<string> fetchAll(ref string err)
        {
            List<string> list = new List<string>();

            XmlDocument doc = new XmlDocument();

            if (Config.IsProduction == true)
            {
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xml += "<?qbxml version=\"13.0\"?>";
                xml += "<QBXML>";
                xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
                xml += "<ItemSalesTaxQueryRq>";
                xml += "<ActiveStatus >All</ActiveStatus>";
                xml += "</ItemSalesTaxQueryRq>";
                xml += "</QBXMLMsgsRq>";
                xml += "</QBXML>";
                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {

                    string response = qbook.sendRequest(xml);
                    doc.LoadXml(response);
                    if (Config.SaveLogXML == true)
                    {
                        string pathFile = Directory.GetCurrentDirectory() + "\\samples\\ItemTax.xml";
                        File.WriteAllText(pathFile, response);
                    }
                    qbook.Disconnect();
                }
                else
                {
                    err = "QuickBook no conecto";
                }
            }
            else
            {
                string pathFile = Directory.GetCurrentDirectory() + "\\samples\\ItemTax.xml";
                doc.Load(@pathFile);
            }

            string code = "";
            string statusMessage = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["ItemSalesTaxQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["ItemSalesTaxQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {
                var data = doc["QBXML"]["QBXMLMsgsRs"]["ItemSalesTaxQueryRs"];

                var nodeList = data.SelectNodes("ItemSalesTaxRet");

                foreach (XmlNode node in nodeList)
                {
                    list.Add("" + node["ListID"].InnerText);
                }

            }

            return list;
        }

        public bool LoadByListID(string lid, ref string err)
        {
            string xml = "" +
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<?qbxml version=\"13.0\"?>" +
            "<QBXML>" +
            "<QBXMLMsgsRq onError=\"stopOnError\">" +
            "<ItemSalesTaxQueryRq>" +
            "<ListID >" + lid + "</ListID>" +
            "<OwnerID>0</OwnerID>" +
            "</ItemSalesTaxQueryRq>" +
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

                code = res["QBXML"]["QBXMLMsgsRs"]["ItemSalesTaxQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["ItemSalesTaxQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {
                    var node = res["QBXML"]["QBXMLMsgsRs"]["ItemSalesTaxQueryRs"]["ItemSalesTaxRet"];

                    ListID = "" + node["ListID"].InnerText;
                    TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                    TimeModified = DateTime.Parse("" + node["TimeModified"].InnerText);
                    EditSequence = "" + node["EditSequence"].InnerText;
                    Name = "" + node["Name"].InnerText;
                    if (node["BarCodeValue"] != null)
                        BarCodeValue = "" + node["BarCodeValue"].InnerText;
                    if (node["IsActive"] != null)
                        IsActive = ("" + node["IsActive"].InnerText == "true" ? true : false);

                    if (node["ClassRef"] != null)
                    {
                        ClassRef = new Class();
                        ClassRef.ListID = "" + node["ClassRef"]["ListID"].InnerText;
                        ClassRef.FullName = "" + node["ClassRef"]["FullName"].InnerText;
                    }

                    if (node["ItemDesc"] != null)
                        ItemDesc = "" + node["ItemDesc"].InnerText;

                    if (node["TaxRate"] != null)
                        TaxRate = Functions.ParseFloat("" + node["TaxRate"].InnerText);

                    if (node["TaxVendorRef"] != null)
                    {
                        TaxVendorRef = new TaxVendor();
                        TaxVendorRef.ListID = "" + node["TaxVendorRef"]["ListID"].InnerText;
                        TaxVendorRef.FullName = "" + node["TaxVendorRef"]["FullName"].InnerText;
                    }

                    if (node["ExternalGUID"] != null)
                        ExternalGUID = "" + node["ExternalGUID"].InnerText;

                    XmlNodeList __extras = node.SelectNodes("DataExtRet");
                    foreach (XmlNode ex in __extras)
                    {
                        var name = ex["DataExtName"].InnerText;
                        var value = ex["DataExtValue"].InnerText;
                        AddDataEx(name, value);
                    }
                    return true;

                }
                else
                {
                    err = statusMessage;
                }
                qbook.Disconnect();

            }
            else
            {
                err = "QuickBook no conecto";
            }
            return false;
        }

        public string toXmlRef()
        {
            return "<ItemSalesTaxRef>" +
            "<ListID >" + ListID + "</ListID>" +
            "</ItemSalesTaxRef>";
        }
    }
}
