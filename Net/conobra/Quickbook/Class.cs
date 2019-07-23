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
        public string Name { get; set; }
        private bool? active;

        public string IsActive
        {
            get { return Convert.ToString(active); }
            set
            {
                if (value != null && value.ToUpper() == "TRUE")
                {
                    active = true;
                }
                else
                {
                    active = false;
                }

            }
        }
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

            

            StringBuilder toXML = new StringBuilder();

            toXML.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            toXML.Append("<?qbxml version=\"13.0\"?>");
            toXML.Append("<QBXML>");
            toXML.Append("<QBXMLMsgsRq onError=\"stopOnError\">");
            toXML.Append("<ClassAddRq>");
            toXML.Append("<ClassAdd>");
            if (FullName != string.Empty)
            {                
                string value = Functions.htmlEntity(FullName);
                toXML.Append("<Name>" + value + "</Name>"); //-- required -->
            }

            if (active != null)
            {
                toXML.Append("<IsActive>" + (active == true ? "true" : "false") + "</IsActive> ");//-- optional -->
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
            xml.Append("<ClassRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {
                string value = Functions.htmlEntity(FullName);
                xml.Append("<FullName>" + value + "</FullName>"); //-- required -->
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
                string value = Functions.htmlEntity(FullName);
                xml.Append("<FullName >" + value + "</FullName>");
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
            toXML.Append("<IncludeRetElement >FullName</IncludeRetElement>");
            toXML.Append("<IncludeRetElement >IsActive</IncludeRetElement>");
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
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
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
                        if (Config.SaveXML == true)
                        {
                            string pathFile = Directory.GetCurrentDirectory() + "\\samples\\C_" + DateTime.Now.Ticks + ".xml";
                            File.WriteAllText(pathFile, response);
                        }
                        qbook.Disconnect();

                    }
                    else
                    {
                        err = "QuickBook no conecto";
                    }
                }
                else {
                    //Datos de prueba
                    string pathFile = Directory.GetCurrentDirectory() + "\\samples\\NewClass_test.xml";
                    string response = File.ReadAllText(pathFile);

                    res.LoadXml(response);
                }

                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["ClassQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["ClassQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {

                    List<Abstract> quickbookListClass = new List<Abstract>();
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
                throw new Exception("Error  al Obtener Class registros CSV de Quickbooks: " + ex.Message);
            }

            return new List<Abstract>();
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
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

                    List<Abstract> quickbookListClass = new List<Abstract>();
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
                throw new Exception("Error al Obtener Class registros de Quickbooks: " + ex.Message);
            }

            return new List<Abstract>();
        }

        private List<Abstract> GetClasses(XmlNodeList rets)
        {


            List<Abstract> quickbookListClass = new List<Abstract>();
            
            string message = string.Empty;

            foreach (XmlNode node in rets)
            {
                Class toUpdate = new Class();
                if (node["ListID"] != null)
                {
                    toUpdate.ListID = node["ListID"].InnerText;
                }

                if (node["Name"] != null)
                {
                    toUpdate.Name = node["Name"].InnerText;
                }
               

                if (node["FullName"] != null)
                {
                    toUpdate.FullName = node["FullName"].InnerText;
                }

                if (node["IsActive"] != null)
                {
                    toUpdate.active = node["IsActive"].InnerText == "true" ? true : false;
                }               

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
                if (node["Sublevel"] != null)
                {
                    toUpdate.Sublevel =Convert.ToInt16( node["Sublevel"].InnerText); 
                }   

                quickbookListClass.Add(toUpdate);
            }

            return quickbookListClass;
            
        }
    }
}
