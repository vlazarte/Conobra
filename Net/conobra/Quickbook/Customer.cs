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

        public DateTime TimeCreated { get; set; }
        public DateTime Modified { get; set; }
        public string EditSequence { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool? IsActive { get; set; }// optional   0 = false, 1 = true
        public Class ClassRef { get; set; }// optional
        public Customer ParentRef { get; set; } // optional
        public int Sublevel { get; set; }
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
        public decimal? Balance { get; set; }
        public decimal? TotalBalance { get; set; }
        public float OpenBalance { get; set; }  // optional
        public DateTime OpenBalanceDate { get; set; }  // optional
        public SalesTaxCode SalesTaxCodeRef { get; set; }  // optional
        public ItemSalesTax ItemSalesTaxRef { get; set; }  // optional
        public string ResaleNumber { get; set; }
        public string AccountNumber { get; set; }  // optional
        public float CreditLimit { get; set; }  // optional
        public PreferredPaymentMethod PreferredPaymentMethodRef { get; set; }  // optional
        public CreditCard CreditCardInfo { get; set; }  // optional

        //<!-- JobStatus may have one of the following values: Awarded, Closed, InProgress, None [DEFAULT], NotAwarded, Pending -->
        public string JobStatus { get; set; }   // optional
        public DateTime? JobStartDate { get; set; } // optional
        public DateTime JobProjectedEndDate { get; set; }  // optional
        public DateTime JobEndDate { get; set; }  // optional
        public string JobDesc { get; set; } // optional
        public JobType JobTypeRef { get; set; }   // optional
        public string Notes { get; set; } // optional
        public List<string> AdditionalNotes { get; set; }  // optional

        /*
         * <AdditionalNotes> <!-- optional, may repeat -->
<Note >STRTYPE</Note> <!-- required -->
</AdditionalNotes>
         */
        //<!-- PreferredDeliveryMethod may have one of the following values: None [Default], Email, Fax -->
        public string PreferredDeliveryMethod = string.Empty; // optional
        public PriceLevel PriceLevelRef;
        public string ExternalGUID = string.Empty; // optional
        public Currency CurrencyRef { get; set; }  // optional

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
                    IsActive = ("" + node["IsActive"].InnerText) == "true" ? true : false;

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

                if (node["Balance"] != null)
                    Balance = Functions.ParseDecimal("" + node["Balance"].InnerText);

                if (node["TotalBalance"] != null)
                    TotalBalance = Functions.ParseDecimal("" + node["TotalBalance"].InnerText);

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
                xml += "<IsActive >" + (IsActive == true ? "true" : "false") + "</IsActive>";
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
                    xml += AdditionalContactRef[i].toXML();
                }
            }

            if (Contacts.Count > 0)
            {
                for (int i = 0; i < Contacts.Count; i++)
                {
                    xml += Contacts[i].toXml();
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
            if (ListID == "")
                return "";
            return "<CustomerRef>" +
            "<ListID >" + ListID + "</ListID>" +
            "</CustomerRef>";
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
        public override List<Abstract> GetRecords(ref string err)
        {
            err = "No implemented yet Customer";
            return new List<Abstract>();
        }

    }

    public class AdditionalContact
    {
        public string ContactName; // Required
        public string ContactValue; // required

        public string toXML()
        {
            if (ContactName != "" && ContactValue != "")
            {

                return "<AdditionalContactRef>" +
                    "<ContactName >" + ContactName + "</ContactName>" +
                    "<ContactValue >" + ContactValue + "</ContactValue>" +
                    "</AdditionalContactRef>";
            }

            return "";
        }

    }

    public class Contact
    {
        public string Salutation; // optional
        public string FirstName; // required
        public string MiddleName; // optional
        public string LastName; // optional
        public string JobTitle; // òptional
        public List<AdditionalContact> AdditionalContactRef; // 0 - 5 

        public Contact()
        {
            AdditionalContactRef = new List<AdditionalContact>();
        }

        public void AddAdditionalContact(AdditionalContact AC)
        {
            if (AdditionalContactRef.Count >= 5)
                return;
            AdditionalContactRef.Add(AC);
        }

        public string toXml()
        {
            if (FirstName == "")
                return "";

            string add = "";
            for (int i = 0; i < AdditionalContactRef.Count; i++)
            {
                add += AdditionalContactRef[i].toXML();
            }

            return "" +
                "<Contacts>" +
                    "<Salutation >" + Salutation + "</Salutation>" +
                    "<FirstName >" + FirstName + "</FirstName>" +
                    "<MiddleName >" + MiddleName + "</MiddleName>" +
                    "<LastName >" + LastName + "</LastName>" +
                    "<JobTitle >" + JobTitle + "</JobTitle>" +
                    add +
                "</Contacts>";
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
