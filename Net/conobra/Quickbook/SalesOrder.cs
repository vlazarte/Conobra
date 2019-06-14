using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;

namespace Quickbook
{
    public class SalesOrder : Abstract
    {
        public string TxnID;
        public List<string> CustomerRef;
        public string TxnDate;
        public List<string> BillAddress;
        public List<string> ShipAddress;
        public List<string> SalesRepRef;
        public string RefNumber = "";
        public string PONumber = "";
        public List<string> ItemSalesTaxRef;
        private List<SalesOrderLine> lines;

        private Hashtable dataEx;

        public SalesOrder()
        {
            dataEx = new Hashtable();
            lines = new List<SalesOrderLine>();
        }

        public Hashtable getDataEx()
        {
            return dataEx;
        }

        public void addDataEx(string name, string value)
        {
            dataEx.Add(name, value);
        }

        public void addLine(SalesOrderLine Line)
        {
            lines.Add(Line);
        }

        public string toXmlAdd()
        {
            string bill = "";
            string ship = "";

            string[] tags = { "Addr1", "Addr2", "Addr3", "Addr4", "Addr5" };

            int c = 0;
            for (int i = 0; i < BillAddress.Count; i++)
            {
                string v = BillAddress[i];
                if (v == "")
                    continue;

                bill += "<" + tags[c] + ">" + v + "</" + tags[c] + ">";
                c++;
            }

            c = 0;

            for (int i = 0; i < ShipAddress.Count; i++)
            {
                string v = ShipAddress[i];
                if (v == "")
                    continue;

                ship += "<" + tags[c] + ">" + v + "</" + tags[c] + ">";
                c++;
            }


            string xml = "" +
           "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<?qbxml version=\"13.0\"?>" +
            "<QBXML>" +
                "<QBXMLMsgsRq onError=\"stopOnError\">" +
                    "<SalesOrderAddRq requestID=\"U2FsZXNSZWNlaXB0QWRkfDE2NDY2\" >" +
                        "<SalesOrderAdd>" +
                            "<CustomerRef>" +
                                "<ListID>" + CustomerRef[0] + "</ListID>" +
                            "</CustomerRef>" +
                            "<TxnDate>" + TxnDate + "</TxnDate>" +
                            (RefNumber != "" ? "<RefNumber>" + RefNumber + "</RefNumber>" : "") +
                            "<BillAddress>" +
                                bill +
                            "</BillAddress>" +
                            "<ShipAddress>" +
                                ship +
                            "</ShipAddress>" +
                            "<PONumber >" + PONumber + "</PONumber>" +
                            "<SalesRepRef>" +
                                "<ListID>" + SalesRepRef[0] + "</ListID>" +
                            "</SalesRepRef>";

            foreach (SalesOrderLine Line in lines)
            {
                xml += Line.toXMLAdd() + Environment.NewLine;
            }

            xml += "</SalesOrderAdd>" +
                    "</SalesOrderAddRq>" +
                "</QBXMLMsgsRq>" +
            "</QBXML>";

            return xml;
        }

        public bool IsFullyInvoiced(string refNumber, ref string err)
        {
            string xml = "" +
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<?qbxml version=\"13.0\"?>" +
            "<QBXML>" +
            "<QBXMLMsgsRq onError=\"stopOnError\">" +
            "<SalesOrderQueryRq>" +
            "<RefNumber >" + refNumber + "</RefNumber>" +
            "<IncludeRetElement >IsManuallyClosed</IncludeRetElement>" +
            "<IncludeRetElement >IsFullyInvoiced</IncludeRetElement>" +
            "<OwnerID>0</OwnerID>" +
            "</SalesOrderQueryRq>" +
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

                code = res["QBXML"]["QBXMLMsgsRs"]["SalesOrderQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["SalesOrderQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {
                    var node = res["QBXML"]["QBXMLMsgsRs"]["SalesOrderQueryRs"]["SalesOrderRet"];

                    if (node["IsFullyInvoiced"].InnerText == "true")
                        return true;
                    if (node["IsManuallyClosed"].InnerText == "true")
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

        public bool LoadByRefNumber(string refNumber, ref string err)
        {
            string xml = "" +
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<?qbxml version=\"13.0\"?>" +
            "<QBXML>" +
            "<QBXMLMsgsRq onError=\"stopOnError\">" +
            "<SalesOrderQueryRq>" +
            "<RefNumber >" + refNumber + "</RefNumber>" +
            "<OwnerID>0</OwnerID>" +
            "</SalesOrderQueryRq>" +
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

                code = res["QBXML"]["QBXMLMsgsRs"]["SalesOrderQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["SalesOrderQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {
                    var node = res["QBXML"]["QBXMLMsgsRs"]["SalesOrderQueryRs"]["SalesOrderRet"];

                    TxnID = "" + node["TxnID"].InnerText;

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

        public bool AddRecord(out string err)
        {

            try
            {
                string xml = this.toXmlAdd();
                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {
                    string response = qbook.sendRequest(xml);

                    XmlDocument res = new XmlDocument();
                    res.LoadXml(response);

                    string code = "";
                    string statusMessage = "";

                    code = res["QBXML"]["QBXMLMsgsRs"]["SalesOrderAddRs"].Attributes["statusCode"].Value;
                    statusMessage = res["QBXML"]["QBXMLMsgsRs"]["SalesOrderAddRs"].Attributes["statusMessage"].Value;

                    if (code == "0")
                    {

                        //string editSequence = "";

                        RefNumber = res["QBXML"]["QBXMLMsgsRs"]["SalesOrderAddRs"]["SalesOrderRet"]["RefNumber"].InnerText;
                        TxnID = res["QBXML"]["QBXMLMsgsRs"]["SalesOrderAddRs"]["SalesOrderRet"]["TxnID"].InnerText;

                        //editSequence = res["QBXML"]["QBXMLMsgsRs"]["CustomerAddRs"]["CustomerRet"]["EditSequence"].InnerText;
                        err = null;
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
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }

            return false;
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet SalesOrder";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err)
        {
            err = "No implemented yet SalesOrder";
            return new List<Abstract>();
        }

    }
}
