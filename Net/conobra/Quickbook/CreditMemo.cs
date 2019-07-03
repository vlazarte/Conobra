using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;

namespace Quickbook
{
    public class CreditMemo : Abstract
    {
        public string TxnID = "";
        public DateTime TimeCreated;
        public DateTime Modified;
        public string EditSequence;
        public int? TxnNumber;
        public Customer CustomerRef; // required  
        public Class ClassRef; // optional
        public ARAccount ARAccountRef;
        public Template TemplateRef;
        public DateTime? TxnDate;
        public string RefNumber = "";//
        public Address BillAddress;
        public Address BillAddressBlock;
        public Address ShipAddress;
        public Address ShipAddressBlock;
        public bool? IsPending;
        public string PONumber = ""; //
        public Terms TermsRef; //--
        public DateTime? DueDate;
        public SalesRep SalesRepRef;
        public string FOB = "";//
        public DateTime? ShipDate;
        public ShipMethod ShipMethodRef;//--
        public decimal? Subtotal;
        public ItemSalesTax ItemSalesTaxRef;
        public decimal? SalesTaxPercentage;
        public decimal? SalesTaxTotal;//
        public decimal? TotalAmount;
        public decimal? CreditRemaining;
        public Currency CurrencyRef;
        public decimal? ExchangeRate;
        public decimal? CreditRemainingInHomeCurrency;
        public string Memo = "";
        //CustomerMsgRef
        public bool? IsToBePrinted;
        public bool? IsToBeEmailed;
        public CustomerSalesTaxCode CustomerSalesTaxCodeRef;
        public string Other = "";//
        public string ExternalGUID = "";//
        /*
         * 
<LinkedTxn> <!-- optional, may repeat -->
<TxnID >IDTYPE</TxnID> <!-- required -->
<!-- TxnType may have one of the following values: ARRefundCreditCard, Bill, BillPaymentCheck, BillPaymentCreditCard, BuildAssembly, Charge, Check, CreditCardCharge, CreditCardCredit, CreditMemo, Deposit, Estimate, InventoryAdjustment, Invoice, ItemReceipt, JournalEntry, LiabilityAdjustment, Paycheck, PayrollLiabilityCheck, PurchaseOrder, ReceivePayment, SalesOrder, SalesReceipt, SalesTaxPaymentCheck, Transfer, VendorCredit, YTDAdjustment -->
<TxnType >ENUMTYPE</TxnType> <!-- required -->
<TxnDate >DATETYPE</TxnDate> <!-- required -->
<RefNumber >STRTYPE</RefNumber> <!-- optional -->
<!-- LinkType may have one of the following values: AMTTYPE, QUANTYPE -->
<LinkType >ENUMTYPE</LinkType> <!-- optional -->
<Amount >AMTTYPE</Amount> <!-- required -->
</LinkedTxn>
         * */

        public List<CreditMemoLine> CreditMemoLineRet;
        public List<CreditMemoLineGroup> CreditMemoLineGroupRet;

        public List<object> Lines;

        public List<string> LinkedTransactions;

        public CreditMemo()
        {
            Lines = new List<object>();
            LinkedTransactions = new List<string>();
            CreditMemoLineRet = new List<CreditMemoLine>();
            CreditMemoLineGroupRet = new List<CreditMemoLineGroup>();
        }

        public bool isValid()
        {
            if (CustomerRef == null)
                return false;

            return true;
        }

        public static List<CreditMemo> getInvoiceByDate(string from, string to, ref string err)
        {

            XmlDocument doc = new XmlDocument();
            List<CreditMemo> list = new List<CreditMemo>();

            if (Config.IsProduction == true)
            {
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xml += "<?qbxml version=\"13.0\"?>";
                xml += "<QBXML>";
                xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
                xml += "<CreditMemoQueryRq>";

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
                xml += "</CreditMemoQueryRq >";
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

            code = doc["QBXML"]["QBXMLMsgsRs"]["CreditMemoQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["CreditMemoQueryRs"].Attributes["statusMessage"].Value;
            statusSeverity = doc["QBXML"]["QBXMLMsgsRs"]["CreditMemoQueryRs"].Attributes["statusSeverity"].Value;

            if (code == "0")
            {
                var node = doc["QBXML"]["QBXMLMsgsRs"]["CreditMemoQueryRs"];
                var nodes = node.SelectNodes("CreditMemoRet");

                foreach (XmlNode N in nodes)
                {
                    CreditMemo I = new CreditMemo();
                    I.Modified = DateTime.Parse("" + N["TimeModified"].InnerText);
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
                    list = new List<CreditMemo>();
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
                xml += "<CreditMemoQueryRq>";
                if (ref_number != "")
                    xml += "<RefNumber >" + ref_number + "</RefNumber >";
                else if (txn_id != "")
                    xml += "<TxnID >" + txn_id + "</TxnID >";
                xml += "<IncludeLineItems>true</IncludeLineItems>";
                xml += "<OwnerID>0</OwnerID>";
                xml += "</CreditMemoQueryRq >";
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
                string path = Directory.GetCurrentDirectory() + "\\samples\\I_" + ref_number + ".xml";
                doc.Load(@path);
            }

            string code = "";
            string statusMessage = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["CreditMemoQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["CreditMemoQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {
                Lines = new List<object>();
                clearDataExList();
                CreditMemoLineRet.Clear();
                LinkedTransactions.Clear();
                CreditMemoLineGroupRet.Clear();

                var node = doc["QBXML"]["QBXMLMsgsRs"]["CreditMemoQueryRs"]["CreditMemoRet"];

                TxnID = "" + node["TxnID"].InnerText;
                TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                Modified = DateTime.Parse("" + node["TimeModified"].InnerText);
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

                /*
                 <ClassRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</ClassRef>*/

                if (node["ARAccountRef"] != null)
                {
                    ARAccountRef = new ARAccount();
                    ARAccountRef.ListID = "" + node["ARAccountRef"]["ListID"].InnerText;
                    ARAccountRef.FullName = "" + node["ARAccountRef"]["FullName"].InnerText;
                }


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

                /*
                 <PONumber >STRTYPE</PONumber> <!-- optional -->
*/


                if (node["TermsRef"] != null)
                {
                    TermsRef = new Terms();
                    TermsRef.ListID = "" + node["TermsRef"]["ListID"].InnerText;

                    TermsRef.LoadByListID(TermsRef.ListID, ref err);
                    //TermsRef.FullName = "" + node["TermsRef"]["FullName"].InnerText;
                }

                if (node["DueDate"] != null)
                    DueDate = DateTime.Parse("" + node["DueDate"].InnerText);

                if (node["SalesRepRef"] != null)
                {
                    SalesRepRef = new SalesRep();
                    SalesRepRef.ListID = "" + node["SalesRepRef"]["ListID"].InnerText;
                    SalesRepRef.FullName = "" + node["SalesRepRef"]["FullName"].InnerText;
                }

                //<FOB >STRTYPE</FOB> <!-- optional -->

                if (node["ShipDate"] != null)
                    ShipDate = DateTime.Parse("" + node["ShipDate"].InnerText);

                /*
                 <ShipMethodRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</ShipMethodRef>*/

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

                if (node["ApplieTotalAmountdAmount"] != null)
                    TotalAmount = Functions.ParseDecimal("" + node["TotalAmount"].InnerText);

                if (node["CreditRemaining"] != null)
                    CreditRemaining = Functions.ParseDecimal("" + node["CreditRemaining"].InnerText);


                if (node["CurrencyRef"] != null)
                {
                    CurrencyRef = new Currency();
                    CurrencyRef.ListID = "" + node["CurrencyRef"]["ListID"].InnerText;
                    CurrencyRef.FullName = "" + node["CurrencyRef"]["FullName"].InnerText;
                }


                if (node["ExchangeRate"] != null)
                    ExchangeRate = Functions.ParseDecimal("" + node["ExchangeRate"].InnerText);

                if (node["CreditRemainingInHomeCurrency"] != null)
                    CreditRemainingInHomeCurrency = Functions.ParseDecimal("" + node["CreditRemainingInHomeCurrency"].InnerText);

                if (node["Memo"] != null)
                    Memo = "" + node["Memo"].InnerText;


                /*<CustomerMsgRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</CustomerMsgRef>*/

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

                /*<SuggestedDiscountAmount >AMTTYPE</SuggestedDiscountAmount> <!-- optional -->
<SuggestedDiscountDate >DATETYPE</SuggestedDiscountDate> <!-- optional -->
<Other >STRTYPE</Other> <!-- optional -->
<ExternalGUID >GUIDTYPE</ExternalGUID> <!-- optional -->*/

                /*
                 <LinkedTxn> <!-- optional, may repeat -->
<TxnID >IDTYPE</TxnID> <!-- required -->
<!-- TxnType may have one of the following values: ARRefundCreditCard, Bill, BillPaymentCheck, BillPaymentCreditCard, BuildAssembly, Charge, Check, CreditCardCharge, CreditCardCredit, CreditMemo, Deposit, Estimate, InventoryAdjustment, Invoice, ItemReceipt, JournalEntry, LiabilityAdjustment, Paycheck, PayrollLiabilityCheck, PurchaseOrder, ReceivePayment, SalesOrder, SalesReceipt, SalesTaxPaymentCheck, Transfer, VendorCredit, YTDAdjustment -->
<TxnType >ENUMTYPE</TxnType> <!-- required -->
<TxnDate >DATETYPE</TxnDate> <!-- required -->
<RefNumber >STRTYPE</RefNumber> <!-- optional -->
<!-- LinkType may have one of the following values: AMTTYPE, QUANTYPE -->
<LinkType >ENUMTYPE</LinkType> <!-- optional -->
<Amount >AMTTYPE</Amount> <!-- required -->
</LinkedTxn>*/


                foreach (XmlNode Gex in node.ChildNodes)
                {
                    if (!(Gex.Name == "CreditMemoLineGroupRet" || Gex.Name == "CreditMemoLineRet"))
                        continue;

                    if (Gex.Name == "CreditMemoLineGroupRet")
                    {
                        CreditMemoLineGroup LG = new CreditMemoLineGroup();

                        if (Gex["Desc"] != null)
                            LG.Desc = Gex["Desc"].InnerText;

                        if (Gex["ItemGroupRef"] != null)
                        {
                            LG.ItemGroupRef = new ItemGroup();
                            if (Gex["ItemGroupRef"]["FullName"] != null)
                                LG.ItemGroupRef.Name = Gex["ItemGroupRef"]["FullName"].InnerText;

                            if (Gex["ItemGroupRef"]["ListID"] != null)
                                LG.ItemGroupRef.ListID = Gex["ItemGroupRef"]["ListID"].InnerText;
                        }


                        LG.CreditMemoLineRet = new List<CreditMemoLine>();

                        XmlNodeList SListInvoice = Gex.SelectNodes("CreditMemoLineRet");
                        foreach (XmlNode ex in SListInvoice)
                        {
                            CreditMemoLine NewInvoice = new CreditMemoLine();
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

                            LG.CreditMemoLineRet.Add(NewInvoice);
                        }

                        Lines.Add(LG);
                        //CreditMemoLineGroupRet.Add(LG);

                    }
                    else if (Gex.Name == "CreditMemoLineRet")
                    {
                        CreditMemoLine NewInvoice = new CreditMemoLine();
                        NewInvoice.TxnLineID = "" + Gex["TxnLineID"].InnerText;


                        if (Gex["ItemRef"] != null)
                        {
                            NewInvoice.ItemRef = new Item();
                            NewInvoice.ItemRef.ListID = "" + Gex["ItemRef"]["ListID"].InnerText;
                            NewInvoice.ItemRef.FullName = "" + Gex["ItemRef"]["FullName"].InnerText;
                        }
                        if (Gex["Desc"] != null)
                            NewInvoice.Desc = "" + Gex["Desc"].InnerText;

                        if (Gex["Quantity"] != null)
                            NewInvoice.Quantity = Functions.ParseDecimal("" + Gex["Quantity"].InnerText);

                        if (Gex["UnitOfMeasure"] != null)
                            NewInvoice.UnitOfMeasure = "" + Gex["UnitOfMeasure"].InnerText;


                        /*if (ex["OverrideUOMSetRef"] != null)
                        {
                            NewInvoice.OverrideUOMSetRef = new OverrideUOMSet();
                            NewInvoice.OverrideUOMSetRef.ListID = "" + ex["OverrideUOMSetRef"]["ListID"].InnerText;
                            NewInvoice.OverrideUOMSetRef.FullName = "" + ex["OverrideUOMSetRef"]["FullName"].InnerText;
                        }*/


                        if (Gex["Rate"] != null)
                        {
                            NewInvoice.Rate = Functions.ParseDecimal("" + Gex["Rate"].InnerText);
                        }
                        else if (Gex["RatePercent"] != null)
                        {
                            NewInvoice.RatePercent = Functions.ParseDecimal("" + Gex["RatePercent"].InnerText);
                        }


                        if (Gex["ClassRef"] != null)
                        {
                            NewInvoice.ClassRef = new Class();
                            NewInvoice.ClassRef.ListID = "" + Gex["ClassRef"]["ListID"].InnerText;
                            NewInvoice.ClassRef.FullName = "" + Gex["ClassRef"]["FullName"].InnerText;
                        }

                        if (Gex["Amount"] != null)
                            NewInvoice.Amount = Functions.ParseDecimal("" + Gex["Amount"].InnerText);

                        if (Gex["InventorySiteRef"] != null)
                        {
                            NewInvoice.InventorySiteRef = new InventorySite();
                            NewInvoice.InventorySiteRef.ListID = "" + Gex["InventorySiteRef"]["ListID"].InnerText;
                            NewInvoice.InventorySiteRef.Name = "" + Gex["InventorySiteRef"]["FullName"].InnerText;
                        }

                        /*if (ex["InventorySiteLocationRef"] != null)
                        {
                            NewInvoice.InventorySiteLocationRef = new InventorySiteLocation();
                            NewInvoice.InventorySiteLocationRef.ListID = "" + ex["InventorySiteLocationRef"]["ListID"].InnerText;
                            NewInvoice.InventorySiteLocationRef.Name = "" + ex["InventorySiteLocationRef"]["FullName"].InnerText;
                        }*/


                        if (Gex["SerialNumber"] != null)
                        {
                            NewInvoice.SerialNumber = "" + Gex["SerialNumber"].InnerText;
                        }
                        else if (Gex["LotNumber"] != null)
                        {
                            NewInvoice.LotNumber = "" + Gex["LotNumber"].InnerText;
                        }

                        if (Gex["ServiceDate"] != null)
                            NewInvoice.ServiceDate = DateTime.Parse("" + Gex["ServiceDate"].InnerText);

                        if (Gex["SalesTaxCodeRef"] != null)
                        {
                            NewInvoice.SalesTaxCodeRef = new SalesTaxCode();
                            NewInvoice.SalesTaxCodeRef.ListID = "" + Gex["SalesTaxCodeRef"]["ListID"].InnerText;
                            NewInvoice.SalesTaxCodeRef.FullName = "" + Gex["SalesTaxCodeRef"]["FullName"].InnerText;
                        }

                        if (Gex["Other1"] != null)
                            NewInvoice.Other1 = "" + Gex["Other1"].InnerText;

                        if (Gex["Other2"] != null)
                            NewInvoice.Other2 = "" + Gex["Other2"].InnerText;

                        Lines.Add(NewInvoice);
                        //CreditMemoLineRet.Add(NewInvoice);
                    }
                }

                if (Lines.Count > 0)
                {
                    List<object> NewLines = new List<object>();

                    var ID = new ItemDiscount();
                    var lista_descuentos = ID.fetchAll(ref err);

                    for (int i = 0; i < Lines.Count; i++)
                    {
                        var type = Lines[i].GetType().ToString();
                        if (type.IndexOf("CreditMemoLineGroup") >= 0)
                        {
                            CreditMemoLineGroup G = (CreditMemoLineGroup)Lines[i];

                            if (i + 1 < Lines.Count)
                            {
                                for (int j = i + 1; j < Lines.Count; j++)
                                {
                                    var stype = Lines[j].GetType().ToString();
                                    // El siguiente es Linea...
                                    if (stype.IndexOf("CreditMemoLine") >= 0)
                                    {
                                        TransactionLine obj = (TransactionLine)Lines[j];
                                        // Y esa linea es un Descuento..
                                        if (lista_descuentos.Contains(obj.ItemRef.ListID))
                                        {
                                            G.Discounts.Add(obj);
                                            i++;
                                        }
                                        else
                                            break;
                                    }
                                    else
                                        break;
                                }
                            }
                            NewLines.Add(G);
                            CreditMemoLineGroupRet.Add(G);
                        }
                        else if (type.IndexOf("CreditMemoLine") >= 0)
                        {
                            CreditMemoLine L = (CreditMemoLine)Lines[i];
                            NewLines.Add(Lines[i]);
                            CreditMemoLineRet.Add(L);
                        }
                    }

                }

                // or

                /*<InvoiceLineGroupRet> <!-- optional -->
<TxnLineID >IDTYPE</TxnLineID> <!-- required -->
<ItemGroupRef> <!-- required -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</ItemGroupRef>
<Desc >STRTYPE</Desc> <!-- optional -->
<Quantity >QUANTYPE</Quantity> <!-- optional -->
<UnitOfMeasure >STRTYPE</UnitOfMeasure> <!-- optional -->
<OverrideUOMSetRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</OverrideUOMSetRef>
<IsPrintItemsInGroup >BOOLTYPE</IsPrintItemsInGroup> <!-- required -->
<TotalAmount >AMTTYPE</TotalAmount> <!-- required -->
<InvoiceLineRet> <!-- optional, may repeat -->
<TxnLineID >IDTYPE</TxnLineID> <!-- required -->
<ItemRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</ItemRef>
<Desc >STRTYPE</Desc> <!-- optional -->
<Quantity >QUANTYPE</Quantity> <!-- optional -->
<UnitOfMeasure >STRTYPE</UnitOfMeasure> <!-- optional -->
<OverrideUOMSetRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</OverrideUOMSetRef>
<!-- BEGIN OR -->
<Rate >PRICETYPE</Rate> <!-- optional -->
<!-- OR -->
<RatePercent >PERCENTTYPE</RatePercent> <!-- optional -->
<!-- END OR -->
<ClassRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</ClassRef>
<Amount >AMTTYPE</Amount> <!-- optional -->
<InventorySiteRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</InventorySiteRef>
<InventorySiteLocationRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</InventorySiteLocationRef>
<!-- BEGIN OR -->
<SerialNumber >STRTYPE</SerialNumber> <!-- optional -->
<!-- OR -->
<LotNumber >STRTYPE</LotNumber> <!-- optional -->
<!-- END OR -->
<ServiceDate >DATETYPE</ServiceDate> <!-- optional -->
<SalesTaxCodeRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</SalesTaxCodeRef>
<Other1 >STRTYPE</Other1> <!-- optional -->
<Other2 >STRTYPE</Other2> <!-- optional -->
<DataExtRet> <!-- optional, may repeat -->
<OwnerID >GUIDTYPE</OwnerID> <!-- optional -->
<DataExtName >STRTYPE</DataExtName> <!-- required -->
<!-- DataExtType may have one of the following values: AMTTYPE, DATETIMETYPE, INTTYPE, PERCENTTYPE, PRICETYPE, QUANTYPE, STR1024TYPE, STR255TYPE -->
<DataExtType >ENUMTYPE</DataExtType> <!-- required -->
<DataExtValue >STRTYPE</DataExtValue> <!-- required -->
</DataExtRet>
</InvoiceLineRet>
<DataExtRet> <!-- optional, may repeat -->
<OwnerID >GUIDTYPE</OwnerID> <!-- optional -->
<DataExtName >STRTYPE</DataExtName> <!-- required -->
<!-- DataExtType may have one of the following values: AMTTYPE, DATETIMETYPE, INTTYPE, PERCENTTYPE, PRICETYPE, QUANTYPE, STR1024TYPE, STR255TYPE -->
<DataExtType >ENUMTYPE</DataExtType> <!-- required -->
<DataExtValue >STRTYPE</DataExtValue> <!-- required -->
</DataExtRet>
</InvoiceLineGroupRet>*/

                XmlNodeList __extras = node.SelectNodes("DataExtRet");
                foreach (XmlNode ex in __extras)
                {
                    var name = ex["DataExtName"].InnerText;
                    var value = ex["DataExtValue"].InnerText;
                    AddDataEx(name, value);
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


        public string toXmlAdd()
        {
            if (!isValid())
                return null;
            string xml = "";
            xml += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            xml += "<?qbxml version=\"13.0\"?>";
            xml += "<QBXML>";
            xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
            xml += "<InvoiceAddRq>";
            xml += "<InvoiceAdd>";
            if (CustomerRef != null)
                xml += CustomerRef.toXmlRef();
            if (ClassRef != null)
                xml += ClassRef.toXmlRef();
            if (ARAccountRef != null)
                xml += ARAccountRef.toXmlRef();
            if (TemplateRef != null)
                xml += TemplateRef.toXmlRef();

            if (TxnDate != null)
                xml += "<TxnDate>" + Functions.DateToString((DateTime)TxnDate) + "</TxnDate>";

            if (RefNumber != "")
                xml += "<RefNumber>" + RefNumber + "</RefNumber>";

            if (BillAddress != null)
                xml += BillAddress.toXml();

            if (ShipAddress != null)
                xml += ShipAddress.toXml();

            if (IsPending != null)
                xml += "<IsPending>" + (IsPending == true ? "true" : "false") + "</IsPending>";

            if (PONumber != "")
                xml += "<PONumber>" + PONumber + "</PONumber>";

            if (TermsRef != null)
                xml += TermsRef.toXmlRef();

            if (DueDate != null)
                xml += "<DueDate>" + Functions.DateToString((DateTime)DueDate) + "</DueDate>";

            if (SalesRepRef != null)
                xml += SalesRepRef.toXmlRef();

            if (FOB != "")
                xml += "<FOB>" + FOB + "</FOB>";

            if (ShipDate != null)
            {
                xml += "<ShipDate>" + Functions.DateToString((DateTime)ShipDate) + "</ShipDate>";
            }
            //ShipMethodRef optional

            if (ItemSalesTaxRef != null)
                ItemSalesTaxRef.toXmlRef();

            if (Memo != "")
                xml += "<Memo>" + Memo + "</Memo>";

            //CustomerMsgRef

            if (IsToBePrinted != null)
                xml += "<IsToBePrinted>" + (IsToBePrinted == true ? "true" : "false") + "</IsToBePrinted>";
            if (IsToBeEmailed != null)
                xml += "<IsToBeEmailed>" + (IsToBeEmailed == true ? "true" : "false") + "</IsToBeEmailed>";

            if (CustomerSalesTaxCodeRef != null)
                CustomerSalesTaxCodeRef.toXmlRef();


            if (Other != "")
                xml += "<Other>" + Other + "</Other>";

            if (ExchangeRate != null)
                xml += "<ExchangeRate>" + Functions.FloatToString((float)ExchangeRate) + "</ExchangeRate>";

            if (ExternalGUID != "")
                xml += "<ExternalGUID>" + ExternalGUID + "</ExternalGUID>";

            for (int i = 0; i < LinkedTransactions.Count; i++)
            {
                if (LinkedTransactions[i] != "")
                    xml += "<LinkToTxnID>" + LinkedTransactions[i] + "</LinkToTxnID>";
            }

            for (int i = 0; i < CreditMemoLineRet.Count; i++)
            {
                if (CreditMemoLineRet[i].isValid() == true)
                    xml += CreditMemoLineRet[i].toXmlAdd();
            }

            for (int i = 0; i < CreditMemoLineGroupRet.Count; i++)
            {
                if (CreditMemoLineGroupRet[i].isValid() == true)
                    xml += CreditMemoLineGroupRet[i].toXmlAdd();
            }

            xml += "</InvoiceAdd>";
            xml += "</InvoiceAddRq>";
            xml += "</QBXMLMsgsRq>";
            xml += "</QBXML>";

            return xml;
            /*
<SetCredit> <!-- optional, may repeat -->
<CreditTxnID useMacro="MACROTYPE">IDTYPE</CreditTxnID> <!-- required -->
<AppliedAmount >AMTTYPE</AppliedAmount> <!-- required -->
<Override >BOOLTYPE</Override> <!-- optional -->
</SetCredit>*/

        }

        public bool AddRecord(ref string err)
        {


            string xml = toXmlAdd();
            if (xml == null)
            {
                err = "Hubo un error al generar el XML";
                return false;
            }
            var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

            if (qbook.Connect())
            {
                string response = qbook.sendRequest(xml);

                XmlDocument res = new XmlDocument();
                res.LoadXml(response);

                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["InvoiceAddRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["InvoiceAddRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {
                    //string editSequence = "";
                    TxnID = res["QBXML"]["QBXMLMsgsRs"]["InvoiceAddRs"]["InvoiceRet"]["TxnID"].InnerText;
                    RefNumber = res["QBXML"]["QBXMLMsgsRs"]["InvoiceAddRs"]["InvoiceRet"]["RefNumber"].InnerText;
                    //editSequence = res["QBXML"]["QBXMLMsgsRs"]["CustomerAddRs"]["CustomerRet"]["EditSequence"].InnerText;
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
            err = "No implemented yet CreditMemo";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet CreditMemo";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet CreditMemo";
            return new List<Abstract>();
        }

    }
}
