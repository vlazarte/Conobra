using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Quickbook
{
    public class Class : Abstract
    {

        public string FullName { get; set; }
        public bool? IsActive { get; set; }
        public int Sublevel { get; set; }
        public Class ParentRef { get; set; }

        /*
<ClassRef>
<ListID >IDTYPE</ListID>
</ClassRef>
         */
        public Class()
        {
            FullName = string.Empty;
            IsActive = null;
            ParentRef = null;
        }
        public bool isValid()
        {
            return FullName != "";
        }
        public string toXmlAdd()
        {
            if (!isValid())
                return null;

            XmlElement ele = (new XmlDocument()).CreateElement("test");

            StringBuilder toXML = new StringBuilder();

            toXML.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            toXML.Append("<?qbxml version=\"13.0\"?>");
            toXML.Append("<QBXML>");
            toXML.Append("<QBXMLMsgsRq onError=\"stopOnError\">");
            toXML.Append("<ClassAddRq>");
            toXML.Append("<ClassAdd>");
            if (FullName != string.Empty)
            {
                ele.InnerText = FullName + "";
                toXML.Append("<Name>" + ele.InnerXml + "</Name>"); //-- required -->
            }

            if (IsActive != null)
            {
                toXML.Append("<IsActive>" + (IsActive == true ? "true" : "false") + "</IsActive> ");//-- optional -->
            }
            if (ParentRef != null)
            {
                toXML.Append(ParentRef.ToXMlParentRef());
            }
            toXML.Append("</ClassAdd>");
            // toXML.Append("         <IncludeRetElement >STRTYPE</IncludeRetElement> ");// <!-- optional, may repeat -->
            toXML.Append("</ClassAddRq>");
            toXML.Append("</QBXMLMsgsRq>");
            toXML.Append("</QBXML>");
            return toXML.ToString();
        }
        public string toXmlRef()
        {
            StringBuilder xml = new StringBuilder();
            XmlElement ele = (new XmlDocument()).CreateElement("test");
            xml.Append("<ClassRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {
                ele.InnerText = FullName + "";
                xml.Append("<FullName>" + ele.InnerXml + "</FullName>"); //-- required -->
            }

            xml.Append("</ClassRef>");

            return xml.ToString();
        }
        public string ToXMlParentRef()
        {
            StringBuilder xml = new StringBuilder();
            XmlElement ele = (new XmlDocument()).CreateElement("test");
            xml.Append("<ParentRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {
                ele.InnerText = FullName + "";
                xml.Append("<FullName >" + ele.InnerXml + "</FullName>");
            }
            xml.Append("</ParentRef>");
            return xml.ToString();
        }
        public string toXmlQuery(bool includeFilter)
        {

            XmlElement ele = (new XmlDocument()).CreateElement("test");

            StringBuilder toXML = new StringBuilder();

            toXML.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            toXML.Append("<?qbxml version=\"13.0\"?>");
            toXML.Append("<QBXML>");
            toXML.Append("<QBXMLMsgsRq onError=\"stopOnError\">");
            toXML.Append("<ClassQueryRq>");

            if (includeFilter)
            {



                toXML.Append(" <FromModifiedDate >" + DateTime.Now.ToString("yyy-MM-dd") + "T00:00:00-04:00</FromModifiedDate>");
                toXML.Append(" <ToModifiedDate >"+DateTime.Now.ToString("yyy-MM-dd")+"T23:59:59-04:00</ToModifiedDate> ");

            }
            toXML.Append("<IncludeRetElement >ListID</IncludeRetElement>");
            toXML.Append("<IncludeRetElement >Name</IncludeRetElement>");
            toXML.Append("<IncludeRetElement >ParentRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement >Sublevel</IncludeRetElement>");
            toXML.Append("</ClassQueryRq>");
            toXML.Append("</QBXMLMsgsRq>");
            toXML.Append("</QBXML>");

            return toXML.ToString();
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            try
            {
                string xml = toXmlAdd();
                if (xml == null)
                {
                    err = "Hubo un error al generar el XML";
                    return false;
                }

                XmlDocument res = new XmlDocument();

                if (Config.IsProduction == true)
                {
                    var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                    if (qbook.Connect())
                    {
                        string response = qbook.sendRequest(xml);
                        xmlSend = xml.Replace(",", ".");

                        res.LoadXml(response);

                        xmlRecived = res.InnerXml;
                        xmlRecived = xmlRecived.Replace(",", ".");
                        if (Config.SaveXML == true)
                        {
                            string pathFile = Directory.GetCurrentDirectory() + "\\samples\\C_" + ListID + ".xml";
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
                    string pathFile = Directory.GetCurrentDirectory() + "\\samples\\NewClass_" + DateTime.Now.Ticks + ".xml";
                    File.WriteAllText(pathFile, xml);
                }

                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["ClassAddRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["ClassAddRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {

                    //string editSequence = "";
                    ListID = res["QBXML"]["QBXMLMsgsRs"]["ClassAddRs"]["ClassRet"]["ListID"].InnerText;
                    //editSequence = res["QBXML"]["QBXMLMsgsRs"]["CustomerAddRs"]["CustomerRet"]["EditSequence"].InnerText;

                    // Crear DataExRet
                    if (RegisterDataEx(ref err))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    err = statusMessage;
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }


            return false;


        }
        public List<Class> GetRecordsCVS(ref string err)
        {

            try
            {
                string xml = toXmlQuery(false);
                if (xml == null)
                {
                    err = "Hubo un error al generar el XML";
                    return null;
                }

                XmlDocument res = new XmlDocument();

                if (Config.IsProduction == true)
                {
                    var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                    if (qbook.Connect())
                    {
                        string response = qbook.sendRequest(xml);
                        res.LoadXml(response);
                        qbook.Disconnect();

                    }
                    else
                    {
                        err = "QuickBook no conecto";
                    }
                }

                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["ClassQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["ClassQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {

                    List<Class> quickbookListClass = new List<Class>();
                    XmlNodeList rets = res.SelectNodes("/QBXML/QBXMLMsgsRs/ClassQueryRs/ClassRet");
                    quickbookListClass = GetClasses(rets);

                    if (quickbookListClass.Count > 0)
                    {
                        return quickbookListClass;
                    }
                    else
                    {
                        err = "No se obtuvo ningun registro";
                    }

                }
                else
                {
                    err = statusMessage;
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }

            return new List<Class>();
        }
        public List<Class> GetRecords(ref string err)
        {

            try
            {
                string xml = toXmlQuery(true);
                if (xml == null)
                {
                    err = "Hubo un error al generar el XML";
                    return null;
                }

                XmlDocument res = new XmlDocument();

                if (Config.IsProduction == true)
                {
                    var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                    if (qbook.Connect())
                    {
                        string response = qbook.sendRequest(xml);
                        res.LoadXml(response);
                        qbook.Disconnect();

                    }
                    else
                    {
                        err = "QuickBook no conecto";
                    }
                }

                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["ClassQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["ClassQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {

                    List<Class> quickbookListClass = new List<Class>();
                    XmlNodeList rets = res.SelectNodes("/QBXML/QBXMLMsgsRs/ClassQueryRs/ClassRet");
                    quickbookListClass=GetClasses(rets);

                    if (quickbookListClass.Count > 0)
                    {
                        return quickbookListClass;
                    }
                    else
                    {
                        err = "No se obtuvo ningun registro";
                    }

                }
                else
                {
                    err = statusMessage;
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }

            return new List<Class>();
        }

        private List<Class> GetClasses(XmlNodeList rets)
        {


            List<Class> quickbookListClass = new List<Class>();
            
            string message = string.Empty;

            foreach (XmlNode node in rets)
            {
                Class toUpdate = new Class();
                toUpdate.ListID = node["ListID"].InnerText;
                toUpdate.FullName = node["Name"].InnerText;

                if (node.SelectNodes("ParentRef").Count > 0)
                {
                    toUpdate.ParentRef = new Class();
                    if (node["ParentRef"]["ListID"] != null)
                    {
                        toUpdate.ParentRef.ListID = node["ParentRef"]["ListID"].InnerText;
                    }
                    if (node["ParentRef"]["FullName"] != null)
                    {
                        toUpdate.ParentRef.FullName = node["ParentRef"]["FullName"].InnerText;
                    }
                }

                quickbookListClass.Add(toUpdate);
            }

            return quickbookListClass;
            
        }
    }
}
