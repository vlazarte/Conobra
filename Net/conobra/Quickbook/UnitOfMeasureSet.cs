using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;

namespace Quickbook
{
    public class UnitOfMeasureSet : Abstract
    {

        public string FullName;
        public string UnitOfMeasureType;
        public BaseUnit BaseUnitRef;

        public Hashtable getBaseUnits(ref string err)
        {
            Hashtable results = new Hashtable();


            XmlDocument res = new XmlDocument();
            if (Config.IsProduction == true)
            {
                string xml = "" +
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<?qbxml version=\"13.0\"?>" +
                "<QBXML>" +
                "<QBXMLMsgsRq onError=\"stopOnError\">" +
                "<UnitOfMeasureSetQueryRq>" +
                "</UnitOfMeasureSetQueryRq>" +
                "</QBXMLMsgsRq>" +
                "</QBXML>";
                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {

                    string response = qbook.sendRequest(xml);
                    res.LoadXml(response);
                    if (Config.SaveXML == true)
                    {
                        string pathFile = Directory.GetCurrentDirectory() + "\\samples\\BaseUnits.xml";
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
                string pathFile = Directory.GetCurrentDirectory() + "\\samples\\BaseUnits.xml";
                res.Load(@pathFile);
            }


            string code = "";
            string statusMessage = "";

            code = res["QBXML"]["QBXMLMsgsRs"]["UnitOfMeasureSetQueryRs"].Attributes["statusCode"].Value;
            statusMessage = res["QBXML"]["QBXMLMsgsRs"]["UnitOfMeasureSetQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {
                var root = res["QBXML"]["QBXMLMsgsRs"]["UnitOfMeasureSetQueryRs"];

                XmlNodeList list = root.SelectNodes("UnitOfMeasureSetRet");

                foreach (XmlNode node in list)
                {

                    ListID = "" + node["ListID"].InnerText;
                    if (node["BaseUnit"] != null)
                    {
                        BaseUnitRef = new BaseUnit();
                        if (node["BaseUnit"]["Name"] != null)
                            BaseUnitRef.Name = "" + node["BaseUnit"]["Name"].InnerText;
                        if (node["BaseUnit"]["Abbreviation"] != null)
                            BaseUnitRef.Abbreviation = "" + node["BaseUnit"]["Abbreviation"].InnerText;

                        if (!results.ContainsKey(BaseUnitRef.Name))
                        {
                            results.Add(BaseUnitRef.Name, BaseUnitRef.Abbreviation);
                        }
                    }
                }

            }

            return results;
        }

        public bool LoadByName(string name, ref string err)
        {
            string xml = "" +
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<?qbxml version=\"13.0\"?>" +
            "<QBXML>" +
            "<QBXMLMsgsRq onError=\"stopOnError\">" +
            "<UnitOfMeasureSetQueryRq>" +
            "<FullName>" + name + "</FullName>" +
            "</UnitOfMeasureSetQueryRq>" +
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

                code = res["QBXML"]["QBXMLMsgsRs"]["UnitOfMeasureSetQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["UnitOfMeasureSetQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {
                    var node = res["QBXML"]["QBXMLMsgsRs"]["UnitOfMeasureSetQueryRs"]["UnitOfMeasureSetRet"];

                    ListID = "" + node["ListID"].InnerText;
                    if (node["BaseUnit"] != null)
                    {
                        BaseUnitRef = new BaseUnit();
                        if (node["BaseUnit"]["Name"] != null)
                            BaseUnitRef.Name = "" + node["BaseUnit"]["Name"].InnerText;
                        if (node["BaseUnit"]["Abbreviation"] != null)
                            BaseUnitRef.Abbreviation = "" + node["BaseUnit"]["Abbreviation"].InnerText;
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

        public bool LoadByListID(string lid, ref string err)
        {
            string xml = "" +
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<?qbxml version=\"13.0\"?>" +
            "<QBXML>" +
            "<QBXMLMsgsRq onError=\"stopOnError\">" +
            "<UnitOfMeasureSetQueryRq>" +
            "<ListID >" + lid + "</ListID>" +
            "</UnitOfMeasureSetQueryRq>" +
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

                 code = res["QBXML"]["QBXMLMsgsRs"]["UnitOfMeasureSetQueryRs"].Attributes["statusCode"].Value;
                 statusMessage = res["QBXML"]["QBXMLMsgsRs"]["UnitOfMeasureSetQueryRs"].Attributes["statusMessage"].Value;

                 if (code == "0")
                 {
                     var node = res["QBXML"]["QBXMLMsgsRs"]["UnitOfMeasureSetQueryRs"]["UnitOfMeasureSetRet"];

                     ListID = "" + node["ListID"].InnerText;
                     if (node["BaseUnit"] != null)
                     {
                         BaseUnitRef = new BaseUnit();
                         if (node["BaseUnit"]["Name"] != null)
                             BaseUnitRef.Name = "" + node["BaseUnit"]["Name"].InnerText;
                         if (node["BaseUnit"]["Abbreviation"] != null)
                             BaseUnitRef.Abbreviation = "" + node["BaseUnit"]["Abbreviation"].InnerText;
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
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet UnitOfMeasureSet";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet UnitOfMeasureSet";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet UnitOfMeasureSet";
            return new List<Abstract>();
        }
    }

    public class BaseUnit
    {
        public string Name;
        public string Abbreviation;
    }

}
