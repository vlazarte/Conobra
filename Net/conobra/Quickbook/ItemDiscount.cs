using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using System.IO;

namespace Quickbook
{
    public class ItemDiscount : ItemAbstract
    {
        public DateTime TimeCreated;
        public DateTime TimeModified;
        public string EditSequence = string.Empty;
        public string Name = string.Empty;
        public string FullName = string.Empty;
        public string BarCodeValue = string.Empty;
        public bool IsActive;
        public Class ClassRef;
        public ItemDiscount ParentRef;
        public int Sublevel;
        public string ItemDesc = string.Empty;

        public SalesTaxCode SalesTaxCodeRef;

        public float? DiscountRate;
        // or
        public float? DiscountRatePercent;

        public Account AccountRef;
        public string ExternalGUID = string.Empty;

        public ItemDiscount()
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
                xml += "<ItemDiscountQueryRq>";
                xml += "<ActiveStatus >All</ActiveStatus>";
                xml += "</ItemDiscountQueryRq>";
                xml += "</QBXMLMsgsRq>";
                xml += "</QBXML>";

                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {

                    string response = qbook.sendRequest(xml);
                    doc.LoadXml(response);

                    if (Config.SaveXML == true)
                    {
                        string pathFile = Directory.GetCurrentDirectory() + "\\samples\\ItemDiscount.xml";
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
                string pathFile = Directory.GetCurrentDirectory() + "\\samples\\ItemDiscount.xml";
                doc.Load(@pathFile);
            }

            string code = "";
            string statusMessage = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["ItemDiscountQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["ItemDiscountQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {
                var data = doc["QBXML"]["QBXMLMsgsRs"]["ItemDiscountQueryRs"];

                var nodeList = data.SelectNodes("ItemDiscountRet");

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
            "<ItemDiscountQueryRq>" +
            "<ListID >" + lid + "</ListID>" +
            "<OwnerID>0</OwnerID>" +
            "</ItemDiscountQueryRq>" +
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

                code = res["QBXML"]["QBXMLMsgsRs"]["ItemDiscountQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["ItemDiscountQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {
                    var node = res["QBXML"]["QBXMLMsgsRs"]["ItemDiscountQueryRs"]["ItemDiscountRet"];

                    ListID = "" + node["ListID"].InnerText;
                    TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                    TimeModified = DateTime.Parse("" + node["TimeModified"].InnerText);
                    EditSequence = "" + node["EditSequence"].InnerText;
                    Name = "" + node["Name"].InnerText;
                    FullName = "" + node["FullName"].InnerText;
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

                    Sublevel = Int32.Parse("" + node["Sublevel"].InnerText);
                    if (node["ItemDesc"] != null)
                        ItemDesc = "" + node["ItemDesc"].InnerText;

                    if (node["SalesTaxCodeRef"] != null)
                    {
                        SalesTaxCodeRef = new SalesTaxCode();
                        SalesTaxCodeRef.ListID = "" + node["SalesTaxCodeRef"]["ListID"].InnerText;
                        SalesTaxCodeRef.FullName = "" + node["SalesTaxCodeRef"]["FullName"].InnerText;
                    }

                    CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    ci.NumberFormat.CurrencyDecimalSeparator = ",";
                    if (node["DiscountRate"] != null)
                        DiscountRate = Functions.ParseFloat("" + node["DiscountRate"].InnerText);

                    if (node["DiscountRatePercent"] != null)
                        DiscountRatePercent = Functions.ParseFloat("" + node["DiscountRatePercent"].InnerText);

                    if (node["AccountRef"] != null)
                    {
                        AccountRef = new Account();
                        AccountRef.ListID = "" + node["AccountRef"]["ListID"].InnerText;
                        AccountRef.FullName = "" + node["AccountRef"]["FullName"].InnerText;
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

    }
}
