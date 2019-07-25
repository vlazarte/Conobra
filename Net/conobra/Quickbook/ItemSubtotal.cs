using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using System.IO;

namespace Quickbook
{
    public class ItemSubtotal : ItemAbstract
    {

        public DateTime TimeCreated;
        public DateTime TimeModified;
        public string EditSequence = string.Empty;
        public string Name = string.Empty;
        public string BarCodeValue = string.Empty;
        public bool IsActive;
        public string ItemDesc = string.Empty;

        //FinanceCharge, ReimbursableExpenseGroup, ReimbursableExpenseSubtotal
        public string SpecialItemType = string.Empty;

        public string ExternalGUID = string.Empty;


        public ItemSubtotal()
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
                xml += "<ItemSubtotalQueryRq>";
                xml += "<ActiveStatus >All</ActiveStatus>";
                xml += "</ItemSubtotalQueryRq>";
                xml += "</QBXMLMsgsRq>";
                xml += "</QBXML>";
                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {
                    string response = qbook.sendRequest(xml);
                    doc.LoadXml(response);

                    if (Config.SaveLogXML == true)
                    {
                        string pathFile = Directory.GetCurrentDirectory() + "\\samples\\ItemSubtotal.xml";
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
                string pathFile = Directory.GetCurrentDirectory() + "\\samples\\ItemSubtotal.xml";
                doc.Load(@pathFile);
            }

            string code = "";
            string statusMessage = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["ItemSubtotalQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["ItemSubtotalQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {
                var data = doc["QBXML"]["QBXMLMsgsRs"]["ItemSubtotalQueryRs"];

                var nodeList = data.SelectNodes("ItemSubtotalRet");

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
            "<ItemSubtotalQueryRq>" +
            "<ListID >" + lid + "</ListID>" +
            "<OwnerID>0</OwnerID>" +
            "</ItemSubtotalQueryRq>" +
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

                code = res["QBXML"]["QBXMLMsgsRs"]["ItemSubtotalQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["ItemSubtotalQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {
                    var node = res["QBXML"]["QBXMLMsgsRs"]["ItemSubtotalQueryRs"]["ItemSubtotalRet"];

                    ListID = "" + node["ListID"].InnerText;
                    TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                    TimeModified = DateTime.Parse("" + node["TimeModified"].InnerText);
                    EditSequence = "" + node["EditSequence"].InnerText;
                    Name = "" + node["Name"].InnerText;
                    if (node["BarCodeValue"] != null)
                        BarCodeValue = "" + node["BarCodeValue"].InnerText;
                    if (node["IsActive"] != null)
                        IsActive = ("" + node["IsActive"].InnerText == "true" ? true : false);

                    if (node["SpecialItemType"] != null)
                        SpecialItemType = "" + node["SpecialItemType"].InnerText;

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

        public static List<ItemSubtotal> getList(ref string err)
        {
            string xml = "" +
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<?qbxml version=\"13.0\"?>" +
            "<QBXML>" +
            "<QBXMLMsgsRq onError=\"stopOnError\">" +
            "<ItemSubtotalQueryRq>" +
            "<OwnerID>0</OwnerID>" +
            "</ItemSubtotalQueryRq>" +
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

                code = res["QBXML"]["QBXMLMsgsRs"]["ItemSubtotalQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["ItemSubtotalQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {

                    var nodes = res["QBXML"]["QBXMLMsgsRs"]["ItemSubtotalQueryRs"];
                    XmlNodeList data = null;

                    List<ItemSubtotal> list = new List<ItemSubtotal>();

                    data = nodes.SelectNodes("ItemSubtotalRet");
                    foreach (XmlNode node in data)
                    {
                        ItemSubtotal I = new ItemSubtotal();

                        I.ListID = "" + node["ListID"].InnerText;
                        I.TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                        I.TimeModified = DateTime.Parse("" + node["TimeModified"].InnerText);
                        I.EditSequence = "" + node["EditSequence"].InnerText;
                        I.Name = "" + node["Name"].InnerText;
                        if (node["BarCodeValue"] != null)
                            I.BarCodeValue = "" + node["BarCodeValue"].InnerText;
                        if (node["IsActive"] != null)
                            I.IsActive = ("" + node["IsActive"].InnerText == "true" ? true : false);

                        if (node["SpecialItemType"] != null)
                            I.SpecialItemType = "" + node["SpecialItemType"].InnerText;

                        if (node["ExternalGUID"] != null)
                            I.ExternalGUID = "" + node["ExternalGUID"].InnerText;

                        XmlNodeList __extras = node.SelectNodes("DataExtRet");
                        foreach (XmlNode ex in __extras)
                        {
                            var name = ex["DataExtName"].InnerText;
                            var value = ex["DataExtValue"].InnerText;
                            I.AddDataEx(name, value);
                        }
                        list.Add(I);
                    }

                    return list;

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

            return new List<ItemSubtotal>();
        }

    }
}
