using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;


namespace Quickbook
{
    public class VendorCredit
    {
        private bool exists;

        public string TxnID ;
        public string EditSequence ;

        private Vendor vendor;
        private Account apaAccount;
        private DateTime txnDate;
        private DateTime dueDate;
        private string refNumber;
        private string memo;
        private float rate;

        public string lastResponse;

        private List<BillExpenseLine> expenseLines ;

        public List<BillItemLine> itemLines;

        private Connector qb;
        
        /*
         <TermsRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</TermsRef>
         <ExternalGUID >GUIDTYPE</ExternalGUID> <!-- optional -->
         */

        public VendorCredit(Connector instance)
        {
            vendor = null;
            apaAccount = null;
            expenseLines = new List<BillExpenseLine>();
            itemLines = new List<BillItemLine>();
            qb = instance;

            exists = false;

            TxnID = string.Empty;
            EditSequence = string.Empty;
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

        public void addExpenseLine( BillExpenseLine line )
        {
            expenseLines.Add(line);
        }

        public void addItemLine( BillItemLine line )
        {
            itemLines.Add(line);
        }


        public void updateItemLine( string itemName, BillItemLine line )
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

        public void updateExpenseLine( string account, BillExpenseLine line )
        {
            foreach( BillExpenseLine l in expenseLines ) {
                if (l.account.ListID == account)
                {
                    l.copy(line);
                    break;
                }
            }
        }

        public Vendor VendorRef
        {
            get { return vendor; }
            set { vendor = value; }
        }

        public Account APAccountRef
        {
            get { return apaAccount; }
            set { apaAccount = value; }
        }

        public DateTime TxnDate
        {
            get { return txnDate; }
            set { txnDate = value; }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        public string RefNumber
        {
            get { return refNumber; }
            set { refNumber = value; }
        }

        public string Memo
        {
            get { return memo; }
            set { memo = value; }
        }

        public float ExchangeRate
        {
            get { return rate; }
            set { rate = value; }
        }

     
        public XmlDocument create()
        {
            try
            {
                if (qb.Connect())
                {

                    string msg = qb.sendRequest(toXml());
                    lastResponse = msg;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(msg);
                    return doc;
                }
            } catch( Exception ex ) {
                //lastResponse = ex.Message;
            }
            return null;
        }

        public string toXmlLoadBill( string refNumber )
        {
            string xml = string.Empty;
            xml += "<?xml version=\"1.0\" ?>" + Environment.NewLine ;
            xml += "<?qbxml version=\"13.0\"?>" + Environment.NewLine ;
            xml += "<QBXML>" + Environment.NewLine ;
                xml += "<QBXMLMsgsRq onError=\"stopOnError\">" + Environment.NewLine;
                    xml += "<VendorCreditQueryRq>" + Environment.NewLine;
                        xml += "<RefNumber >" + refNumber + "</RefNumber>" + Environment.NewLine ;
                        xml += "<IncludeLineItems >true</IncludeLineItems>" + Environment.NewLine ; 
                        xml += "<OwnerID>0</OwnerID>" + Environment.NewLine ;
                    xml += "</VendorCreditQueryRq >" + Environment.NewLine;
                xml += "</QBXMLMsgsRq>" + Environment.NewLine ;
            xml += "</QBXML>" ;

            return xml;
        }

        public string toXmlClearExpenseLines()
        {
            string xml = string.Empty;

            xml += "<?xml version=\"1.0\" ?>";
            xml += Environment.NewLine + Environment.NewLine + "<?qbxml version=\"13.0\" ?>";
            xml += Environment.NewLine + "<QBXML>";
            xml += Environment.NewLine + "<QBXMLMsgsRq onError=\"stopOnError\">";
            xml += Environment.NewLine + "<VendorCreditModRq requestID=\"10002\">";
            xml += Environment.NewLine + "<VendorCreditMod>";

            xml += Environment.NewLine + "<TxnID>" + TxnID + "</TxnID>";
            xml += Environment.NewLine + "<EditSequence>" + EditSequence + "</EditSequence>";

            xml += Environment.NewLine + "<ClearExpenseLines>true</ClearExpenseLines>";
            //xml += Environment.NewLine + "<ClearItemLines>true</ClearItemLines>";

            xml += Environment.NewLine + "</VendorCreditMod>";
            xml += Environment.NewLine + "</VendorCreditModRq>";
            xml += Environment.NewLine + "</QBXMLMsgsRq>";
            xml += Environment.NewLine + "</QBXML>";


            return xml;
        }

        public bool LoadBill( string xml )
        {
            XmlDocument doc = new XmlDocument();
            try
            {

                doc.LoadXml(xml);
            }
            catch (Exception ex)
            {
                return false ;
            }

            var statusCode = doc["QBXML"]["QBXMLMsgsRs"]["VendorCreditQueryRs"].Attributes["statusCode"].Value;

            if (statusCode == "0")
            {
                XmlNodeList rets = doc.SelectNodes("/QBXML/QBXMLMsgsRs/VendorCreditQueryRs/VendorCreditRet");
                string message = string.Empty;

                if (rets.Count > 0)
                {
                    foreach (XmlNode node in rets)
                    {

                        TxnID = node["TxnID"].InnerText ;
                        EditSequence = node["EditSequence"].InnerText ;

                        Hashtable aux = new Hashtable();

                        XmlNodeList extras = node.SelectNodes("ExpenseLineRet");
                        foreach (XmlNode ex in extras)
                        {
                            string txn = ex["TxnLineID"].InnerText ;
                            string lid = ex["AccountRef"]["ListID"].InnerText ;

                            aux[lid] = txn;
                        }

                        foreach (BillExpenseLine ll in expenseLines)
                        {
                            if (aux.ContainsKey(ll.account.ListID))
                            {
                                ll.TxnLineID = aux[ll.account.ListID] + "";
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
            xml += Environment.NewLine + "<VendorCreditModRq requestID=\"10002\">";
            xml += Environment.NewLine + "<VendorCreditMod>";

            xml += Environment.NewLine + "<TxnID>" + TxnID + "</TxnID>";
            xml += Environment.NewLine + "<EditSequence>" + EditSequence + "</EditSequence>";

            if (vendor != null)
            {
                xml += Environment.NewLine + "<VendorRef>";
                xml += Environment.NewLine + "<ListID >" + vendor.ListID + "</ListID>";
                //xml += Environment.NewLine + "<FullName >STRTYPE</FullName>";
                xml += Environment.NewLine + "</VendorRef>";
            }

            if (apaAccount != null)
            {
                xml += Environment.NewLine + "<APAccountRef>";
                xml += Environment.NewLine + "<ListID >" + apaAccount.ListID + "</ListID>";
                //xml += Environment.NewLine + "<FullName >STRTYPE</FullName>";
                xml += Environment.NewLine + "</APAccountRef>";
            }

            xml += Environment.NewLine + "<TxnDate>" + txnDate.ToString("yyyy-MM-dd") + "</TxnDate>";
            

            xml += Environment.NewLine + "<RefNumber>" + refNumber + "</RefNumber>";
            if (memo != string.Empty)
            {
                xml += Environment.NewLine + "<Memo>" + memo + "</Memo>";
            }

            xml += Environment.NewLine + "<ExchangeRate>" + rate.ToString("0.00") + "</ExchangeRate>";

            foreach (BillExpenseLine line in expenseLines)
            {
                xml += Environment.NewLine + line.toXmlMod();
            }

            foreach (BillItemLine line in itemLines)
            {
                xml += Environment.NewLine + line.toXmlMod();
            }

            xml += Environment.NewLine + "</VendorCreditMod>";
            xml += Environment.NewLine + "</VendorCreditModRq>";
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
                    xml += Environment.NewLine + "<VendorCreditAddRq requestID=\"10001\">";
                        xml += Environment.NewLine + "<VendorCreditAdd >";

                        if ( vendor != null ) {
                            xml += Environment.NewLine + "<VendorRef>";
	                            xml += Environment.NewLine + "<ListID >" + vendor.ListID + "</ListID>";
	                            //xml += Environment.NewLine + "<FullName >STRTYPE</FullName>";
                            xml += Environment.NewLine + "</VendorRef>";
                        }

                        if ( apaAccount != null ) {
                            xml += Environment.NewLine + "<APAccountRef>";
		                        xml += Environment.NewLine + "<ListID >" + apaAccount.ListID + "</ListID>";
		                        //xml += Environment.NewLine + "<FullName >STRTYPE</FullName>";
	                        xml += Environment.NewLine + "</APAccountRef>";
                        }

                        xml += Environment.NewLine + "<TxnDate>" + txnDate.ToString("yyyy-MM-dd") + "</TxnDate>" ;
                        

	                    xml += Environment.NewLine + "<RefNumber>" + refNumber + "</RefNumber>" ;    
                        if ( memo != string.Empty ) {
                            xml += Environment.NewLine + "<Memo>" + memo + "</Memo>" ;    
                        }

                        xml += Environment.NewLine + "<ExchangeRate>" + rate.ToString("0.00") + "</ExchangeRate>" ;

                        foreach (BillExpenseLine line in expenseLines)
                        {
                            xml += Environment.NewLine + line.toXml();
                        }

                        foreach (BillItemLine line in itemLines)
                        {
                            xml += Environment.NewLine + line.toXml();
                        }

                        xml += Environment.NewLine + "</VendorCreditAdd>";
                    xml += Environment.NewLine + "</VendorCreditAddRq>";
	            xml += Environment.NewLine + "</QBXMLMsgsRq>";
            xml += Environment.NewLine + "</QBXML>";


            return xml;
        }

    }
}
