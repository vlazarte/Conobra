using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;

namespace Quickbook
{
    public class Customer : Abstract
    {
        public string Name { get; set; }
        private bool active;

        public string IsActive
        {
            get { return Convert.ToString(active); }
            set
            {
                if (value!=null && value.ToUpper() == "TRUE")
                {
                    active = true;
                }
                else
                {
                    active = false;
                }

            }
        }
        public Class ClassRef { get; set; }// optional
        public Customer ParentRef { get; set; } // optional
        public string CompanyName { get; set; } // optional
        public string Salutation { get; set; } // optional

        public string FirstName { get; set; } // optional
        public string MiddleName { get; set; } // optional
        public string LastName { get; set; } // optional
        public string JobTitle { get; set; }   // optional

        public Address BillAddress { get; set; }
        public Address BillAddressBlock { get; set; }


        public Address ShipAddress { get; set; }
        public Address ShipAddressBlock { get; set; }
        public List<Address> ShipToAddress { get; set; } // optional 0 - 50 veces

        public string Phone { get; set; } // optional
        public string AltPhone { get; set; } // optional
        public string Fax { get; set; }  // optional
        public string Email { get; set; }  // optional
        public string Cc { get; set; }  // optional
        public string Contact { get; set; }  // optional
        public string AltContact { get; set; }  // optional



        public List<AdditionalContact> AdditionalContactRef { get; set; }  // 0 - 8 items
        public List<Contact> Contacts { get; set; }  // optional
        public CustomerType CustomerTypeRef { get; set; }  // optional
        public Terms TermsRef { get; set; }  // optional
        public SalesRep SalesRepRef { get; set; }  // optional
        
        public double OpenBalance { get; set; }  // optional
        public DateTime OpenBalanceDate { get; set; }  // optional
        public SalesTaxCode SalesTaxCodeRef { get; set; }  // optional

        public ItemSalesTax ItemSalesTaxRef { get; set; }  // optional
        public string ResaleNumber { get; set; }
        public string AccountNumber { get; set; }  // optional
        public double CreditLimit { get; set; }  // optional
        public PreferredPaymentMethod PreferredPaymentMethodRef { get; set; }  // optional
        public CreditCard CreditCardInfo { get; set; }  // optional


        public string JobStatus { get; set; }   // optional
        public DateTime? JobStartDate { get; set; } // optional
        public DateTime JobProjectedEndDate { get; set; }  // optional
        public DateTime JobEndDate { get; set; }  // optional
        public string JobDesc { get; set; } // optional
        public JobType JobTypeRef { get; set; }   // optional
        public string Notes { get; set; } // optional
        public List<string> AdditionalNotes { get; set; }  // optional

        public string PreferredDeliveryMethod = string.Empty; // optional
        public PriceLevel PriceLevelRef;
        public string ExternalGUID = string.Empty; // optional
        public Currency CurrencyRef { get; set; }  // optional

        public double? Balance { get; set; }
        public double? TotalBalance { get; set; }


        public DateTime TimeCreated { get; set; }
        public DateTime Modified { get; set; }
        public string EditSequence { get; set; }
        public string FullName { get; set; }
        public int Sublevel { get; set; }


        public Customer()
        {
            ClassRef = null;
            ParentRef = null;
            CustomerTypeRef = null;
            TermsRef = null;
            SalesRepRef = null;
            SalesTaxCodeRef = null;
            ItemSalesTaxRef = null;
            PreferredPaymentMethodRef = null;
            CreditCardInfo = null;
            JobTypeRef = null;
            PriceLevelRef = null;
            CurrencyRef = null;
            BillAddress = null;
            ShipAddress = null;
            FullName = string.Empty;
            ShipToAddress = new List<Address>();
            AdditionalContactRef = new List<AdditionalContact>();
            Contacts = new List<Contact>();
            AdditionalNotes = new List<string>();

            IsActive = null;
            Sublevel = -1;
            CompanyName = string.Empty;
            Salutation = string.Empty;
            FirstName = string.Empty; // optional
            MiddleName = string.Empty; // optional
            JobDesc = string.Empty;
            JobStartDate = null;
            JobTitle = string.Empty;
            Phone = string.Empty;
            AltPhone = string.Empty;
            Fax = string.Empty; // optional
            Email = string.Empty; // optional
            Cc = string.Empty; // optional
            Contact = string.Empty; // optional
            AltContact = string.Empty; // optional
            Balance = null;
            TotalBalance = null;
            ResaleNumber = string.Empty; // optional
            AccountNumber = string.Empty; // optional
            JobStatus = string.Empty; // optional
            Notes = string.Empty; // optional

            ObjectName = "Customer";
        }
        public bool isValid()
        {
            return Name != "";
        }

        public bool LoadByListID(string ListID, ref string err)
        {
            if (full_loaded == true)
            {
                return true;
            }

            XmlDocument doc = new XmlDocument();

            if (Config.IsProduction == true)
            {
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                xml += "<?qbxml version=\"13.0\"?>";
                xml += "<QBXML>";
                xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
                xml += "<CustomerQueryRq>";
                xml += "<ListID >" + ListID + "</ListID >";
                xml += "<OwnerID>0</OwnerID>";
                xml += "</CustomerQueryRq >";
                xml += "</QBXMLMsgsRq>";
                xml += "</QBXML>";
                var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

                if (qbook.Connect())
                {
                    string response = qbook.sendRequest(xml);
                    doc.LoadXml(response);

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
                string pathFile = Directory.GetCurrentDirectory() + "\\samples\\C_" + ListID + ".xml";
                doc.Load(@pathFile);
            }

            string code = "";
            string statusMessage = "";

            code = doc["QBXML"]["QBXMLMsgsRs"]["CustomerQueryRs"].Attributes["statusCode"].Value;
            statusMessage = doc["QBXML"]["QBXMLMsgsRs"]["CustomerQueryRs"].Attributes["statusMessage"].Value;

            if (code == "0")
            {
                var node = doc["QBXML"]["QBXMLMsgsRs"]["CustomerQueryRs"]["CustomerRet"];

                this.ListID = ListID;
                TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                Modified = DateTime.Parse("" + node["TimeModified"].InnerText);
                EditSequence = "" + node["EditSequence"].InnerText;
                Name = "" + node["Name"].InnerText;
                FullName = "" + node["FullName"].InnerText;

                if (node["IsActive"] != null)
                    active = ("" + node["IsActive"].InnerText) == "true" ? true : false;

                if (node["ParentRef"] != null)
                {
                    ParentRef = new Customer();
                    ParentRef.ListID = "" + node["ParentRef"]["ListID"].InnerText;
                    ParentRef.FullName = "" + node["ParentRef"]["FullName"].InnerText;
                }

                if (node["Sublevel"] != null)
                    Sublevel = Int32.Parse("" + node["Sublevel"].InnerText);

                if (node["CompanyName"] != null)
                    CompanyName = "" + node["CompanyName"].InnerText;

                if (node["FirstName"] != null)
                    FirstName = "" + node["FirstName"].InnerText;

                if (node["LastName"] != null)
                    LastName = "" + node["LastName"].InnerText;


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
                    for (int i = 1; i <= 5; i++)
                    {
                        if (node["ShipAddressBlock"]["Addr" + i] != null)
                        {
                            ShipAddressBlock.addAddrLine("" + node["ShipAddressBlock"]["Addr" + i].InnerText);
                        }

                    }
                }

                // ShipToAddress
                ShipToAddress = new List<Address>();
                XmlNodeList listShipTo = node.SelectNodes("ShipToAddress");
                foreach (XmlNode ex in listShipTo)
                {
                    Address A = new Address(Address.Ship_To_Address);
                    if (ex["Name"] != null)
                        A.Name = "" + ex["Name"].InnerText;

                    for (int i = 1; i <= 5; i++)
                    {
                        if (ex["Addr" + i] != null)
                        {
                            A.addAddrLine("" + ex["Addr" + i].InnerText);
                        }

                    }

                    if (ex["City"] != null)
                        A.City = "" + ex["City"].InnerText;
                    if (ex["State"] != null)
                        A.State = "" + ex["State"].InnerText;
                    if (ex["PostalCode"] != null)
                        A.PostalCode = "" + ex["PostalCode"].InnerText;
                    //if (ex["DefaultShipTo"] != null)
                    //A. = "" + ex["DefaultShipTo"].InnerText;

                    ShipToAddress.Add(A);
                }

                if (node["Phone"] != null)
                    Phone = "" + node["Phone"].InnerText;

                if (node["Contact"] != null)
                    Contact = "" + node["Contact"].InnerText;

                AdditionalContactRef = new List<AdditionalContact>();
                XmlNodeList listAddContact = node.SelectNodes("AdditionalContactRef");
                foreach (XmlNode ex in listAddContact)
                {
                    AdditionalContact AC = new AdditionalContact();

                    if (ex["ContactName"] != null)
                        AC.ContactName = "" + ex["ContactName"].InnerText;
                    if (ex["ContactValue"] != null)
                        AC.ContactValue = "" + ex["ContactValue"].InnerText;

                    AdditionalContactRef.Add(AC);
                }


                // ContactsRet multiple
                /*
                <ContactsRet> <!-- optional, may repeat -->
                    <ListID >IDTYPE</ListID> <!-- required -->
                    <TimeCreated >DATETIMETYPE</TimeCreated> <!-- required -->
                    <TimeModified >DATETIMETYPE</TimeModified> <!-- required -->
                    <EditSequence >STRTYPE</EditSequence> <!-- required -->
                    <Contact >STRTYPE</Contact> <!-- optional -->
                    <Salutation >STRTYPE</Salutation> <!-- optional -->
                    <FirstName >STRTYPE</FirstName> <!-- required -->
                    <MiddleName >STRTYPE</MiddleName> <!-- optional -->
                    <LastName >STRTYPE</LastName> <!-- optional -->
                    <JobTitle >STRTYPE</JobTitle> <!-- optional -->
                    <AdditionalContactRef> <!-- must occur 0 - 5 times -->
                        <ContactName >STRTYPE</ContactName> <!-- required -->
                        <ContactValue >STRTYPE</ContactValue> <!-- required -->
                    </AdditionalContactRef>
                </ContactsRet>
                    */

                if (node["CustomerTypeRef"] != null)
                {
                    CustomerTypeRef = new CustomerType();
                    CustomerTypeRef.ListID = "" + node["CustomerTypeRef"]["ListID"].InnerText;
                    CustomerTypeRef.FullName = "" + node["CustomerTypeRef"]["FullName"].InnerText;
                }

                if (node["TermsRef"] != null)
                {
                    TermsRef = new Terms();
                    TermsRef.ListID = "" + node["TermsRef"]["ListID"].InnerText;
                    TermsRef.FullName = "" + node["TermsRef"]["FullName"].InnerText;
                }

                if (node["SalesRepRef"] != null)
                {
                    SalesRepRef = new SalesRep();
                    SalesRepRef.ListID = "" + node["SalesRepRef"]["ListID"].InnerText;
                    SalesRepRef.FullName = "" + node["SalesRepRef"]["FullName"].InnerText;
                }

                //if (node["Balance"] != null)
                  //  Balance = Functions.ParseDecimal("" + node["Balance"].InnerText);

                //if (node["TotalBalance"] != null)
                //    TotalBalance = Functions.ParseDecimal("" + node["TotalBalance"].InnerText);

                if (node["JobStatus"] != null)
                    JobStatus = "" + node["JobStatus"].InnerText;

                if (node["PreferredDeliveryMethod"] != null)
                    PreferredDeliveryMethod = "" + node["PreferredDeliveryMethod"].InnerText;

                if (node["PriceLevelRef"] != null)
                {
                    PriceLevelRef = new PriceLevel();
                    PriceLevelRef.ListID = "" + node["PriceLevelRef"]["ListID"].InnerText;
                    PriceLevelRef.FullName = "" + node["PriceLevelRef"]["FullName"].InnerText;
                }

                if (node["Email"] != null)
                    Email = "" + node["Email"].InnerText;

                if (node["Cc"] != null)
                    Cc = "" + node["Cc"].InnerText;


                XmlNodeList listDataEx = node.SelectNodes("DataExtRet");
                foreach (XmlNode ex in listDataEx)
                {
                    string de_name = "" + ex["DataExtName"].InnerText;
                    string de_type = "" + ex["DataExtType"].InnerText;
                    string de_value = "" + ex["DataExtValue"].InnerText;

                    AddDataEx(de_name, de_value);

                }

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
            /*if (!hasConnector())
                throw new Exception("Es necesario vincular la conexion a Quickbook");*/
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
                    string pathFile = Directory.GetCurrentDirectory() + "\\samples\\NewCustomer_" + DateTime.Now.Ticks + ".xml";
                    File.WriteAllText(pathFile, xml);
                }

                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["CustomerAddRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["CustomerAddRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {

                    //string editSequence = "";
                    ListID = res["QBXML"]["QBXMLMsgsRs"]["CustomerAddRs"]["CustomerRet"]["ListID"].InnerText;
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

        public string toXmlAdd()
        {
            if (!isValid())
                return null;
            string xml = "";
            XmlElement ele = (new XmlDocument()).CreateElement("test");


            xml += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            xml += "<?qbxml version=\"13.0\"?>";
            xml += "<QBXML>";
            xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
            xml += "<CustomerAddRq>";
            xml += "<CustomerAdd>";

            ele.InnerText = Name + "";

            xml += "<Name >" + ele.InnerXml + "</Name>";
            if (IsActive != null)
                xml += "<IsActive >" + (active == true ? "true" : "false") + "</IsActive>";
            if (ClassRef != null)
                xml += ClassRef.toXmlRef();
            if (ParentRef != null)
            {
                xml += "<ParentRef>";
                if (ParentRef.ListID != string.Empty)
                {
                    xml += "<ListID >" + ParentRef.ListID + "</ListID>";
                }
                if (ParentRef.FullName != string.Empty)
                {
                    ele.InnerText = ParentRef.FullName + "";
                    xml += "<FullName >" + ele.InnerXml + "</FullName>";
                }

                xml += "</ParentRef>";
            }
            if (CompanyName != "")
            {
                ele.InnerText = CompanyName + "";
                xml += "<CompanyName >" + ele.InnerXml + "</CompanyName>";
            }
            if (Salutation != "")
                xml += "<Salutation >" + Salutation + "</Salutation>";
            if (FirstName != "")
            {
                ele.InnerText = FirstName + "";
                xml += "<FirstName >" + ele.InnerXml + "</FirstName>";
            }
            if (MiddleName != "")
            {
                ele.InnerText = MiddleName + "";
                xml += "<MiddleName >" + ele.InnerXml + "</MiddleName>";
            }
            if (LastName != "")
            {
                ele.InnerText = LastName + "";
                xml += "<LastName >" + LastName + "</LastName>";
            }
            if (JobTitle != "")
            {
                ele.InnerText = JobTitle + "";
                xml += "<JobTitle >" + ele.InnerXml + "</JobTitle>";
            }

            if (BillAddress != null)
            {
                xml += BillAddress.toXml();
            }
            if (ShipAddress != null)
            {
                xml += ShipAddress.toXml();
            }
            if (ShipToAddress.Count > 0)
            {
                for (int i = 0; i < ShipToAddress.Count; i++)
                {
                    xml += ShipToAddress[i].toXml();
                }
            }

            if (Phone != "")
                xml += "<Phone >" + Phone + "</Phone>";
            if (AltPhone != "")
                xml += "<AltPhone >" + AltPhone + "</AltPhone>";
            if (Fax != "")
                xml += "<Fax >" + Fax + "</Fax>";
            if (Email != "")
                xml += "<Email >" + Email + "</Email>";
            if (Cc != "")
                xml += "<Cc >" + Cc + "</Cc>";
            if (Contact != "")
                xml += "<Contact >" + Contact + "</Contact>";
            if (AltContact != "")
                xml += "<AltContact >" + AltContact + "</AltContact>";

            if (AdditionalContactRef.Count > 0)
            {
                for (int i = 0; i < AdditionalContactRef.Count; i++)
                {
                    xml += AdditionalContactRef[i].toXmlRef();
                }
            }

            if (Contacts.Count > 0)
            {
                for (int i = 0; i < Contacts.Count; i++)
                {
                    xml += Contacts[i].toXmlRef();
                }
            }

            if (CustomerTypeRef != null)
            {
                xml += CustomerTypeRef.toXmlRef();
            }

            if (TermsRef != null)
            {
                xml += TermsRef.toXmlRef();
            }

            if (SalesRepRef != null)
            {
                xml += SalesRepRef.toXmlRef();
            }

            //xml += "<OpenBalance >AMTTYPE</OpenBalance>" + 
            //xml += "<OpenBalanceDate >DATETYPE</OpenBalanceDate>" + 

            if (SalesTaxCodeRef != null)
            {
                xml += SalesTaxCodeRef.toXmlRef();
            }

            if (ItemSalesTaxRef != null)
            {
                xml += ItemSalesTaxRef.toXmlRef();
            }

            if (ResaleNumber != "")
                xml += "<ResaleNumber >" + ResaleNumber + "</ResaleNumber>";

            if (AccountNumber != "")
                xml += "<AccountNumber >" + AccountNumber + "</AccountNumber>";

            //xml += "<CreditLimit >AMTTYPE</CreditLimit>" + 

            if (PreferredPaymentMethodRef != null)
            {
                xml += PreferredPaymentMethodRef.toXmlRef();
            }

            if (CreditCardInfo != null)
            {
                xml += CreditCardInfo.toXml();
            }

            //xml += "<JobStatus >ENUMTYPE</JobStatus>";

            if (JobStartDate != null)
            {
                string DateString = JobStartDate.ToString();
                DateTime dt = Convert.ToDateTime(DateString);
                xml += "<JobStartDate >" + dt.ToString("yyy-MM-dd") + "</JobStartDate>";
            }
            //xml += "<JobProjectedEndDate >DATETYPE</JobProjectedEndDate>" + 
            //xml += "<JobEndDate >DATETYPE</JobEndDate>" + 


            if (JobDesc != "")
            {
                ele.InnerText = JobDesc + "";
                xml += "<JobDesc >" + ele.InnerXml + "</JobDesc>";
            }


            if (JobTypeRef != null)
            {
                xml += JobTypeRef.toXmlRef();
            }

            if (Notes != "")
            {
                ele.InnerText = Notes + "";
                xml += "<Notes >" + ele.InnerXml + "</Notes>";
            }

            for (int i = 0; i < AdditionalNotes.Count; i++)
            {
                xml += "<AdditionalNotes>";
                xml += "<Note >" + AdditionalNotes[i] + "</Note>";
                xml += "</AdditionalNotes>";
            }

            if (PreferredDeliveryMethod != "")
                xml += "<PreferredDeliveryMethod >" + PreferredDeliveryMethod + "</PreferredDeliveryMethod>";

            if (PriceLevelRef != null)
            {
                xml += PriceLevelRef.toXmlRef();
            }

            //xml += "<ExternalGUID >GUIDTYPE</ExternalGUID>" +
            if (CurrencyRef != null)
            {
                xml += CurrencyRef.toXmlRef();
            }

            xml += "</CustomerAdd>";
            //xml += "<IncludeRetElement >STRTYPE</IncludeRetElement>";
            xml += "</CustomerAddRq>";
            xml += "</QBXMLMsgsRq>";
            xml += "</QBXML>";

            return xml;

        }

        public string toXmlRef()
        {

            StringBuilder xml = new StringBuilder();
            XmlElement ele = (new XmlDocument()).CreateElement("test");
            xml.Append("<CustomerRef>");
            if (ListID != string.Empty)
            {
                xml.Append(Environment.NewLine + "<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {
                ele.InnerText = FullName + "";
                xml.Append(Environment.NewLine + "<FullName>" + ele.InnerXml + "</FullName>"); //-- required -->
            }

            xml.Append("</CustomerRef>");

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
                xml.Append("<FullName>" + ele.InnerXml + "</FullName>");
            }

            xml.Append("</ParentRef>");
            return xml.ToString();



        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            try
            {
                string xml = toXmlQuery(true);
                if (xml == null)
                {
                    err = "Hubo un error al generar el XML" + Environment.NewLine;
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
                            string pathFile = Directory.GetCurrentDirectory() + "\\samples\\V_" + DateTime.Now.Ticks + ".xml";
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
                    //Datos de prueba
                    string pathFile = Directory.GetCurrentDirectory() + "\\samples\\NewCustomer_Test.xml";
                    string response = File.ReadAllText(pathFile);

                    res.LoadXml(response);
                }


                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["CustomerQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["CustomerQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {

                    List<Abstract> quickbookListCustomer = new List<Abstract>();
                    XmlNodeList rets = res.SelectNodes("/QBXML/QBXMLMsgsRs/CustomerQueryRs/CustomerRet");
                    quickbookListCustomer = GetCustomers(rets, includeSublevel);

                    if (quickbookListCustomer.Count > 0)
                    {
                        return quickbookListCustomer;
                    }
                    else
                    {
                        err = "No se obtuvo ningun registro" + Environment.NewLine;
                    }

                }
                else
                {
                    err = statusMessage;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error al conectar al Obtener Customers registros de Quickbooks: " + ex.Message);
            }

            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {

            try
            {
                string xml = toXmlQuery(false);
                if (xml == null)
                {
                    err = "Hubo un error al generar el XML" + Environment.NewLine;
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
                        err = "QuickBooks no conecto" + Environment.NewLine;
                    }
                }
                else
                {
                    //Datos de prueba
                    string pathFile = Directory.GetCurrentDirectory() + "\\samples\\NewCustomer_Test.xml";
                    string response = File.ReadAllText(pathFile);

                    res.LoadXml(response);

                }

                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["CustomerQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["CustomerQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {

                    List<Abstract> quickbookListCustomer = new List<Abstract>();
                    XmlNodeList rets = res.SelectNodes("/QBXML/QBXMLMsgsRs/CustomerQueryRs/CustomerRet");
                    quickbookListCustomer = GetCustomers(rets, includeSublevel);

                    if (quickbookListCustomer.Count > 0)
                    {
                        return quickbookListCustomer;
                    }
                    else
                    {
                        err = "No se obtuvo ningun registro" + Environment.NewLine;
                    }

                }
                else
                {
                    err = statusMessage + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en Customer: " + ex.Message + Environment.NewLine);
            }

            return new List<Abstract>();
        }
        public string toXmlQuery(bool includeFilter)
        {

            XmlElement ele = (new XmlDocument()).CreateElement("test");

            StringBuilder toXML = new StringBuilder();

            toXML.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            toXML.Append("<?qbxml version=\"13.0\"?>");
            toXML.Append("<QBXML>");
            toXML.Append("<QBXMLMsgsRq onError=\"stopOnError\">");
            toXML.Append("<CustomerQueryRq>");

            if (includeFilter)
            {
                toXML.Append(" <FromModifiedDate >" + DateTime.Now.ToString("yyy-MM-dd") + "T00:00:00-04:00</FromModifiedDate>");
                toXML.Append(" <ToModifiedDate >" + DateTime.Now.ToString("yyy-MM-dd") + "T23:59:59-04:00</ToModifiedDate> ");

            }



            toXML.Append("<IncludeRetElement>ListID</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Name</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>FullName</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>IsActive</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ClassRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ParentRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Sublevel</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>CompanyName</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Salutation</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>FirstName</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>MiddleName</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>LastName</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>JobTitle</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>BillAddress</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>BillAddressBlock</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ShipAddress</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ShipAddressBlock</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ShipToAddress</IncludeRetElement>");

            toXML.Append("<IncludeRetElement>Phone</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>AltPhone</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Fax</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Email</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Cc</IncludeRetElement>");


            toXML.Append("<IncludeRetElement>Contact</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>AltContact</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>AdditionalContactRef</IncludeRetElement>");

            toXML.Append("<IncludeRetElement>ContactsRet</IncludeRetElement>");

            toXML.Append("<IncludeRetElement>CustomerTypeRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>TermsRef</IncludeRetElement>");

            toXML.Append("<IncludeRetElement>SalesRepRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Balance</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>TotalBalance</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>SalesTaxCodeRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ItemSalesTaxRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ResaleNumber</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>AccountNumber</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>CreditLimit</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>PreferredPaymentMethodRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>CreditCardInfo</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>JobStatus</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>JobStartDate</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>JobProjectedEndDate</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>JobEndDate</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>JobDesc</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>JobTypeRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Notes</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>AdditionalNotesRet</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>PreferredDeliveryMethod</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>PriceLevelRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ExternalGUID</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>CurrencyRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>DataExtRet</IncludeRetElement>");
            toXML.Append("<OwnerID>0</OwnerID>");
            toXML.Append("</CustomerQueryRq>");
            toXML.Append("</QBXMLMsgsRq>");
            toXML.Append("</QBXML>");

            return toXML.ToString();
        }
        private List<Abstract> GetCustomers(XmlNodeList rets, bool includeSublevel)
        {


            List<Abstract> quickbookListCustomer = new List<Abstract>();

            string message = string.Empty;

            foreach (XmlNode node in rets)
            {
                Customer toUpdate = new Customer();
                if (node["Sublevel"] != null)
                {
                    toUpdate.Sublevel = Convert.ToInt16(node["Sublevel"].InnerText);
                }
                //False =solo padres , true, solo hijos
                if (!includeSublevel)
                {
                    if (toUpdate.Sublevel != 0)
                    {
                        continue;
                    }
                }
                else
                {
                    if (toUpdate.Sublevel == 0)
                    {
                        continue;
                    }
                }
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

                if (node.SelectNodes("ClassRef").Count > 0)
                {
                    toUpdate.ClassRef = new Class();

                    toUpdate.ClassRef.ListID = node["ClassRef"].FirstChild.InnerText;
                    toUpdate.ClassRef.FullName = node["ClassRef"].LastChild.InnerText;

                }
                if (node["ParentRef"] != null)
                {
                    toUpdate.ParentRef = new Customer();
                    toUpdate.ParentRef.ListID = node["ClassRef"].FirstChild.InnerText;
                    toUpdate.ParentRef.FullName = node["ClassRef"].LastChild.InnerText;
                }
                if (node["CompanyName"] != null)
                {
                    toUpdate.CompanyName = node["CompanyName"].InnerText;
                }

                if (node["Salutation"] != null)
                {
                    toUpdate.Salutation = node["Salutation"].InnerText;
                }
                if (node["FirstName"] != null)
                {
                    toUpdate.FirstName = node["FirstName"].InnerText;
                }
                if (node["MiddleName"] != null)
                {
                    toUpdate.MiddleName = node["MiddleName"].InnerText;
                }

                if (node["LastName"] != null)
                {
                    toUpdate.LastName = node["LastName"].InnerText;
                }

                if (node["JobTitle"] != null)
                {
                    toUpdate.JobTitle = node["JobTitle"].InnerText;
                }

                if (node.SelectNodes("BillAddress").Count > 0)
                {
                    toUpdate.BillAddress = new Address(0);
                    if (node["BillAddress"]["Addr1"] != null)
                    {
                        toUpdate.BillAddress.Addr1 = node["BillAddress"]["Addr1"].InnerText;
                    }
                    if (node["BillAddress"]["Addr2"] != null)
                    {
                        toUpdate.BillAddress.Addr2 = node["BillAddress"]["Addr2"].InnerText;
                    }
                    if (node["BillAddress"]["Addr3"] != null)
                    {
                        toUpdate.BillAddress.Addr3 = node["BillAddress"]["Addr3"].InnerText;
                    }
                    if (node["BillAddress"]["Addr4"] != null)
                    {
                        toUpdate.BillAddress.Addr4 = node["BillAddress"]["Addr4"].InnerText;
                    }
                    if (node["BillAddress"]["Addr5"] != null)
                    {
                        toUpdate.BillAddress.Addr5 = node["BillAddress"]["Addr5"].InnerText;
                    }
                    if (node["BillAddress"]["City"] != null)
                    {
                        toUpdate.BillAddress.City = node["BillAddress"]["City"].InnerText;
                    }
                    if (node["BillAddress"]["Country"] != null)
                    {
                        toUpdate.BillAddress.Country = node["BillAddress"]["Country"].InnerText;
                    }
                    if (node["BillAddress"]["Note"] != null)
                    {
                        toUpdate.BillAddress.Note = node["BillAddress"]["Note"].InnerText;
                    }
                    if (node["BillAddress"]["PostalCode"] != null)
                    {
                        toUpdate.BillAddress.PostalCode = node["BillAddress"]["PostalCode"].InnerText;
                    }
                    if (node["BillAddress"]["State"] != null)
                    {
                        toUpdate.BillAddress.State = node["BillAddress"]["State"].InnerText;
                    }
                }

                if (node.SelectNodes("BillAddressBlock").Count > 0)
                {
                    toUpdate.BillAddressBlock = new Address(3);
                    if (node["BillAddressBlock"]["Addr1"] != null)
                    {
                        toUpdate.BillAddress.Addr1 = node["BillAddressBlock"]["Addr1"].InnerText;
                    }
                    if (node["BillAddressBlock"]["Addr2"] != null)
                    {
                        toUpdate.BillAddress.Addr2 = node["BillAddressBlock"]["Addr2"].InnerText;
                    }
                    if (node["BillAddressBlock"]["Addr3"] != null)
                    {
                        toUpdate.BillAddress.Addr3 = node["BillAddressBlock"]["Addr3"].InnerText;
                    }
                    if (node["BillAddressBlock"]["Addr4"] != null)
                    {
                        toUpdate.BillAddress.Addr4 = node["BillAddressBlock"]["Addr4"].InnerText;
                    }
                    if (node["BillAddressBlock"]["Addr5"] != null)
                    {
                        toUpdate.BillAddress.Addr5 = node["BillAddressBlock"]["Addr5"].InnerText;
                    }

                }


                if (node.SelectNodes("ShipAddress").Count > 0)
                {
                    toUpdate.ShipAddress = new Address(1);
                    if (node["ShipAddress"]["Addr1"] != null)
                    {
                        toUpdate.ShipAddress.Addr1 = node["ShipAddress"]["Addr1"].InnerText;
                    }
                    if (node["ShipAddress"]["Addr2"] != null)
                    {
                        toUpdate.ShipAddress.Addr2 = node["ShipAddress"]["Addr2"].InnerText;
                    }
                    if (node["ShipAddress"]["Addr3"] != null)
                    {
                        toUpdate.ShipAddress.Addr3 = node["ShipAddress"]["Addr3"].InnerText;
                    }
                    if (node["ShipAddress"]["Addr4"] != null)
                    {
                        toUpdate.ShipAddress.Addr4 = node["ShipAddress"]["Addr4"].InnerText;
                    }
                    if (node["ShipAddress"]["Addr5"] != null)
                    {
                        toUpdate.ShipAddress.Addr5 = node["ShipAddress"]["Addr5"].InnerText;
                    }
                    if (node["ShipAddress"]["City"] != null)
                    {
                        toUpdate.ShipAddress.City = node["ShipAddress"]["City"].InnerText;
                    }
                    if (node["ShipAddress"]["Country"] != null)
                    {
                        toUpdate.ShipAddress.Country = node["ShipAddress"]["Country"].InnerText;
                    }
                    if (node["ShipAddress"]["Note"] != null)
                    {
                        toUpdate.ShipAddress.Note = node["ShipAddress"]["Note"].InnerText;
                    }
                    if (node["ShipAddress"]["PostalCode"] != null)
                    {
                        toUpdate.ShipAddress.PostalCode = node["ShipAddress"]["PostalCode"].InnerText;
                    }
                    if (node["ShipAddress"]["State"] != null)
                    {
                        toUpdate.ShipAddress.State = node["ShipAddress"]["State"].InnerText;
                    }
                }
                if (node.SelectNodes("ShipAddressBlock").Count > 0)
                {
                    toUpdate.ShipAddressBlock = new Address(4);
                    if (node["ShipAddressBlock"]["Addr1"] != null)
                    {
                        toUpdate.ShipAddressBlock.Addr1 = node["ShipAddressBlock"]["Addr1"].InnerText;
                    }
                    if (node["ShipAddressBlock"]["Addr2"] != null)
                    {
                        toUpdate.ShipAddressBlock.Addr2 = node["ShipAddressBlock"]["Addr2"].InnerText;
                    }
                    if (node["ShipAddressBlock"]["Addr3"] != null)
                    {
                        toUpdate.ShipAddressBlock.Addr3 = node["ShipAddressBlock"]["Addr3"].InnerText;
                    }
                    if (node["ShipAddressBlock"]["Addr4"] != null)
                    {
                        toUpdate.ShipAddressBlock.Addr4 = node["ShipAddressBlock"]["Addr4"].InnerText;
                    }
                    if (node["ShipAddressBlock"]["Addr5"] != null)
                    {
                        toUpdate.ShipAddressBlock.Addr5 = node["ShipAddressBlock"]["Addr5"].InnerText;
                    }

                }


                if (node.SelectNodes("ShipToAddress").Count > 0)
                {
                    toUpdate.ShipToAddress = new List<Address>();
                    XmlNodeList nodes = node.SelectNodes("ShipToAddress");
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        Address ShipToAddress = new Address(4);
                        if (nodes[i]["Addr1"] != null)
                        {
                            ShipToAddress.Addr1 = nodes[i]["Addr1"].InnerText;
                        }
                        if (nodes[i]["Addr2"] != null)
                        {
                            ShipToAddress.Addr2 = nodes[i]["Addr2"].InnerText;
                        }
                        if (nodes[i]["Addr3"] != null)
                        {
                            ShipToAddress.Addr3 = nodes[i]["Addr3"].InnerText;
                        }
                        if (nodes[i]["Addr4"] != null)
                        {
                            ShipToAddress.Addr4 = nodes[i]["Addr4"].InnerText;
                        }
                        if (nodes[i]["Addr5"] != null)
                        {
                            ShipToAddress.Addr5 = nodes[i]["Addr5"].InnerText;
                        }
                        if (nodes[i]["City"] != null)
                        {
                            ShipToAddress.City = nodes[i]["City"].InnerText;
                        }
                        if (nodes[i]["Country"] != null)
                        {
                            ShipToAddress.Country = nodes[i]["Country"].InnerText;
                        }
                        if (nodes[i]["Note"] != null)
                        {
                            ShipToAddress.Note = nodes[i]["Note"].InnerText;
                        }
                        if (nodes[i]["PostalCode"] != null)
                        {
                            ShipToAddress.PostalCode = nodes[i]["PostalCode"].InnerText;
                        }
                        if (nodes[i]["State"] != null)
                        {
                            ShipToAddress.State = nodes[i]["State"].InnerText;
                        }

                        toUpdate.ShipToAddress.Add(ShipToAddress);
                    }

                }

                if (node["Phone"] != null)
                {
                    toUpdate.Phone = node["Phone"].InnerText;
                }
                if (node["AltPhone"] != null)
                {
                    toUpdate.AltPhone = node["AltPhone"].InnerText;
                }
                if (node["Fax"] != null)
                {
                    toUpdate.Fax = node["Fax"].InnerText;
                }
                if (node["Email"] != null)
                {
                    toUpdate.Email = node["Email"].InnerText;
                }
                if (node["Cc"] != null)
                {
                    toUpdate.Cc = node["Cc"].InnerText;
                }
                if (node["Contact"] != null)
                {
                    toUpdate.Contact = node["Contact"].InnerText;
                }
                if (node["AltContact"] != null)
                {
                    toUpdate.AltContact = node["AltContact"].InnerText;
                }

                if (node.SelectNodes("AdditionalContactRef").Count > 0)
                {
                    toUpdate.AdditionalContactRef = new List<AdditionalContact>();
                    XmlNodeList nodes = node.SelectNodes("AdditionalContactRef");
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        AdditionalContact contact = new AdditionalContact();
                        contact.ContactName = nodes[i].FirstChild.InnerText;
                        contact.ContactValue = nodes[i].LastChild.InnerText;

                        toUpdate.AdditionalContactRef.Add(contact);
                    }

                }
                if (node.SelectNodes("ContactsRet").Count > 0)
                {
                    toUpdate.Contacts = new List<Contact>();


                    XmlNodeList nodes = node.SelectNodes("ContactsRet");
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        Contact contact = new Contact();
                        if (nodes[i]["ListID"] != null)
                        {
                            contact.ListID = nodes[i]["ListID"].InnerText;
                        }
                        if (nodes[i]["FirstName"] != null)
                        {
                            contact.FirstName = nodes[i]["FirstName"].InnerText;
                        }
                        if (nodes[i]["LastName"] != null)
                        {
                            contact.LastName = nodes[i]["LastName"].InnerText;
                        }
                        if (nodes[i]["MiddleName"] != null)
                        {
                            contact.MiddleName = nodes[i]["MiddleName"].InnerText;
                        }
                        if (nodes[i]["Salutation"] != null)
                        {
                            contact.Salutation = nodes[i]["Salutation"].InnerText;
                        }
                        if (nodes[i]["JobTitle"] != null)
                        {
                            contact.JobTitle = nodes[i]["JobTitle"].InnerText;
                        }

                        if (nodes[i].SelectNodes("AdditionalContactRef").Count > 0)
                        {

                            contact.AdicionalContacRef = new List<AdditionalContact>();
                            XmlNodeList nodes2 = nodes[i].SelectNodes("AdditionalContactRef");
                            for (int j = 0; j < nodes2.Count; j++)
                            {
                                AdditionalContact contactRef = new AdditionalContact();
                                contactRef.ContactName = nodes2[j].FirstChild.InnerText;
                                contactRef.ContactValue = nodes2[j].LastChild.InnerText;

                                contact.AdicionalContacRef.Add(contactRef);
                            }

                        }

                        toUpdate.Contacts.Add(contact);
                    }

                }
                if (node.SelectNodes("CustomerTypeRef").Count > 0)
                {
                    toUpdate.CustomerTypeRef = new CustomerType();
                    if (node["CustomerTypeRef"]["ListID"] != null)
                    {
                        toUpdate.CustomerTypeRef.ListID = node["CustomerTypeRef"]["ListID"].InnerText;
                    }
                    if (node["VendorTypeRef"]["FullName"] != null)
                    {
                        toUpdate.CustomerTypeRef.FullName = node["CustomerTypeRef"]["FullName"].InnerText;
                    }

                }
                if (node.SelectNodes("TermsRef").Count > 0)
                {
                    toUpdate.TermsRef = new Terms();
                    if (node["TermsRef"]["ListID"] != null)
                    {
                        toUpdate.TermsRef.ListID = node["TermsRef"]["ListID"].InnerText;
                    }
                    if (node["TermsRef"]["FullName"] != null)
                    {
                        toUpdate.TermsRef.FullName = node["TermsRef"]["FullName"].InnerText;
                    }
                }
                if (node.SelectNodes("SalesRepRef").Count > 0)
                {
                    toUpdate.SalesRepRef = new SalesRep();
                    if (node["SalesRepRef"]["ListID"] != null)
                    {
                        toUpdate.SalesRepRef.ListID = node["SalesRepRef"]["ListID"].InnerText;
                    }
                    if (node["SalesRepRef"]["FullName"] != null)
                    {
                        toUpdate.SalesRepRef.FullName = node["SalesRepRef"]["FullName"].InnerText;
                    }
                }
                System.Globalization.CultureInfo myInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
                if (node["Balance"] != null)
                {
                    toUpdate.Balance = Double.Parse(node["Balance"].InnerText, myInfo); 

                }
                if (node["TotalBalance"] != null)
                {
                    toUpdate.TotalBalance = Double.Parse(node["TotalBalance"].InnerText, myInfo); 

                }



                if (node["SalesTaxCodeRef"] != null)
                {
                    toUpdate.SalesTaxCodeRef = new SalesTaxCode();
                    if (node["SalesTaxCodeRef"]["ListID"] != null)
                    {
                        toUpdate.SalesTaxCodeRef.ListID = node["SalesTaxCodeRef"]["ListID"].InnerText;
                    }
                    if (node["SalesTaxCodeRef"]["FullName"] != null)
                    {
                        toUpdate.SalesTaxCodeRef.FullName = node["SalesTaxCodeRef"]["FullName"].InnerText;
                    }

                }




                if (node["ItemSalesTaxRef"] != null)
                {
                    toUpdate.ItemSalesTaxRef = new ItemSalesTax();
                    if (node["ItemSalesTaxRef"]["ListID"] != null)
                    {
                        toUpdate.SalesTaxCodeRef.ListID = node["ItemSalesTaxRef"]["ListID"].InnerText;
                    }
                    if (node["ItemSalesTaxRef"]["FullName"] != null)
                    {
                        toUpdate.SalesTaxCodeRef.FullName = node["ItemSalesTaxRef"]["FullName"].InnerText;
                    }

                }


                if (node["ResaleNumber"] != null)
                {
                    toUpdate.ResaleNumber = node["ResaleNumber"].InnerText;
                }


                if (node["OpenBalanceDate"] != null)
                {
                    toUpdate.OpenBalanceDate = Convert.ToDateTime(node["OpenBalanceDate"].InnerText);
                }

                if (node["AccountNumber"] != null)
                {
                    toUpdate.AccountNumber = node["AccountNumber"].InnerText;
                }

                if (node["CreditLimit"] != null)
                {
                    toUpdate.CreditLimit = Double.Parse(node["CreditLimit"].InnerText, myInfo); 
                }


                if (node["PreferredPaymentMethodRef"] != null)
                {
                    toUpdate.PreferredPaymentMethodRef = new PreferredPaymentMethod();
                    if (node["PreferredPaymentMethodRef"]["ListID"] != null)
                    {
                        toUpdate.PreferredPaymentMethodRef.ListID = node["PreferredPaymentMethodRef"]["ListID"].InnerText;
                    }
                    if (node["PreferredPaymentMethodRef"]["FullName"] != null)
                    {
                        toUpdate.PreferredPaymentMethodRef.FullName = node["PreferredPaymentMethodRef"]["FullName"].InnerText;
                    }
                }

                if (node["CreditCardInfo"] != null)
                {
                    toUpdate.CreditCardInfo = new CreditCard();
                    if (node["CreditCardInfo"]["CreditCardNumber"] != null)
                    {
                        toUpdate.CreditCardInfo.CreditCardNumber = node["CreditCardInfo"]["CreditCardNumber"].InnerText;
                    }
                     if (node["CreditCardInfo"]["ExpirationMonth"] != null)
                    {
                        toUpdate.CreditCardInfo.ExpirationMonth =Convert.ToInt16(  node["CreditCardInfo"]["ExpirationMonth"].InnerText);
                    }

                      if (node["CreditCardInfo"]["ExpirationYear"] != null)
                    {
                        toUpdate.CreditCardInfo.ExpirationYear =Convert.ToInt16(  node["CreditCardInfo"]["ExpirationYear"].InnerText);
                    }

                     if (node["CreditCardInfo"]["NameOnCard"] != null)
                    {
                        toUpdate.CreditCardInfo.NameOnCard =  node["CreditCardInfo"]["NameOnCard"].InnerText;
                    }

                     if (node["CreditCardInfo"]["CreditCardAddress"] != null)
                    {
                        toUpdate.CreditCardInfo.CreditCardAddress =  node["CreditCardInfo"]["CreditCardAddress"].InnerText;
                    }

                    if (node["CreditCardInfo"]["CreditCardPostalCode"] != null)
                    {
                        toUpdate.CreditCardInfo.CreditCardPostalCode =  node["CreditCardInfo"]["CreditCardPostalCode"].InnerText;
                    }


                }
                if (node["JobStatus"] != null)
                {
                    toUpdate.JobStatus = node["JobStatus"].InnerText;
                 }

                if (node["JobStartDate"] != null)
                {
                    toUpdate.JobStartDate = Convert.ToDateTime( node["JobStartDate"].InnerText);
                 }

                if (node["JobProjectedEndDate"] != null)
                {
                    toUpdate.JobProjectedEndDate = Convert.ToDateTime(node["JobProjectedEndDate"].InnerText);
                }

                if (node["JobEndDate"] != null)
                {
                    toUpdate.JobEndDate = Convert.ToDateTime(node["JobEndDate"].InnerText);
                }
                if (node["JobDesc"] != null)
                {
                    toUpdate.JobDesc = node["JobDesc"].InnerText;
                }

                if (node["JobDesc"] != null)
                {
                    toUpdate.JobDesc = node["JobDesc"].InnerText;
                }



                if (node["JobTypeRef"] != null)
                {
                    toUpdate.JobTypeRef = new JobType();
                    if (node["JobTypeRef"]["ListID"] != null)
                    {
                        toUpdate.JobTypeRef.ListID = node["JobTypeRef"]["ListID"].InnerText;
                    }
                    if (node["JobTypeRef"]["FullName"] != null)
                    {
                        toUpdate.JobTypeRef.FullName = node["JobTypeRef"]["FullName"].InnerText;
                    }

                }



                if (node["Notes"] != null)
                {
                    toUpdate.Notes = node["Notes"].InnerText;
                }


//                <AdditionalNotesRet> <!-- optional, may repeat -->
//<NoteID >INTTYPE</NoteID> <!-- required -->
//<Date >DATETYPE</Date> <!-- required -->
//<Note >STRTYPE</Note> <!-- required -->
//</AdditionalNotesRet>

                if (node["PreferredDeliveryMethod"] != null)
                {
                    toUpdate.PreferredDeliveryMethod = node["PreferredDeliveryMethod"].InnerText;
                }

                if (node["PriceLevelRef"] != null)
                {
                    toUpdate.PriceLevelRef = new PriceLevel();
                    if (node["PriceLevelRef"]["ListID"] != null)
                    {
                        toUpdate.PriceLevelRef.ListID = node["PriceLevelRef"]["ListID"].InnerText;
                    }
                    if (node["PriceLevelRef"]["FullName"] != null)
                    {
                        toUpdate.PriceLevelRef.FullName = node["PriceLevelRef"]["FullName"].InnerText;
                    }

                }

                if (node["CurrencyRef"] != null)
                {
                    toUpdate.CurrencyRef = new Currency();
                    if (node["CurrencyRef"]["ListID"] != null)
                    {
                        toUpdate.CurrencyRef.ListID = node["CurrencyRef"]["ListID"].InnerText;
                    }
                    if (node["CurrencyRef"]["FullName"] != null)
                    {
                        toUpdate.CurrencyRef.FullName = node["CurrencyRef"]["FullName"].InnerText;
                    }

                }
                 if (node["EditSequence"] != null)
                {
                    toUpdate.EditSequence = node["EditSequence"].InnerText;
                }
               

                                
                
                
                if (node["ExternalGUID"] != null)
                {
                    toUpdate.ExternalGUID = node["ExternalGUID"].InnerText;

                }


                XmlNodeList extras = node.SelectNodes("DataExtRet");
                foreach (XmlNode ex in extras)
                {
                    var name = ex["DataExtName"].InnerText;
                    var value = ex["DataExtValue"].InnerText;
                    toUpdate.AddDataEx(name, value);

                }

                quickbookListCustomer.Add(toUpdate);
            }

            return quickbookListCustomer;

        }

    }



    public class CreditCard
    {
        public string CreditCardNumber;
        public int ExpirationMonth = -1;
        public int ExpirationYear = -1;
        public string NameOnCard;
        public string CreditCardAddress;
        public string CreditCardPostalCode;

        public string toXml()
        {
            if (CreditCardNumber == "" && ExpirationMonth < 0 && ExpirationYear < 0 &&
                NameOnCard == "" && CreditCardAddress == "" && CreditCardPostalCode == "")
                return "";

            return "" +
                "<CreditCardInfo>" +
                    "<CreditCardNumber >" + CreditCardNumber + "</CreditCardNumber>" +
                    "<ExpirationMonth >" + ExpirationMonth + "</ExpirationMonth>" +
                    "<ExpirationYear >" + ExpirationYear + "</ExpirationYear>" +
                    "<NameOnCard >" + NameOnCard + "</NameOnCard>" +
                    "<CreditCardAddress >" + CreditCardAddress + "</CreditCardAddress>" +
                    "<CreditCardPostalCode >" + CreditCardPostalCode + "</CreditCardPostalCode>" +
                "</CreditCardInfo>";

        }

    }
}
