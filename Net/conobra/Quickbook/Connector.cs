using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interop.QBXMLRP2;
using System.Xml;

namespace Quickbook
{
    public class Connector
    {
        private RequestProcessor2 rp;
        private string ticket;
        private string appName;
        private string maxVersion;
        private string fileName;

        private bool localhost = false;


        public Connector(string app, string file = "")
        {
            fileName = file;
            appName = app;
            ticket = string.Empty;
        }

        public void setLocalhost( bool val )
        {
            localhost = val;
        }

        public bool getLocalhost()
        {
            return localhost;
        }

        public bool isOpen()
        {
            if (localhost == true)
                return true;
            return ticket != string.Empty;
        }

        public bool Connect()
        {
            if (Config.IsProduction == false)
                return true;

            if (localhost == true)
                return true;

            if (ticket != string.Empty)
                return true;
            try
            {
                rp = new RequestProcessor2();
                rp.OpenConnection("", appName);
                ticket = rp.BeginSession(fileName, QBFileMode.qbFileOpenDoNotCare);
                if (ticket != string.Empty)
                {
                    string[] versions = rp.get_QBXMLVersionsForSession(ticket);
                    maxVersion = versions[versions.Length - 1];
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectar a Quickbook: " + ex.Message);
            }
            return true;
        }

        public void Disconnect()
        {
            if (Config.IsProduction == false)
                return;

            if (localhost == true)
                return;
            try
            {
                rp.EndSession(ticket);
                ticket = null;
                rp.CloseConnection();
            }
            catch (Exception e)
            {
            }
            
        }

        private XmlElement buildXmlEnvelope(XmlDocument doc, string maxVer)
        {
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", null, null));
            doc.AppendChild(doc.CreateProcessingInstruction("qbxml", "version=\"" + maxVer + "\""));
            XmlElement qbXML = doc.CreateElement("QBXML");
            doc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = doc.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            return qbXMLMsgsRq;
        }

        public string sendRequest(string xml)
        {
            try
            {
                var response = rp.ProcessRequest(ticket, xml);
                return response;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
