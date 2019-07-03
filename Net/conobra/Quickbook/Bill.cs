using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.Globalization;
using System.IO;


namespace Quickbook
{
    public class Bill : Abstract
    {
        public Vendor VendorRef { get; set; }
        public VendorAddress VendorAddress { get; set; }
        public Account APAccountRef { get; set; }
        public DateTime? txnDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string RefNumber { get; set; }
        public TermsRef TermsRef { get; set; }
        public string Memo { get; set; }
        public bool? IsTaxIncluded { get; set; }
        public SalesTaxCode SalesTaxCodeRef { get; set; }
        public Double? ExchangeRate { get; set; }
        public string ExternalGUID { get; set; }
        public string LinkToTxnID { get; set; }


        private bool exists;

        public string TxnID { get; set; }
        public string EditSequence { get; set; }
        public string lastResponse;

        private List<BillExpenseLine> expenseLines;

        public List<BillItemLine> itemLines;




        public Bill()
        {
            VendorRef = null;
            VendorAddress = null;
            APAccountRef = null;
            RefNumber = string.Empty;
            TermsRef = null;
            Memo = string.Empty;
            IsTaxIncluded = null;
            SalesTaxCodeRef = null;
            ExternalGUID = string.Empty;

            txnDate = null;
            DueDate = null;
            ExchangeRate = null;
            expenseLines = new List<BillExpenseLine>();
            itemLines = new List<BillItemLine>();

            exists = false;
            TxnID = string.Empty;
            EditSequence = string.Empty;
            HasChild = true;
        }

        public void clearExpenseLine()
        {
            expenseLines.Clear();
        }

        public void clearItemLines()
        {
            itemLines.Clear();
        }

        public int countExpenseLines()
        {
            return expenseLines.Count;
        }
        public int countItemLines()
        {
            return itemLines.Count;
        }

        public void addExpenseLine(Abstract line)
        {
            expenseLines.Add((BillExpenseLine)line);
        }

        public void addItemLine(BillItemLine line)
        {
            itemLines.Add(line);
        }


        public void updateItemLine(string itemName, BillItemLine line)
        {
            foreach (BillItemLine l in itemLines)
            {
                if (l.ItemName == itemName)
                {
                    l.copy(line);
                    break;
                }
            }
        }

        public void updateExpenseLine(string account, BillExpenseLine line)
        {
            foreach (BillExpenseLine l in expenseLines)
            {
                if (l.AccountRef.ListID == account)
                {
                    l.copy(line);
                    break;
                }
            }
        }



        public DateTime? TxnDate
        {
            get { return txnDate; }
            set { txnDate = value; }
        }


        public XmlDocument create()
        {
            try
            {
                //if (qb.Connect())
                //{

                //    string msg = qb.sendRequest(toXml());
                //    lastResponse = msg;
                //    XmlDocument doc = new XmlDocument();
                //    doc.LoadXml(msg);
                //    return doc;
                //}
            }
            catch (Exception ex)
            {
                //lastResponse = ex.Message;
            }
            return null;
        }

        public string toXmlLoadBill(string refNumber)
        {
            string xml = string.Empty;
            xml += "<?xml version=\"1.0\" ?>" + Environment.NewLine;
            xml += "<?qbxml version=\"13.0\"?>" + Environment.NewLine;
            xml += "<QBXML>" + Environment.NewLine;
            xml += "<QBXMLMsgsRq onError=\"stopOnError\">" + Environment.NewLine;
            xml += "<BillQueryRq>" + Environment.NewLine;
            xml += "<RefNumber >" + refNumber + "</RefNumber>" + Environment.NewLine;
            xml += "<IncludeLineItems >true</IncludeLineItems>" + Environment.NewLine;
            xml += "<OwnerID>0</OwnerID>" + Environment.NewLine;
            xml += "</BillQueryRq>" + Environment.NewLine;
            xml += "</QBXMLMsgsRq>" + Environment.NewLine;
            xml += "</QBXML>";

            return xml;
        }

        public string toXmlClearExpenseLines()
        {
            string xml = string.Empty;

            xml += "<?xml version=\"1.0\" ?>";
            xml += Environment.NewLine + Environment.NewLine + "<?qbxml version=\"13.0\" ?>";
            xml += Environment.NewLine + "<QBXML>";
            xml += Environment.NewLine + "<QBXMLMsgsRq onError=\"stopOnError\">";
            xml += Environment.NewLine + "<BillModRq requestID=\"10002\">";
            xml += Environment.NewLine + "<BillMod>";

            xml += Environment.NewLine + "<TxnID>" + TxnID + "</TxnID>";
            xml += Environment.NewLine + "<EditSequence>" + EditSequence + "</EditSequence>";

            xml += Environment.NewLine + "<ClearExpenseLines>true</ClearExpenseLines>";
            //xml += Environment.NewLine + "<ClearItemLines>true</ClearItemLines>";

            xml += Environment.NewLine + "</BillMod>";
            xml += Environment.NewLine + "</BillModRq>";
            xml += Environment.NewLine + "</QBXMLMsgsRq>";
            xml += Environment.NewLine + "</QBXML>";


            return xml;
        }

        public bool LoadBill(string xml)
        {
            XmlDocument doc = new XmlDocument();
            try
            {

                doc.LoadXml(xml);
            }
            catch (Exception ex)
            {
                return false;
            }

            var statusCode = doc["QBXML"]["QBXMLMsgsRs"]["BillQueryRs"].Attributes["statusCode"].Value;

            if (statusCode == "0")
            {
                XmlNodeList rets = doc.SelectNodes("/QBXML/QBXMLMsgsRs/BillQueryRs/BillRet");
                string message = string.Empty;

                if (rets.Count > 0)
                {
                    foreach (XmlNode node in rets)
                    {

                        TxnID = node["TxnID"].InnerText;
                        EditSequence = node["EditSequence"].InnerText;

                        Hashtable aux = new Hashtable();

                        XmlNodeList extras = node.SelectNodes("ExpenseLineRet");
                        foreach (XmlNode ex in extras)
                        {
                            string txn = ex["TxnLineID"].InnerText;
                            string lid = ex["AccountRef"]["ListID"].InnerText;

                            aux[lid] = txn;
                        }

                        foreach (BillExpenseLine ll in expenseLines)
                        {
                            if (aux.ContainsKey(ll.AccountRef.ListID))
                            {
                                ll.TxnLineID = aux[ll.AccountRef.ListID] + "";
                            }
                            else
                            {
                                ll.TxnLineID = "-1";
                            }
                        }

                        // Lines..

                        Hashtable aux2 = new Hashtable();

                        XmlNodeList extras2 = node.SelectNodes("ItemLineRet");
                        foreach (XmlNode ex2 in extras2)
                        {
                            string txn2 = ex2["TxnLineID"].InnerText;
                            string lid2 = ex2["ItemRef"]["FullName"].InnerText.ToUpper();

                            aux2[lid2] = txn2;
                        }

                        foreach (BillItemLine ll2 in itemLines)
                        {
                            if (aux2.ContainsKey(ll2.ItemName.ToUpper()))
                            {
                                ll2.TxnLineID = aux2[ll2.ItemName.ToUpper()] + "";
                            }
                        }

                        break;

                    }
                    exists = true;
                    return true;
                }
            }

            return false;
        }


        public string toXmlMod()
        {
            string xml = string.Empty;

            xml += "<?xml version=\"1.0\" ?>";
            xml += Environment.NewLine + Environment.NewLine + "<?qbxml version=\"13.0\" ?>";
            xml += Environment.NewLine + "<QBXML>";
            xml += Environment.NewLine + "<QBXMLMsgsRq onError=\"stopOnError\">";
            xml += Environment.NewLine + "<BillModRq requestID=\"10002\">";
            xml += Environment.NewLine + "<BillMod>";

            xml += Environment.NewLine + "<TxnID>" + TxnID + "</TxnID>";
            xml += Environment.NewLine + "<EditSequence>" + EditSequence + "</EditSequence>";

            if (VendorRef != null)
            {
                xml += VendorRef.toXMLVendorRef();
            }

            if (APAccountRef != null)
            {
                xml += APAccountRef.toXmlRef();
            }

            if (txnDate != null) {
                string DateString = txnDate.ToString();
                DateTime dt = Convert.ToDateTime(DateString);                
                xml += Environment.NewLine + "<TxnDate>" + dt.ToString("yyyy-MM-dd") + "</TxnDate>";
            }


            if (DueDate != null) {
                string DateString = DueDate.ToString();
                DateTime dt = Convert.ToDateTime(DateString);
                xml += Environment.NewLine + "<DueDate>" + dt.ToString("yyyy-MM-dd") + "</DueDate>";
            }
                

            xml += Environment.NewLine + "<RefNumber>" + RefNumber + "</RefNumber>";
            if (Memo != string.Empty)
            {
                xml += Environment.NewLine + "<Memo>" + Memo + "</Memo>";
            }

            if (ExchangeRate != null) {
                System.Globalization.CultureInfo myInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
                string val =ExchangeRate.ToString();
                Double value = Double.Parse(val, myInfo);
                xml += Environment.NewLine + "<ExchangeRate>" + value + "</ExchangeRate>";
            }
            

            foreach (BillExpenseLine line in expenseLines)
            {
                xml += Environment.NewLine + line.toXmlMod();
            }

            foreach (BillItemLine line in itemLines)
            {
                xml += Environment.NewLine + line.toXmlMod();
            }

            xml += Environment.NewLine + "</BillMod>";
            xml += Environment.NewLine + "</BillModRq>";
            xml += Environment.NewLine + "</QBXMLMsgsRq>";
            xml += Environment.NewLine + "</QBXML>";


            return xml;
        }

        public string toXml()
        {
            string xml = string.Empty;

            xml += "<?xml version=\"1.0\" ?>";
            xml += Environment.NewLine + Environment.NewLine + "<?qbxml version=\"13.0\" ?>";
            xml += Environment.NewLine + "<QBXML>";
            xml += Environment.NewLine + "<QBXMLMsgsRq onError=\"stopOnError\">";
            xml += Environment.NewLine + "<BillAddRq>";
            xml += Environment.NewLine + "<BillAdd>";
            if (VendorRef != null)
            {
                xml += VendorRef.toXMLVendorRef();
            }


            if (VendorAddress != null)
            {

                xml += VendorAddress.toXmlRef();
            }

            if (APAccountRef != null)
            {
                xml += APAccountRef.toXmlRef();
            }
            if (txnDate != null)
            {
                string DateString = txnDate.ToString();
                DateTime dt = Convert.ToDateTime(DateString);
                xml += Environment.NewLine + "<TxnDate>" + dt.ToString("yyyy-MM-dd") + "</TxnDate>";
            }


            if (DueDate != null)
            {
                string DateString = DueDate.ToString();
                DateTime dt = Convert.ToDateTime(DateString);
                xml += Environment.NewLine + "<DueDate>" + dt.ToString("yyyy-MM-dd") + "</DueDate>";
            }

         

            xml += Environment.NewLine + "<RefNumber>" + RefNumber + "</RefNumber>";


            if (TermsRef != null)
            {
                xml += TermsRef.toXmlRef();
            }
            if (Memo != string.Empty)
            {
                xml += Environment.NewLine + "<Memo>" + Memo + "</Memo>";
            }

            if (IsTaxIncluded != null)
            {
                xml += " <IsTaxIncluded >" + IsTaxIncluded.ToString() + "</IsTaxIncluded>";
            }

            if (SalesTaxCodeRef != null)
            {
                xml += SalesTaxCodeRef.toXmlRef();
            }


            if (ExchangeRate != null)
            {
                System.Globalization.CultureInfo myInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
                string val = ExchangeRate.ToString();
                double value = Double.Parse(val, myInfo);                
                xml += Environment.NewLine + "<ExchangeRate>" + value.ToString("0.00",myInfo) + "</ExchangeRate>";
            }
            if (ExternalGUID != string.Empty)
            {
                xml += " <ExternalGUID >" + ExternalGUID + "</ExternalGUID>";
            }
            foreach (BillExpenseLine line in expenseLines)
            {
                xml += Environment.NewLine + line.toXml();
            }

            foreach (BillItemLine line in itemLines)
            {
                xml += Environment.NewLine + line.toXml();
            }

            xml += Environment.NewLine + "</BillAdd>";
            xml += Environment.NewLine + "</BillAddRq>";
            xml += Environment.NewLine + "</QBXMLMsgsRq>";
            xml += Environment.NewLine + "</QBXML>";


            return xml;
        }


        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {

            try
            {
                string xml = toXml();
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
                            string pathFile = Directory.GetCurrentDirectory() + "\\samples\\B_" + DateTime.Now.Ticks + ".xml";
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
                    string pathFile = Directory.GetCurrentDirectory() + "\\samples\\Bill_" + DateTime.Now.Ticks + ".xml";
                    File.WriteAllText(pathFile, xml);
                }

                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["BillAddRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["BillAddRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {

                    //string editSequence = "";
                    ListID = res["QBXML"]["QBXMLMsgsRs"]["BillAddRs"]["BillRet"]["TxnID"].InnerText;
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
             
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Obtener Bill registros de Quickbooks: " + ex.Message);
            }


            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Bill";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Bill";
            return new List<Abstract>();
        }
    }
}
