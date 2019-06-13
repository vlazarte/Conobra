using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace Quickbook
{
    public class ItemGroup : ItemAbstract
    {

        public DateTime TimeCreated;
        public DateTime TimeModified;
        public string EditSequence = string.Empty;
        public string Name = string.Empty;
        public string BarCodeValue = string.Empty;
        public bool IsActive;
        public string ItemDesc = string.Empty;
        public UnitOfMeasureSet UnitOfMeasureSetRef;
        public bool IsPrintItemsInGroup;
        public string SpecialItemType = string.Empty;

        public List<ItemGroupLine> ItemList;

        public string ExternalGUID = string.Empty;



        public ItemGroup()
        {
            ItemList = new List<ItemGroupLine>();
        }

        public bool LoadByListID(string lid, ref string err)
        {
            string xml = "" +
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<?qbxml version=\"13.0\"?>" +
            "<QBXML>" +
            "<QBXMLMsgsRq onError=\"stopOnError\">" +
            "<ItemGroupQueryRq>" +
            "<ListID >" + lid + "</ListID>" +
            "<OwnerID>0</OwnerID>" +
            "</ItemGroupQueryRq>" +
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

                code = res["QBXML"]["QBXMLMsgsRs"]["ItemGroupQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["ItemGroupQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {
                    var node = res["QBXML"]["QBXMLMsgsRs"]["ItemGroupQueryRs"]["ItemGroupRet"];

                    ListID = "" + node["ListID"].InnerText;
                    TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                    TimeModified = DateTime.Parse("" + node["TimeModified"].InnerText);
                    EditSequence = "" + node["EditSequence"].InnerText;
                    Name = "" + node["Name"].InnerText;
                    if (node["BarCodeValue"] != null)
                        BarCodeValue = "" + node["BarCodeValue"].InnerText;
                    if (node["IsActive"] != null)
                        IsActive = ("" + node["IsActive"].InnerText == "true" ? true : false);

                    if (node["UnitOfMeasureSetRef"] != null)
                    {
                        UnitOfMeasureSetRef = new UnitOfMeasureSet();
                        UnitOfMeasureSetRef.ListID = "" + node["UnitOfMeasureSetRef"]["ListID"].InnerText;
                        UnitOfMeasureSetRef.FullName = "" + node["UnitOfMeasureSetRef"]["FullName"].InnerText;
                    }

                    XmlNodeList __lines = node.SelectNodes("ItemGroupLine");
                    ItemList = new List<ItemGroupLine>();
                    foreach (XmlNode L in __lines)
                    {
                        ItemGroupLine Line = new ItemGroupLine();
                        if (L["ItemRef"] != null)
                        {
                            Line.ItemRef = new Item();
                            Line.ItemRef.ListID = "" + L["ItemRef"]["ListID"].InnerText;
                            Line.ItemRef.FullName = "" + L["ItemRef"]["FullName"].InnerText;
                        }
                        if (L["Quantity"] != null)
                            Line.Quantity = Functions.ParseFloat("" + L["Quantity"].InnerText);
                        if (L["UnitOfMeasure"] != null)
                            Line.UnitOfMeasure = "" + L["UnitOfMeasure"].InnerText;

                        ItemList.Add(Line);
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
