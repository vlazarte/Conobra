using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public abstract class Abstract
    {



        private List<DataEx> DataExList = new List<DataEx>();
        public string ListID { get; set; }
        public string ObjectName = "";
        public bool HasChild { get; set; }

        protected bool full_loaded = false;

        public void clearDataExList()
        {
            DataExList.Clear();
        }
        public Abstract()
        {
            ListID = string.Empty;
        }
        public void AddDataEx(string name, string value)
        {
            AddDataEx(name, value, "", "");
        }

        public void AddDataEx(string name, string value, string OwnerID, string TxnID)
        {
            if (name != "" && value != "")
            {
                DataEx DE = new DataEx();
                DE.DataExtName = name;
                DE.DataExtValue = value;
                DE.ListDataExtType = ObjectName;
                DE.ListObjRef_ListID = TxnID;
                DE.OwnerID = OwnerID;
                DataExList.Add(DE);
            }
        }

        public List<DataEx> getAll()
        {
            return DataExList;
        }

        public DataEx getDataEx(string name)
        {
            DataEx res = null;
            for (int i = 0; i < DataExList.Count; i++)
            {
                if (DataExList[i].DataExtName == name)
                    return DataExList[i];
            }
            return null;
        }

        public string getDataExValue(string name)
        {
            DataEx res = getDataEx(name);
            if (res != null)
                return res.DataExtValue;
            return null;
        }

        public bool InsertDataEx(string name, string value, ref string err)
        {
            bool result = false;

            try
            {

                string xml = "";

                xml += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xml += "<?qbxml version=\"13.0\"?>";
                xml += "<QBXML>";
                xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
                xml += "<DataExtAddRq>";
                xml += "<DataExtAdd>";
                xml += "<OwnerID >0</OwnerID>";
                xml += "<DataExtName >" + name + "</DataExtName>";
                xml += "<TxnDataExtType >" + ObjectName + "</TxnDataExtType>";
                xml += "<TxnID >" + ListID + "</TxnID>";
                xml += "<DataExtValue >" + value + "</DataExtValue>";
                xml += "</DataExtAdd>";
                xml += "</DataExtAddRq>";
                xml += "</QBXMLMsgsRq>";
                xml += "</QBXML>";

                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {
                    string response = qbook.sendRequest(xml);

                    XmlDocument res = new XmlDocument();
                    res.LoadXml(response);

                    string code = "";
                    string statusMessage = "";

                    code = res["QBXML"]["QBXMLMsgsRs"]["DataExtAddRs"].Attributes["statusCode"].Value;
                    statusMessage = res["QBXML"]["QBXMLMsgsRs"]["DataExtAddRs"].Attributes["statusMessage"].Value;

                    if (code == "0")
                    {
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
            catch (Exception ex)
            {
                err = ex.Message;
            }

            return result;
        }

        public bool UpdateDataEx(string name, string value, ref string err)
        {
            bool result = false;

            DataEx CF = getDataEx(name);

            if (CF == null)
            {
                // Crear
                result = InsertDataEx(name, value, ref err);
                if (!result)
                {
                    err = "No se pudo crear el DataEx " + name + ". " + err;
                }
                return result;

            }

            try
            {

                string xml = "";

                xml += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xml += "<?qbxml version=\"13.0\"?>";
                xml += "<QBXML>";
                xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
                xml += "<DataExtModRq>";
                xml += "<DataExtMod>";
                xml += "<OwnerID >" + CF.OwnerID + "</OwnerID>";
                xml += "<DataExtName >" + CF.DataExtName + "</DataExtName>";
                xml += "<TxnDataExtType >" + CF.ListDataExtType + "</TxnDataExtType>";
                xml += "<TxnID >" + CF.ListObjRef_ListID + "</TxnID>";
                xml += "<DataExtValue >" + value + "</DataExtValue>";
                xml += "</DataExtMod>";
                xml += "</DataExtModRq>";
                xml += "</QBXMLMsgsRq>";
                xml += "</QBXML>";

                 var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                 if (qbook.Connect())
                 {
                     string response = qbook.sendRequest(xml);

                     XmlDocument res = new XmlDocument();
                     res.LoadXml(response);

                     string code = "";
                     string statusMessage = "";

                     code = res["QBXML"]["QBXMLMsgsRs"]["DataExtModRs"].Attributes["statusCode"].Value;
                     statusMessage = res["QBXML"]["QBXMLMsgsRs"]["DataExtModRs"].Attributes["statusMessage"].Value;

                     if (code == "0")
                     {
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
            catch (Exception ex)
            {
                err = ex.Message;
            }

            return result;
        }

        public bool RegisterDataEx(ref string err)
        {


            if (ListID == string.Empty)
                throw new Exception("Es necesario el ID");

            if (DataExList.Count == 0)
            {
                return true;
            }
            string xml = toXmlAdd();

            var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

            if (qbook.Connect())
            {


                string response = qbook.sendRequest(xml);

                XmlDocument res = new XmlDocument();
                res.LoadXml(response);

                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["DataExtAddRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["DataExtAddRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {
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

        public string toXmlAdd()
        {
            string xml = "";
            xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<?qbxml version=\"13.0\"?>" +
                "<QBXML>" +
                    "<QBXMLMsgsRq onError=\"stopOnError\">";

            for (int i = 0; i < DataExList.Count; i++)
            {
                DataExList[i].ListObjRef_ListID = ListID;
                xml += DataExList[i].toXmlAdd();
            }

            xml += "</QBXMLMsgsRq>" +
            "</QBXML>";

            return xml;
        }
        public abstract bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived);
        public abstract List<Abstract> GetRecords(ref string err, bool includeSublevel);
        public abstract List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel);
        
    }
}
