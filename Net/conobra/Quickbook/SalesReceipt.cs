using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;

namespace Quickbook
{
    public class SalesReceipt : Abstract
    {



        public string TxnID = "";

        public DateTime TimeCreated;
        public DateTime TimeModified;
        public string EditSequence;
        public int? TxnNumber;
        public Customer CustomerRef; // required
        public Class ClassRef; // optional
        public Template TemplateRef;
        public DateTime? TxnDate;
        public string RefNumber = "";//
        public Address BillAddress;
        public Address BillAddressBlock;
        public Address ShipAddress;
        public Address ShipAddressBlock;
        public bool? IsPending;
        public string CheckNumber;
        public PaymentMethod PaymentMethodRef;
        public DateTime? DueDate;
        public SalesRep SalesRepRef;
        public DateTime? ShipDate;
        public ShipMethod ShipMethodRef;//--
        public string FOB;
        public decimal? Subtotal;
        public ItemSalesTax ItemSalesTaxRef;
        public decimal? SalesTaxPercentage;
        public decimal? SalesTaxTotal;//
        public decimal? TotalAmount;
        public Currency CurrencyRef;
        public decimal? ExchangeRate;
        public decimal? TotalAmountInHomeCurrency;
        public string Memo = "";
        public CustomerMsg CustomerMsgRef;

        public bool? IsToBePrinted;
        public bool? IsToBeEmailed;
        public CustomerSalesTaxCode CustomerSalesTaxCodeRef;

        public DepositToAccount DepositToAccountRef;


        /*
         <CreditCardTxnInfo> <!-- optional -->
<CreditCardTxnInputInfo> <!-- required -->
<CreditCardNumber >STRTYPE</CreditCardNumber> <!-- required -->
<ExpirationMonth >INTTYPE</ExpirationMonth> <!-- required -->
<ExpirationYear >INTTYPE</ExpirationYear> <!-- required -->
<NameOnCard >STRTYPE</NameOnCard> <!-- required -->
<CreditCardAddress >STRTYPE</CreditCardAddress> <!-- optional -->
<CreditCardPostalCode >STRTYPE</CreditCardPostalCode> <!-- optional -->
<CommercialCardCode >STRTYPE</CommercialCardCode> <!-- optional -->
<!-- TransactionMode may have one of the following values: CardNotPresent [DEFAULT], CardPresent -->
<TransactionMode >ENUMTYPE</TransactionMode> <!-- optional -->
<!-- CreditCardTxnType may have one of the following values: Authorization, Capture, Charge, Refund, VoiceAuthorization -->
<CreditCardTxnType >ENUMTYPE</CreditCardTxnType> <!-- optional -->
</CreditCardTxnInputInfo>
<CreditCardTxnResultInfo> <!-- required -->
<ResultCode >INTTYPE</ResultCode> <!-- required -->
<ResultMessage >STRTYPE</ResultMessage> <!-- required -->
<CreditCardTransID >STRTYPE</CreditCardTransID> <!-- required -->
<MerchantAccountNumber >STRTYPE</MerchantAccountNumber> <!-- required -->
<AuthorizationCode >STRTYPE</AuthorizationCode> <!-- optional -->
<!-- AVSStreet may have one of the following values: Pass, Fail, NotAvailable -->
<AVSStreet >ENUMTYPE</AVSStreet> <!-- optional -->
<!-- AVSZip may have one of the following values: Pass, Fail, NotAvailable -->
<AVSZip >ENUMTYPE</AVSZip> <!-- optional -->
<!-- CardSecurityCodeMatch may have one of the following values: Pass, Fail, NotAvailable -->
<CardSecurityCodeMatch >ENUMTYPE</CardSecurityCodeMatch> <!-- optional -->
<ReconBatchID >STRTYPE</ReconBatchID> <!-- optional -->
<PaymentGroupingCode >INTTYPE</PaymentGroupingCode> <!-- optional -->
<!-- PaymentStatus may have one of the following values: Unknown, Completed -->
<PaymentStatus >ENUMTYPE</PaymentStatus> <!-- required -->
<TxnAuthorizationTime >DATETIMETYPE</TxnAuthorizationTime> <!-- required -->
<TxnAuthorizationStamp >INTTYPE</TxnAuthorizationStamp> <!-- optional -->
<ClientTransID >STRTYPE</ClientTransID> <!-- optional -->
</CreditCardTxnResultInfo>
</CreditCardTxnInfo>
         */

        public string Other = "";//
        public string ExternalGUID = "";//



        public List<SalesReceiptLine> SalesReceiptLineRet;
        public List<SalesReceiptLineGroup> SalesReceiptLineGroupRet;

        public List<string> LinkedTransactions;

        public SalesReceipt()
        {

            LinkedTransactions = new List<string>();
            SalesReceiptLineRet = new List<SalesReceiptLine>();
            SalesReceiptLineGroupRet = new List<SalesReceiptLineGroup>();

            ObjectName = "SalesReceipt";
        }

        public bool isValid()
        {
            if (CustomerRef == null)
                return false;

            return true;
        }

        public static List<SalesReceipt> getInvoiceByDate(string from, string to, ref string err)
        {

            XmlDocument doc = new XmlDocument();
            List<SalesReceipt> list = new List<SalesReceipt>();

            if (Config.IsProduction == true)
            {
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xml += "<?qbxml version=\"13.0\"?>";
                xml += "<QBXML>";
                xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
                xml += "<SalesReceiptQueryRq>";

                /*xml += "<ModifiedDateRangeFilter>";
                //2017-08-17T00:00:00
                    xml += "<FromModifiedDate>" + from + "</FromModifiedDate>";
                    //2017-08-17T23:59:59
                    xml += "<ToModifiedDate>" + to + "</ToModifiedDate>";
                xml += "</ModifiedDateRangeFilter>";*/

                xml += "<TxnDateRangeFilter>";
                xml += "<FromTxnDate>" + from + "</FromTxnDate>";
                xml += "<ToTxnDate>" + to + "</ToTxnDate>";
                xml += "</TxnDateRangeFilter>";

                xml += "<IncludeRetElement >TxnID</IncludeRetElement>";
                xml += "<IncludeRetElement >TimeModified</IncludeRetElement>";
                xml += "<IncludeRetElement >TxnNumber</IncludeRetElement>";
                xml += "<IncludeRetElement >RefNumber</IncludeRetElement>";
                xml += "<OwnerID>0</OwnerID>";
                xml += "</SalesReceiptQueryRq>";
                xml += "</QBXMLMsgsRq>";
                xml += "</QBXML>";
                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {
                    string response = qbook.sendRequest(xml);
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
                string path = Directory.GetCurrentDirectory() + "\\samples\\InvoiceList.xml";
                doc.Load(@path);
            }

            string code = "";
            string statusMessage = "";
            string statusSeverity = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["SalesReceiptQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["SalesReceiptQueryRs"].Attributes["statusMessage"].Value;
            statusSeverity = doc["QBXML"]["QBXMLMsgsRs"]["SalesReceiptQueryRs"].Attributes["statusSeverity"].Value;

            if (code == "0")
            {
                var node = doc["QBXML"]["QBXMLMsgsRs"]["SalesReceiptQueryRs"];
                var nodes = node.SelectNodes("SalesReceiptRet");

                foreach (XmlNode N in nodes)
                {
                    SalesReceipt I = new SalesReceipt();
                    I.TimeModified = DateTime.Parse("" + N["TimeModified"].InnerText);
                    if (N["RefNumber"] != null)
                    {
                        I.RefNumber = "" + N["RefNumber"].InnerText;
                        I.TxnID = "" + N["TxnID"].InnerText;
                        I.TxnNumber = Int32.Parse("" + N["TxnNumber"].InnerText);
                        list.Add(I);
                    }


                }
            }
            else if (code == "1")
            {
                if (statusSeverity == "Info")
                {
                    list = new List<SalesReceipt>();
                    err = "";
                }
            }
            else
            {
                err = statusMessage;
                list = null;
            }

            return list;

        }

        public bool LoadByRefNumber(string ref_number, string txn_id, ref string err)
        {
            if (full_loaded == true)
                return true;

            XmlDocument doc = new XmlDocument();

            if (Config.IsProduction == true)
            {
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xml += "<?qbxml version=\"13.0\"?>";
                xml += "<QBXML>";
                xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
                xml += "<SalesReceiptQueryRq>";
                if (ref_number != "")
                    xml += "<RefNumber >" + ref_number + "</RefNumber >";
                else if (txn_id != "")
                    xml += "<TxnID >" + txn_id + "</TxnID >";
                xml += "<IncludeLineItems>true</IncludeLineItems>";
                xml += "<OwnerID>0</OwnerID>";
                xml += "</SalesReceiptQueryRq >";
                xml += "</QBXMLMsgsRq>";
                xml += "</QBXML>";

                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {

                    string response = qbook.sendRequest(xml);

                    if (Config.SaveLogXML == true)
                    {
                        string pathFile = Directory.GetCurrentDirectory() + "\\samples\\SR_" + ref_number + ".xml";
                        File.WriteAllText(pathFile, response);
                    }

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
                string pathFile = Directory.GetCurrentDirectory() + "\\samples\\SR_" + ref_number + ".xml";
                doc.Load(@pathFile);
            }

            string code = "";
            string statusMessage = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["SalesReceiptQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["SalesReceiptQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {

                clearDataExList();

                SalesReceiptLineRet.Clear();
                SalesReceiptLineGroupRet.Clear();
                LinkedTransactions.Clear();

                var root = doc["QBXML"]["QBXMLMsgsRs"]["SalesReceiptQueryRs"];

                var nodes = root.SelectNodes("SalesReceiptRet");

                if (nodes.Count > 1)
                {
                    err = "Existen " + nodes.Count + " SalesReceipt con el mismo Sales No.";
                    return false;
                }

                var node = root["SalesReceiptRet"];

                TxnID = "" + node["TxnID"].InnerText;
                ListID = TxnID;
                TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                TimeModified = DateTime.Parse("" + node["TimeModified"].InnerText);
                EditSequence = "" + node["EditSequence"].InnerText;
                TxnNumber = Int32.Parse("" + node["TxnNumber"].InnerText);

                if (node["CustomerRef"] != null)
                {
                    CustomerRef = new Customer();
                    CustomerRef.ListID = "" + node["CustomerRef"]["ListID"].InnerText;
                    CustomerRef.LoadByListID(CustomerRef.ListID, ref err);
                    /*string[] FullName = ("" + node["CustomerRef"]["FullName"].InnerText).Split(' ');
                    CustomerRef.FirstName = FullName[0];
                    CustomerRef.LastName = FullName[1];*/
                }

                /*<ClassRef> <!-- optional -->
                <ListID >IDTYPE</ListID> <!-- optional -->
                <FullName >STRTYPE</FullName> <!-- optional -->
                </ClassRef>*/
                if (node["TemplateRef"] != null)
                {
                    TemplateRef = new Template();
                    TemplateRef.ListID = "" + node["TemplateRef"]["ListID"].InnerText;
                    TemplateRef.FullName = "" + node["TemplateRef"]["FullName"].InnerText;

                }

                if (node["TxnDate"] != null)
                    TxnDate = DateTime.Parse("" + node["TxnDate"].InnerText);

                if (node["RefNumber"] != null)
                    RefNumber = "" + node["RefNumber"].InnerText;


                if (node["BillAddress"] != null)
                {
                    BillAddress = new Address(0);

                    for (int i = 1; i <= 5; i++)
                    {
                        if (node["BillAddress"]["Addr" + i] != null)
                        {
                            BillAddress.addAddrLine("" + node["BillAddress"]["Addr" + i].InnerText);
                        }

                    }
                    if (node["BillAddress"]["City"] != null)
                        BillAddress.City = "" + node["BillAddress"]["City"].InnerText;
                    if (node["BillAddress"]["State"] != null)
                        BillAddress.State = "" + node["BillAddress"]["State"].InnerText;
                    if (node["BillAddress"]["Note"] != null)
                        BillAddress.Note = "" + node["BillAddress"]["Note"].InnerText;

                }



                if (node["BillAddressBlock"] != null)
                {
                    BillAddressBlock = new Address(0);
                    for (int i = 1; i < 5; i++)
                    {
                        if (node["BillAddressBlock"]["Addr" + i] != null)
                        {
                            BillAddressBlock.addAddrLine("" + node["BillAddressBlock"]["Addr" + i].InnerText);
                        }

                    }


                }

                if (node["ShipAddress"] != null)
                {
                    ShipAddress = new Address(1);

                    for (int i = 1; i <= 5; i++)
                    {
                        if (node["ShipAddress"]["Addr" + i] != null)
                        {
                            ShipAddress.addAddrLine("" + node["ShipAddress"]["Addr" + i].InnerText);
                        }

                    }

                    if (node["ShipAddress"]["City"] != null)
                        ShipAddress.City = "" + node["ShipAddress"]["City"].InnerText;
                    if (node["ShipAddress"]["State"] != null)
                        ShipAddress.State = "" + node["ShipAddress"]["State"].InnerText;
                    if (node["ShipAddress"]["Note"] != null)
                        ShipAddress.Note = "" + node["ShipAddress"]["Note"].InnerText;
                }


                if (node["ShipAddressBlock"] != null)
                {
                    ShipAddressBlock = new Address(1);
                    for (int i = 1; i < 5; i++)
                    {
                        if (node["ShipAddressBlock"]["Addr" + i] != null)
                        {
                            ShipAddressBlock.addAddrLine("" + node["ShipAddressBlock"]["Addr" + i].InnerText);
                        }

                    }
                }


                if (node["IsPending"] != null)
                    IsPending = ("" + node["IsPending"].InnerText == "true" ? true : false);


                if (node["CheckNumber"] != null)
                    CheckNumber = node["CheckNumber"].InnerText + "";


                /*
                 
                 <PaymentMethodRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</PaymentMethodRef>
                 */



                if (node["DueDate"] != null)
                    DueDate = DateTime.Parse("" + node["DueDate"].InnerText);

                if (node["SalesRepRef"] != null)
                {
                    SalesRepRef = new SalesRep();
                    SalesRepRef.ListID = "" + node["SalesRepRef"]["ListID"].InnerText;
                    SalesRepRef.FullName = "" + node["SalesRepRef"]["FullName"].InnerText;
                }


                if (node["ShipDate"] != null)
                    ShipDate = DateTime.Parse("" + node["ShipDate"].InnerText);

                /*
                 <ShipMethodRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</ShipMethodRef>*/

                if (node["FOB"] != null)
                    FOB = node["FOB"].InnerText + "";

                if (node["Subtotal"] != null)
                    Subtotal = Functions.ParseDecimal("" + node["Subtotal"].InnerText);

                if (node["ItemSalesTaxRef"] != null)
                {
                    ItemSalesTaxRef = new ItemSalesTax();
                    ItemSalesTaxRef.ListID = "" + node["ItemSalesTaxRef"]["ListID"].InnerText;
                    ItemSalesTaxRef.Name = "" + node["ItemSalesTaxRef"]["FullName"].InnerText;
                }

                if (node["SalesTaxPercentage"] != null)
                    SalesTaxPercentage = Functions.ParseDecimal("" + node["SalesTaxPercentage"].InnerText);

                if (node["SalesTaxTotal"] != null)
                    SalesTaxTotal = Functions.ParseDecimal("" + node["SalesTaxTotal"].InnerText);

                if (node["TotalAmount"] != null)
                    TotalAmount = Functions.ParseDecimal("" + node["TotalAmount"].InnerText);


                if (node["CurrencyRef"] != null)
                {
                    CurrencyRef = new Currency();
                    CurrencyRef.ListID = "" + node["CurrencyRef"]["ListID"].InnerText;
                    CurrencyRef.FullName = "" + node["CurrencyRef"]["FullName"].InnerText;
                }


                if (node["ExchangeRate"] != null)
                    ExchangeRate = Functions.ParseDecimal("" + node["ExchangeRate"].InnerText);

                if (node["TotalAmountInHomeCurrency"] != null)
                    TotalAmountInHomeCurrency = Functions.ParseDecimal("" + node["TotalAmountInHomeCurrency"].InnerText);

                if (node["Memo"] != null)
                    Memo = "" + node["Memo"].InnerText;

                /*
                 
                 <CustomerMsgRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</CustomerMsgRef>
                 */

                if (node["IsToBePrinted"] != null)
                    IsToBePrinted = Boolean.Parse("" + node["IsToBePrinted"].InnerText);

                if (node["IsToBeEmailed"] != null)
                    IsToBeEmailed = Boolean.Parse("" + node["IsToBeEmailed"].InnerText);


                if (node["CustomerSalesTaxCodeRef"] != null)
                {
                    CustomerSalesTaxCodeRef = new CustomerSalesTaxCode();
                    CustomerSalesTaxCodeRef.ListID = "" + node["CustomerSalesTaxCodeRef"]["ListID"].InnerText;
                    CustomerSalesTaxCodeRef.FullName = "" + node["CustomerSalesTaxCodeRef"]["FullName"].InnerText;

                }

                /*
                 <DepositToAccountRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</DepositToAccountRef>
                 */

                /*
                 
                 <CreditCardTxnInfo> <!-- optional -->
<CreditCardTxnInputInfo> <!-- required -->
<CreditCardNumber >STRTYPE</CreditCardNumber> <!-- required -->
<ExpirationMonth >INTTYPE</ExpirationMonth> <!-- required -->
<ExpirationYear >INTTYPE</ExpirationYear> <!-- required -->
<NameOnCard >STRTYPE</NameOnCard> <!-- required -->
<CreditCardAddress >STRTYPE</CreditCardAddress> <!-- optional -->
<CreditCardPostalCode >STRTYPE</CreditCardPostalCode> <!-- optional -->
<CommercialCardCode >STRTYPE</CommercialCardCode> <!-- optional -->
<!-- TransactionMode may have one of the following values: CardNotPresent [DEFAULT], CardPresent -->
<TransactionMode >ENUMTYPE</TransactionMode> <!-- optional -->
<!-- CreditCardTxnType may have one of the following values: Authorization, Capture, Charge, Refund, VoiceAuthorization -->
<CreditCardTxnType >ENUMTYPE</CreditCardTxnType> <!-- optional -->
</CreditCardTxnInputInfo>
<CreditCardTxnResultInfo> <!-- required -->
<ResultCode >INTTYPE</ResultCode> <!-- required -->
<ResultMessage >STRTYPE</ResultMessage> <!-- required -->
<CreditCardTransID >STRTYPE</CreditCardTransID> <!-- required -->
<MerchantAccountNumber >STRTYPE</MerchantAccountNumber> <!-- required -->
<AuthorizationCode >STRTYPE</AuthorizationCode> <!-- optional -->
<!-- AVSStreet may have one of the following values: Pass, Fail, NotAvailable -->
<AVSStreet >ENUMTYPE</AVSStreet> <!-- optional -->
<!-- AVSZip may have one of the following values: Pass, Fail, NotAvailable -->
<AVSZip >ENUMTYPE</AVSZip> <!-- optional -->
<!-- CardSecurityCodeMatch may have one of the following values: Pass, Fail, NotAvailable -->
<CardSecurityCodeMatch >ENUMTYPE</CardSecurityCodeMatch> <!-- optional -->
<ReconBatchID >STRTYPE</ReconBatchID> <!-- optional -->
<PaymentGroupingCode >INTTYPE</PaymentGroupingCode> <!-- optional -->
<!-- PaymentStatus may have one of the following values: Unknown, Completed -->
<PaymentStatus >ENUMTYPE</PaymentStatus> <!-- required -->
<TxnAuthorizationTime >DATETIMETYPE</TxnAuthorizationTime> <!-- required -->
<TxnAuthorizationStamp >INTTYPE</TxnAuthorizationStamp> <!-- optional -->
<ClientTransID >STRTYPE</ClientTransID> <!-- optional -->
</CreditCardTxnResultInfo>
</CreditCardTxnInfo>
                 */



                XmlNodeList ListGroups = node.SelectNodes("SalesReceiptLineGroupRet");
                foreach (XmlNode Gex in ListGroups)
                {
                    SalesReceiptLineGroup LG = new SalesReceiptLineGroup();
                    LG.SalesReceiptLineRet = new List<SalesReceiptLine>();

                    XmlNodeList SListInvoice = Gex.SelectNodes("SalesReceiptLineRet");
                    foreach (XmlNode ex in SListInvoice)
                    {
                        SalesReceiptLine NewInvoice = new SalesReceiptLine();
                        NewInvoice.TxnLineID = "" + ex["TxnLineID"].InnerText;


                        if (ex["ItemRef"] != null)
                        {
                            NewInvoice.ItemRef = new Item();
                            NewInvoice.ItemRef.ListID = "" + ex["ItemRef"]["ListID"].InnerText;
                            NewInvoice.ItemRef.FullName = "" + ex["ItemRef"]["FullName"].InnerText;
                        }
                        if (ex["Desc"] != null)
                            NewInvoice.Desc = "" + ex["Desc"].InnerText;

                        if (ex["Quantity"] != null)
                            NewInvoice.Quantity = Functions.ParseDecimal("" + ex["Quantity"].InnerText);

                        if (ex["UnitOfMeasure"] != null)
                        {
                            NewInvoice.UnitOfMeasure = "" + ex["UnitOfMeasure"].InnerText;
                            //NewInvoice.UnitOfMeasureSetRef = new UnitOfMeasureSet();
                            //NewInvoice.UnitOfMeasureSetRef.LoadByName(NewInvoice.UnitOfMeasure, ref err);
                        }


                        /*if (ex["OverrideUOMSetRef"] != null)
                        {
                            NewInvoice.OverrideUOMSetRef = new OverrideUOMSet();
                            NewInvoice.OverrideUOMSetRef.ListID = "" + ex["OverrideUOMSetRef"]["ListID"].InnerText;
                            NewInvoice.OverrideUOMSetRef.FullName = "" + ex["OverrideUOMSetRef"]["FullName"].InnerText;
                        }*/


                        if (ex["Rate"] != null)
                        {
                            NewInvoice.Rate = Functions.ParseDecimal("" + ex["Rate"].InnerText);
                        }
                        else if (ex["RatePercent"] != null)
                        {
                            NewInvoice.RatePercent = Functions.ParseDecimal("" + ex["RatePercent"].InnerText);
                        }


                        if (ex["ClassRef"] != null)
                        {
                            NewInvoice.ClassRef = new Class();
                            NewInvoice.ClassRef.ListID = "" + ex["ClassRef"]["ListID"].InnerText;
                            NewInvoice.ClassRef.FullName = "" + ex["ClassRef"]["FullName"].InnerText;
                        }

                        if (ex["Amount"] != null)
                            NewInvoice.Amount = Functions.ParseDecimal("" + ex["Amount"].InnerText);

                        if (ex["InventorySiteRef"] != null)
                        {
                            NewInvoice.InventorySiteRef = new InventorySite();
                            NewInvoice.InventorySiteRef.ListID = "" + ex["InventorySiteRef"]["ListID"].InnerText;
                            NewInvoice.InventorySiteRef.Name = "" + ex["InventorySiteRef"]["FullName"].InnerText;
                        }

                        /*if (ex["InventorySiteLocationRef"] != null)
                        {
                            NewInvoice.InventorySiteLocationRef = new InventorySiteLocation();
                            NewInvoice.InventorySiteLocationRef.ListID = "" + ex["InventorySiteLocationRef"]["ListID"].InnerText;
                            NewInvoice.InventorySiteLocationRef.Name = "" + ex["InventorySiteLocationRef"]["FullName"].InnerText;
                        }*/


                        if (ex["SerialNumber"] != null)
                        {
                            NewInvoice.SerialNumber = "" + ex["SerialNumber"].InnerText;
                        }
                        else if (ex["LotNumber"] != null)
                        {
                            NewInvoice.LotNumber = "" + ex["LotNumber"].InnerText;
                        }

                        if (ex["ServiceDate"] != null)
                            NewInvoice.ServiceDate = DateTime.Parse("" + ex["ServiceDate"].InnerText);

                        if (ex["SalesTaxCodeRef"] != null)
                        {
                            NewInvoice.SalesTaxCodeRef = new SalesTaxCode();
                            NewInvoice.SalesTaxCodeRef.ListID = "" + ex["SalesTaxCodeRef"]["ListID"].InnerText;
                            NewInvoice.SalesTaxCodeRef.FullName = "" + ex["SalesTaxCodeRef"]["FullName"].InnerText;
                        }

                        if (ex["Other1"] != null)
                            NewInvoice.Other1 = "" + ex["Other1"].InnerText;

                        if (ex["Other2"] != null)
                            NewInvoice.Other2 = "" + ex["Other2"].InnerText;

                        LG.SalesReceiptLineRet.Add(NewInvoice);
                    }

                    SalesReceiptLineGroupRet.Add(LG);
                }


                XmlNodeList ListInvoice = node.SelectNodes("SalesReceiptLineRet");
                foreach (XmlNode ex in ListInvoice)
                {
                    SalesReceiptLine NewInvoice = new SalesReceiptLine();
                    NewInvoice.TxnLineID = "" + ex["TxnLineID"].InnerText;


                    if (ex["ItemRef"] != null)
                    {
                        NewInvoice.ItemRef = new Item();
                        NewInvoice.ItemRef.ListID = "" + ex["ItemRef"]["ListID"].InnerText;
                        NewInvoice.ItemRef.FullName = "" + ex["ItemRef"]["FullName"].InnerText;
                    }
                    if (ex["Desc"] != null)
                        NewInvoice.Desc = "" + ex["Desc"].InnerText;

                    if (ex["Quantity"] != null)
                        NewInvoice.Quantity = Functions.ParseDecimal("" + ex["Quantity"].InnerText);

                    if (ex["UnitOfMeasure"] != null)
                    {
                        NewInvoice.UnitOfMeasure = "" + ex["UnitOfMeasure"].InnerText;
                        //NewInvoice.UnitOfMeasureSetRef = new UnitOfMeasureSet();
                        //NewInvoice.UnitOfMeasureSetRef.LoadByName(NewInvoice.UnitOfMeasure, ref err);
                    }


                    /*if (ex["OverrideUOMSetRef"] != null)
                    {
                        NewInvoice.OverrideUOMSetRef = new OverrideUOMSet();
                        NewInvoice.OverrideUOMSetRef.ListID = "" + ex["OverrideUOMSetRef"]["ListID"].InnerText;
                        NewInvoice.OverrideUOMSetRef.FullName = "" + ex["OverrideUOMSetRef"]["FullName"].InnerText;
                    }*/


                    if (ex["Rate"] != null)
                    {
                        NewInvoice.Rate = Functions.ParseDecimal("" + ex["Rate"].InnerText);
                    }
                    else if (ex["RatePercent"] != null)
                    {
                        NewInvoice.RatePercent = Functions.ParseDecimal("" + ex["RatePercent"].InnerText);
                    }


                    if (ex["ClassRef"] != null)
                    {
                        NewInvoice.ClassRef = new Class();
                        NewInvoice.ClassRef.ListID = "" + ex["ClassRef"]["ListID"].InnerText;
                        NewInvoice.ClassRef.FullName = "" + ex["ClassRef"]["FullName"].InnerText;
                    }

                    if (ex["Amount"] != null)
                        NewInvoice.Amount = Functions.ParseDecimal("" + ex["Amount"].InnerText);

                    if (ex["InventorySiteRef"] != null)
                    {
                        NewInvoice.InventorySiteRef = new InventorySite();
                        NewInvoice.InventorySiteRef.ListID = "" + ex["InventorySiteRef"]["ListID"].InnerText;
                        NewInvoice.InventorySiteRef.Name = "" + ex["InventorySiteRef"]["FullName"].InnerText;
                    }

                    /*if (ex["InventorySiteLocationRef"] != null)
                    {
                        NewInvoice.InventorySiteLocationRef = new InventorySiteLocation();
                        NewInvoice.InventorySiteLocationRef.ListID = "" + ex["InventorySiteLocationRef"]["ListID"].InnerText;
                        NewInvoice.InventorySiteLocationRef.Name = "" + ex["InventorySiteLocationRef"]["FullName"].InnerText;
                    }*/


                    if (ex["SerialNumber"] != null)
                    {
                        NewInvoice.SerialNumber = "" + ex["SerialNumber"].InnerText;
                    }
                    else if (ex["LotNumber"] != null)
                    {
                        NewInvoice.LotNumber = "" + ex["LotNumber"].InnerText;
                    }

                    if (ex["ServiceDate"] != null)
                        NewInvoice.ServiceDate = DateTime.Parse("" + ex["ServiceDate"].InnerText);

                    if (ex["SalesTaxCodeRef"] != null)
                    {
                        NewInvoice.SalesTaxCodeRef = new SalesTaxCode();
                        NewInvoice.SalesTaxCodeRef.ListID = "" + ex["SalesTaxCodeRef"]["ListID"].InnerText;
                        NewInvoice.SalesTaxCodeRef.FullName = "" + ex["SalesTaxCodeRef"]["FullName"].InnerText;
                    }

                    if (ex["Other1"] != null)
                        NewInvoice.Other1 = "" + ex["Other1"].InnerText;

                    if (ex["Other2"] != null)
                        NewInvoice.Other2 = "" + ex["Other2"].InnerText;

                    SalesReceiptLineRet.Add(NewInvoice);
                }


                XmlNodeList __extras = node.SelectNodes("DataExtRet");
                foreach (XmlNode ex in __extras)
                {
                    var name = ex["DataExtName"].InnerText;
                    var value = ex["DataExtValue"].InnerText;
                    var ownerID = ex["OwnerID"].InnerText;
                    AddDataEx(name, value, ownerID, TxnID);
                }
                return true;

                full_loaded = true;

                return true;


            }
            else
            {
                err = statusMessage;
            }
            return false;
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet SalesReceipt";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet SalesReceipt";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet SalesReceipt";
            return new List<Abstract>();
        }



    }
}
