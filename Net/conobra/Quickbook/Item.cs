using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;

namespace Quickbook
{
    public class Item : Abstract
    {
        public string FullName;


        public string toXmlRef()
        {
            return "<ItemRef>" +
            "<ListID>" + ListID + "</ListID>" +
            "</ItemRef>";
        }

        public string getDataExByListID(string listId, string dataExName, ref string err)
        {
            string res = "";


            XmlDocument doc = new XmlDocument();

            if (Config.IsProduction == true)
            {
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xml += "<?qbxml version=\"13.0\"?>";
                xml += "<QBXML>";
                xml += "<QBXMLMsgsRq onError=\"stopOnError\">";

                xml += "<ItemQueryRq> ";
                xml += "<ListID>" + listId + "</ListID> ";
                xml += "<OwnerID >0</OwnerID> ";
                xml += "</ItemQueryRq> ";

                xml += "</QBXMLMsgsRq>";
                xml += "</QBXML>";
                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {
                    string response = qbook.sendRequest(xml);

                    //string pathF = Directory.GetCurrentDirectory() + "\\samples\\item_" + listId + ".xml";
                    //File.WriteAllText(pathF, response);

                    //response = File.ReadAllText(response);

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
                //string path = Directory.GetCurrentDirectory() + "\\samples\\I_" + ref_number + ".xml";
                //doc.Load(@path);
            }

            string code = "";
            string statusMessage = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {

                //clearDataExList();

                XmlElement node = null;

                if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemServiceRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemServiceRet"];

                else if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemNonInventoryRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemNonInventoryRet"];

                else if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemOtherChargeRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemOtherChargeRet"];

                else if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemInventoryRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemInventoryRet"];

                else if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemInventoryAssemblyRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemInventoryAssemblyRet"];

                else if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemFixedAssetRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemFixedAssetRet"];

                else if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemSubtotalRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemSubtotalRet"];

                else if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemDiscountRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemDiscountRet"];

                else if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemPaymentRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemPaymentRet"];

                else if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemSalesTaxRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemSalesTaxRet"];

                else if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemSalesTaxGroupRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemSalesTaxGroupRet"];

                else if (doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemGroupRet"] != null)
                    node = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"]["ItemGroupRet"];

                if (node == null)
                    return "";

                XmlNodeList __extras = node.SelectNodes("DataExtRet");
                if (__extras == null)
                    return "";
                foreach (XmlNode ex in __extras)
                {
                    var name = ex["DataExtName"].InnerText;
                    var value = ex["DataExtValue"].InnerText;
                    if (name == dataExName)
                        return value;
                }

            }

            return res;
        }

        public static void loadType(ref Hashtable items)
        {
            //Cargamos solo los items que vienen en el Hashtable

            string path = Directory.GetCurrentDirectory() + "\\samples\\ItemList.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(@path);

            string code = "";
            string statusMessage = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {
                var res = doc["QBXML"]["QBXMLMsgsRs"]["ItemQueryRs"];
                XmlNodeList data = null;

                data = res.SelectNodes("ItemServiceRet");
                foreach (XmlNode N in data)
                {
                    var lid = N["ListID"].InnerText;
                    if (items.ContainsKey(lid))
                        items[lid] = "Servicio";
                }

                data = res.SelectNodes("ItemInventoryRet");
                foreach (XmlNode N in data)
                {
                    var lid = N["ListID"].InnerText;
                    if (items.ContainsKey(lid))
                        items[lid] = "Fisico";
                }

            }

        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet Item";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Item";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Item";
            return new List<Abstract>();
        }
    }
}
