using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;

namespace Quickbook
{
    public class Vendor:Abstract
    {
        
        private string name;
        private bool active;
        private string firstName;
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        private string companyName;

        private bool vendorEligible;
        private float balance;
        public bool? IsTaxAgency { get; set; }
        public Class clasRef { get; set; }
        public string Salutation { get; set; }
        public string JobTitle { get; set; }
        public VendorAddress VendorAdress { get; set; }
        public ShipAdress ShipAddress { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Cc { get; set; }
        public string Contact { get; set; }
        public string AltContact { get; set; }
        public AdditionalContactRef AdditionalContactRef { get; set; }
        public ContactsRet Contacts { get; set; }
        public string NameOnCheck { get; set; }
        public string AccountNumber { get; set; }
        public string Notes { get; set; }
        //public AdditionalNotes notes{get;set;}
        public VendorTypeRef VendorType { get; set; }
        public TermsRef TermsRef { get; set; }

        public double CreditLimit { get; set; }
        public string VendorTaxIdent { get; set; }

        public double OpenBalance { get; set; }
        public DateTime? OpenBalanceDate { get; set; }
        public BillingRateRef BillingRateRef { get; set; }
        public string ExternalGUID { get; set; }
        public SalesTaxCodeRef SalesTaxCodeRef { get; set; }

        public string SalesTaxCountry { get; set; }
        public bool? IsSalesTaxAgency { get; set; }

        public string TaxRegistrationNumber { get; set; }

        public string ReportingPeriod { get; set; }
        public bool? IsTaxTrackedOnPurchases { get; set; }
        public TaxOnPurchasesAccountRef TaxOnPurchasesAccountRef { get; set; }

        public bool? IsTaxTrackedOnSales { get; set; }

        public TaxOnSalesAccountRef TaxOnSalesAccountRef { get; set; }
        public SalesTaxReturnRef SalesTaxReturnRef { get; set; }
        public bool IsTaxOnTax { get; set; }
        public PrefillAccountRef PrefillAccountRef { get; set; }

        public Currency CurrencyRef { get; set; }
       


       

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string FullName
        {
            get { return name; }
            set { name = value; }
        }


        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public bool IsVendorEligibleFor1099
        {
            get { return vendorEligible; }
            set { vendorEligible = value; }
        }

        public float Balance
        {
            get { return balance; }
            set { balance = value; }
        }


        public Vendor()
        {


            firstName = string.Empty;
            MiddleName = string.Empty;
            LastName = string.Empty;
            companyName = string.Empty;

            IsTaxAgency = null;
            clasRef = null;
            Salutation = string.Empty;
            JobTitle = string.Empty;
            VendorAdress = null;
            ShipAddress = null;
            Phone = string.Empty;
            AltPhone = string.Empty;
            Fax = string.Empty;
            Email = string.Empty;
            Cc = string.Empty;
            Contact = string.Empty;
            AltContact = string.Empty;
            AdditionalContactRef = null;
            Contacts = null;
            NameOnCheck = string.Empty;
            AccountNumber = string.Empty;
            Notes = string.Empty;

            VendorType = null;
            TermsRef = null;


            VendorTaxIdent = string.Empty;

            BillingRateRef = null;
            ExternalGUID = string.Empty;
            SalesTaxCodeRef = null;

            SalesTaxCountry = string.Empty;
            IsSalesTaxAgency = null;

            TaxRegistrationNumber = string.Empty;

            ReportingPeriod = string.Empty;
            IsTaxTrackedOnPurchases = null;
            TaxOnPurchasesAccountRef = null;

            IsTaxTrackedOnSales = null;

            TaxOnSalesAccountRef = null;
            SalesTaxReturnRef = null;

            PrefillAccountRef = null;

            CurrencyRef = null;
            

            ObjectName = "Vendor";
        }

        public static List<Vendor> getVendors()
        {
            List<Vendor> list = new List<Vendor>();
            //Hashtable list = new Hashtable();

            string xml = "";
            xml += "<?xml version=\"1.0\"?>";
            xml += "<?qbxml version=\"4.0\"?>";
            xml += "<QBXML>";
            xml += "<QBXMLMsgsRq onError=\"stopOnError\">";
            xml += "<VendorQueryRq requestID=\"12\">";
            xml += "<IncludeRetElement>ListID</IncludeRetElement>";
            xml += "<IncludeRetElement>Name</IncludeRetElement>";
            xml += "<IncludeRetElement>FirstName</IncludeRetElement>";
            xml += "<IncludeRetElement>CompanyName</IncludeRetElement>";
            xml += "<IncludeRetElement>IsActive</IncludeRetElement>";
            xml += "<IncludeRetElement>IsVendorEligibleFor1099</IncludeRetElement>";
            xml += "<IncludeRetElement>DataExtRet</IncludeRetElement>";
            xml += "<OwnerID>0</OwnerID>";
            xml += "</VendorQueryRq>";
            xml += "</QBXMLMsgsRq>";
            xml += "</QBXML>";

            if (Vars.qb != null && Vars.qb.isOpen())
            {
                //vendors.xml
                var response = "";
                if (Vars.isLocalhost() == true)
                {
                    response = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "/xml-debug/vendors.xml");
                }
                else
                    response = Vars.qb.sendRequest(xml);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                XmlNodeList rets = doc.SelectNodes("/QBXML/QBXMLMsgsRs/VendorQueryRs/VendorRet");
                string message = string.Empty;

                foreach (XmlNode node in rets)
                {
                    Vendor v = new Vendor();
                    v.ListID = node["ListID"].InnerText;
                    v.Name = node["Name"].InnerText;
                    v.IsActive = (bool)(node["IsActive"].InnerText == "true");
                    if (node.SelectNodes("FirstName").Count > 0)
                    {
                        v.FirstName = node["FirstName"].InnerText;
                    }

                    if (node.SelectNodes("CompanyName").Count > 0)
                    {
                        v.CompanyName = node["CompanyName"].InnerText;
                    }

                    v.IsVendorEligibleFor1099 = (bool)(node["IsVendorEligibleFor1099"].InnerText == "true");
                    //v.Balance = float.Parse(node["Balance"].InnerText);

                    XmlNodeList extras = node.SelectNodes("DataExtRet");
                    foreach (XmlNode ex in extras)
                    {
                        var name = ex["DataExtName"].InnerText;
                        var value = ex["DataExtValue"].InnerText;
                        v.AddDataEx(name, value);
                    }

                    list.Add(v);

                }
            }

            return list;
        }

        public string toXmlQuery(bool includeFilter)
        {

            XmlElement ele = (new XmlDocument()).CreateElement("test");

            StringBuilder toXML = new StringBuilder();

            toXML.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            toXML.Append("<?qbxml version=\"13.0\"?>");
            toXML.Append("<QBXML>");
            toXML.Append("<QBXMLMsgsRq onError=\"stopOnError\">");
            toXML.Append("<VendorQueryRq >");

            if (includeFilter)
            {
                toXML.Append(" <FromModifiedDate >" + DateTime.Now.ToString("yyy-MM-dd") + "T00:00:00-04:00</FromModifiedDate>");
                toXML.Append(" <ToModifiedDate >" + DateTime.Now.ToString("yyy-MM-dd") + "T23:59:59-04:00</ToModifiedDate> ");

            }

            toXML.Append("<IncludeRetElement>ListID</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Name</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>IsActive</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ClassRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>IsTaxAgency</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>CompanyName</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Salutation</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>FirstName</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>MiddleName</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>LastName</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>JobTitle</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>VendorAddress</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ShipAddress</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Phone</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Fax</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Email</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Cc</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Contact</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>AltContact</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>AdditionalContactRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ContactsRet</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>NameOnCheck</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>AccountNumber</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Notes</IncludeRetElement>");
            //<AdditionalNotesRet> <!-- optional, may repeat -->
            //                            <NoteID >INTTYPE</NoteID> <!-- required -->
            //                            <Date >DATETYPE</Date> <!-- required -->
            //                            <Note >STRTYPE</Note> <!-- required -->
            //                    </AdditionalNotesRet>

            toXML.Append("<IncludeRetElement>VendorTypeRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>TermsRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>CreditLimit</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>VendorTaxIdent</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>IsVendorEligibleFor1099</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>Balance</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>OpenBalanceDate</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>BillingRateRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ExternalGUID</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>SalesTaxCodeRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>SalesTaxCountry</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>IsSalesTaxAgency</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>TaxRegistrationNumber</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>ReportingPeriod</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>IsTaxTrackedOnPurchases</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>TaxOnPurchasesAccountRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>IsTaxTrackedOnSales</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>TaxOnSalesAccountRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>IsTaxOnTax</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>PrefillAccountRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>CurrencyRef</IncludeRetElement>");
            toXML.Append("<IncludeRetElement>DataExtRet</IncludeRetElement>");
            toXML.Append("<OwnerID>0</OwnerID>");
            toXML.Append("</VendorQueryRq>");
            toXML.Append("</QBXMLMsgsRq>");
            toXML.Append("</QBXML>");

            return toXML.ToString();
        }

        public string toXMLVendorRef() { 
        
            StringBuilder xml = new StringBuilder();
            XmlElement ele = (new XmlDocument()).CreateElement("test");
            xml.Append("<VendorRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {
                ele.InnerText = FullName + "";
                xml.Append("<FullName>" + ele.InnerXml + "</FullName>"); //-- required -->
            }

            xml.Append("</VendorRef>");

            return xml.ToString();

           
        }

        public List<Abstract> GetRecordsCVS(ref string err)
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

                code = res["QBXML"]["QBXMLMsgsRs"]["VendorQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["VendorQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {

                    List<Abstract> quickbookListVendor = new List<Abstract>();
                    XmlNodeList rets = res.SelectNodes("/QBXML/QBXMLMsgsRs/VendorQueryRs/VendorRet");
                    quickbookListVendor = GetVendors(rets);

                    if (quickbookListVendor.Count > 0)
                    {
                        return quickbookListVendor;
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

            return new List<Abstract>();
        }


        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet Vendor";
            return false;

        }
        public override List<Abstract> GetRecords(ref string err)
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

                code = res["QBXML"]["QBXMLMsgsRs"]["VendorQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["VendorQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {

                    List<Abstract> quickbookListVendors = new List<Abstract>();

                    XmlNodeList fieldsList = (((System.Xml.XmlNode)(res))).LastChild.FirstChild.FirstChild.ChildNodes;

                    XmlNodeList rets = res.SelectNodes("/QBXML/QBXMLMsgsRs/VendorQueryRs/VendorRet");

                    quickbookListVendors = GetVendors(rets);

                    if (quickbookListVendors.Count > 0)
                    {
                        return quickbookListVendors;
                    }
                    else
                    {
                        err = "No se obtuvo ningun registro";
                    }


                    string message = string.Empty;

                    foreach (XmlNode node in rets)
                    {
                        Vendor v = new Vendor();
                        v.ListID = node["ListID"].InnerText;
                        v.Name = node["Name"].InnerText;

                        if (node.SelectNodes("CompanyName").Count > 0)
                        {
                            v.CompanyName = node["CompanyName"].InnerText;
                        }
                        if (node.SelectNodes("AdditionalContactRef").Count > 0)
                        {
                            v.Phone = "";
                            v.Email = "";
                        }

                        XmlNodeList extras = node.SelectNodes("DataExtRet");
                        foreach (XmlNode ex in extras)
                        {
                            var name = ex["DataExtName"].InnerText;
                            var value = ex["DataExtValue"].InnerText;
                            v.AddDataEx(name, value);
                        }

                        quickbookListVendors.Add(v);

                    }

                    if (quickbookListVendors.Count > 0)
                    {
                        return quickbookListVendors;
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

            return new List<Abstract>();
        }

        private List<Abstract> GetVendors(XmlNodeList rets)
        {


            List<Abstract> quickbookListVendor = new List<Abstract>();

            string message = string.Empty;

            foreach (XmlNode node in rets)
            {
                Vendor toUpdate = new Vendor();
                if (node["ListID"] != null)
                {
                    toUpdate.ListID = node["ListID"].InnerText;
                }
                if (node["Name"] != null)
                {
                    toUpdate.Name = node["Name"].InnerText;
                }
                if (node["IsActive"] != null)
                {
                    toUpdate.IsActive = node["IsActive"].InnerText == "true" ? true : false;
                }

                if (node.SelectNodes("ClassRef").Count > 0)
                {
                    toUpdate.clasRef = new Class();

                    toUpdate.clasRef.ListID = node["ClassRef"].FirstChild.InnerText;
                    toUpdate.clasRef.FullName = node["ClassRef"].LastChild.InnerText;

                }
                if (node["IsTaxAgency"] != null)
                {
                    toUpdate.IsTaxAgency = node["IsTaxAgency"].InnerText == "true" ? true : false;
                }
                if (node["CompanyName"] != null)
                {
                    toUpdate.companyName = node["CompanyName"].InnerText;
                }
                if (node["Salutation"] != null)
                {
                    toUpdate.Salutation = node["Salutation"].InnerText;
                }
                if (node["FirstName"] != null)
                {
                    toUpdate.firstName = node["FirstName"].InnerText;
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


                if (node.SelectNodes("VendorAddress").Count > 0)
                {
                    toUpdate.VendorAdress = new VendorAddress();
                    if (node["VendorAddress"]["Addr1"] != null)
                    {
                        toUpdate.VendorAdress.Addr1 = node["VendorAddress"]["Addr1"].InnerText;
                    }
                    if (node["VendorAddress"]["Addr2"] != null)
                    {
                        toUpdate.VendorAdress.Addr2 = node["VendorAddress"]["Addr2"].InnerText;
                    }
                    if (node["VendorAddress"]["Addr3"] != null)
                    {
                        toUpdate.VendorAdress.Addr3 = node["VendorAddress"]["Addr3"].InnerText;
                    }
                    if (node["VendorAddress"]["Addr4"] != null)
                    {
                        toUpdate.VendorAdress.Addr4 = node["VendorAddress"]["Addr4"].InnerText;
                    }
                    if (node["VendorAddress"]["Addr5"] != null)
                    {
                        toUpdate.VendorAdress.Addr5 = node["VendorAddress"]["Addr5"].InnerText;
                    }
                    if (node["VendorAddress"]["City"] != null)
                    {
                        toUpdate.VendorAdress.City = node["VendorAddress"]["City"].InnerText;
                    }
                    if (node["VendorAddress"]["Country"] != null)
                    {
                        toUpdate.VendorAdress.Country = node["VendorAddress"]["Country"].InnerText;
                    }
                    if (node["VendorAddress"]["Note"] != null)
                    {
                        toUpdate.VendorAdress.Note = node["VendorAddress"]["Note"].InnerText;
                    }
                    if (node["VendorAddress"]["PostalCode"] != null)
                    {
                        toUpdate.VendorAdress.PostalCode = node["VendorAddress"]["PostalCode"].InnerText;
                    }
                    if (node["VendorAddress"]["State"] != null)
                    {
                        toUpdate.VendorAdress.State = node["VendorAddress"]["State"].InnerText;
                    }
                }
                if (node.SelectNodes("ShipAddress").Count > 0)
                {
                    toUpdate.ShipAddress = new ShipAdress();
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
                if (node["Phone"] != null)
                {
                    toUpdate.Phone = node["Phone"].InnerText;
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
                    toUpdate.AdditionalContactRef = new AdditionalContactRef();
                    if (node["AdditionalContactRef"]["ContactName"] != null)
                    {
                        toUpdate.AdditionalContactRef.ContactName = node["AdditionalContactRef"]["ContactName"].InnerText;
                    }
                    if (node["AdditionalContactRef"]["ContactValue"] != null)
                    {
                        toUpdate.AdditionalContactRef.ContactValue = node["AdditionalContactRef"]["ContactValue"].InnerText;
                    }

                }

                if (node.SelectNodes("ContactsRet").Count > 0)
                {
                    toUpdate.Contacts = new ContactsRet();
                    if (node["ContactsRet"]["FirstName"] != null)
                    {
                        toUpdate.Contacts.FirstName = node["ContactsRet"]["FirstName"].InnerText;
                    }
                    if (node["ContactsRet"]["LastName"] != null)
                    {
                        toUpdate.Contacts.LastName = node["ContactsRet"]["LastName"].InnerText;
                    }
                    if (node["ContactsRet"]["MiddleName"] != null)
                    {
                        toUpdate.Contacts.MiddleName = node["ContactsRet"]["MiddleName"].InnerText;
                    }
                    if (node["ContactsRet"]["Salutation"] != null)
                    {
                        toUpdate.Contacts.Salutation = node["ContactsRet"]["Salutation"].InnerText;
                    }
                    if (node["ContactsRet"]["JobTitle"] != null)
                    {
                        toUpdate.Contacts.JobTitle = node["ContactsRet"]["JobTitle"].InnerText;
                    }

                }
                if (node["NameOnCheck"] != null)
                {
                    toUpdate.NameOnCheck = node["NameOnCheck"].InnerText;
                }
                if (node["AccountNumber"] != null)
                {
                    toUpdate.AccountNumber = node["AccountNumber"].InnerText;
                }
                if (node["Notes"] != null)
                {
                    toUpdate.Notes = node["Notes"].InnerText;
                }


                //<AdditionalNotesRet> <!-- optional, may repeat -->
                //                            <NoteID >INTTYPE</NoteID> <!-- required -->
                //                            <Date >DATETYPE</Date> <!-- required -->
                //                            <Note >STRTYPE</Note> <!-- required -->
                //                    </AdditionalNotesRet>
                if (node.SelectNodes("VendorTypeRef").Count > 0)
                {
                    toUpdate.VendorType = new VendorTypeRef();
                    if (node["VendorTypeRef"]["ListID"] != null)
                    {
                        toUpdate.VendorType.ListID = node["VendorTypeRef"]["ListID"].InnerText;
                    }
                    if (node["VendorTypeRef"]["FullName"] != null)
                    {
                        toUpdate.VendorType.FullName = node["VendorTypeRef"]["FullName"].InnerText;
                    }


                }
                if (node.SelectNodes("TermsRef").Count > 0)
                {
                    toUpdate.TermsRef = new TermsRef();
                    if (node["TermsRef"]["ListID"] != null)
                    {
                        toUpdate.TermsRef.ListID = node["TermsRef"]["ListID"].InnerText;
                    }
                    if (node["TermsRef"]["FullName"] != null)
                    {
                        toUpdate.TermsRef.FullName = node["TermsRef"]["FullName"].InnerText;
                    }
                }
                if (node["CreditLimit"] != null)
                {
                    toUpdate.CreditLimit = Convert.ToDouble(node["CreditLimit"].InnerText);
                }
                if (node["VendorTaxIdent"] != null)
                {
                    toUpdate.VendorTaxIdent = node["VendorTaxIdent"].InnerText;
                }

                if (node["IsVendorEligibleFor1099"] != null)
                {
                    toUpdate.IsVendorEligibleFor1099 = node["IsVendorEligibleFor1099"].InnerText == "true" ? true : false;
                }
                if (node["Balance"] != null)
                {
                    toUpdate.OpenBalance = Convert.ToDouble(node["Balance"].InnerText);

                }

                if (node["OpenBalanceDate"] != null)
                {
                    toUpdate.OpenBalanceDate = Convert.ToDateTime(node["OpenBalanceDate"].InnerText);
                }


                if (node["BillingRateRef"] != null)
                {
                    toUpdate.BillingRateRef = new BillingRateRef();
                    if (node["BillingRateRef"]["ListID"] != null)
                    {
                        toUpdate.BillingRateRef.ListID = node["BillingRateRef"]["ListID"].InnerText;
                    }
                    if (node["BillingRateRef"]["FullName"] != null)
                    {
                        toUpdate.BillingRateRef.FullName = node["BillingRateRef"]["FullName"].InnerText;
                    }
                }
                if (node["ExternalGUID"] != null)
                {
                    toUpdate.ExternalGUID = node["ExternalGUID"].InnerText;

                }
                if (node["SalesTaxCodeRef"] != null)
                {
                    toUpdate.SalesTaxCodeRef = new SalesTaxCodeRef();
                    if (node["SalesTaxCodeRef"]["ListID"] != null)
                    {
                        toUpdate.SalesTaxCodeRef.ListID = node["SalesTaxCodeRef"]["ListID"].InnerText;
                    }
                    if (node["SalesTaxCodeRef"]["FullName"] != null)
                    {
                        toUpdate.SalesTaxCodeRef.FullName = node["SalesTaxCodeRef"]["FullName"].InnerText;
                    }

                }
                if (node["SalesTaxCountry"] != null)
                {
                    toUpdate.SalesTaxCountry = node["SalesTaxCountry"].InnerText;
                }

                if (node["IsSalesTaxAgency"] != null)
                {
                    toUpdate.IsSalesTaxAgency = node["IsSalesTaxAgency"].InnerText == "true" ? true : false;
                }

                if (node["TaxRegistrationNumber"] != null)
                {
                    toUpdate.TaxRegistrationNumber = node["TaxRegistrationNumber"].InnerText;
                }
                if (node["ReportingPeriod"] != null)
                {
                    toUpdate.ReportingPeriod = node["ReportingPeriod"].InnerText;
                }

                if (node["IsTaxTrackedOnPurchases"] != null)
                {
                    toUpdate.IsTaxTrackedOnPurchases = node["IsTaxTrackedOnPurchases"].InnerText == "true" ? true : false;
                }
                if (node["TaxOnPurchasesAccountRef"] != null)
                {
                    toUpdate.TaxOnPurchasesAccountRef = new TaxOnPurchasesAccountRef();

                    if (node["TaxOnPurchasesAccountRef"]["ListID"] != null)
                    {
                        toUpdate.TaxOnPurchasesAccountRef.ListID = node["TaxOnPurchasesAccountRef"]["ListID"].InnerText;
                    }
                    if (node["TaxOnPurchasesAccountRef"]["FullName"] != null)
                    {
                        toUpdate.TaxOnPurchasesAccountRef.FullName = node["TaxOnPurchasesAccountRef"]["FullName"].InnerText;
                    }

                }

                if (node["IsTaxTrackedOnSales"] != null)
                {
                    toUpdate.IsTaxTrackedOnSales = node["IsTaxTrackedOnSales"].InnerText == "true" ? true : false;
                }

                if (node["TaxOnSalesAccountRef"] != null)
                {
                    toUpdate.TaxOnSalesAccountRef = new TaxOnSalesAccountRef();

                    if (node["TaxOnSalesAccountRef"]["ListID"] != null)
                    {
                        toUpdate.TaxOnSalesAccountRef.ListID = node["TaxOnSalesAccountRef"]["ListID"].InnerText;
                    }
                    if (node["TaxOnSalesAccountRef"]["FullName"] != null)
                    {
                        toUpdate.TaxOnSalesAccountRef.FullName = node["TaxOnSalesAccountRef"]["FullName"].InnerText;
                    }

                }
                if (node["IsTaxTrackedOnSales"] != null)
                {
                    toUpdate.IsTaxTrackedOnSales = node["IsTaxTrackedOnSales"].InnerText == "true" ? true : false;
                }
                if (node["IsTaxOnTax"] != null)
                {
                    toUpdate.IsTaxOnTax = node["IsTaxOnTax"].InnerText == "true" ? true : false;
                }
                if (node["PrefillAccountRef"] != null)
                {
                    toUpdate.PrefillAccountRef = new PrefillAccountRef();
                    if (node["PrefillAccountRef"]["ListID"] != null)
                    {
                        toUpdate.PrefillAccountRef.ListID = node["PrefillAccountRef"]["ListID"].InnerText;
                    }
                    if (node["PrefillAccountRef"]["FullName"] != null)
                    {
                        toUpdate.PrefillAccountRef.FullName = node["PrefillAccountRef"]["FullName"].InnerText;
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

                XmlNodeList extras = node.SelectNodes("DataExtRet");
                foreach (XmlNode ex in extras)
                {
                    var name = ex["DataExtName"].InnerText;
                    var value = ex["DataExtValue"].InnerText;
                    toUpdate.AddDataEx(name, value);
                    
                }

                quickbookListVendor.Add(toUpdate);
            }

            return quickbookListVendor;

        }
    }
}
