using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class Address : Abstract
    {
        public static int Bill_Address = 0;
        public static int Ship_Address = 1;
        public static int Ship_To_Address = 2;
        public static int Bill_BillAddressBlock = 3;
        public static int Bill_ShipAddressBlock = 4;

        private int AddressType = 0;

        public List<string> AddressLine;

        public string Addr1{get;set;}
        public string Addr2 { get; set; }
        public string Addr3 { get; set; }
        public string Addr4 { get; set; }
        public string Addr5 { get; set; }

        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Note { get; set; }
        public bool IsDefault = false;



        public Address(int AddresType)
        {

            Addr1 = string.Empty;
            Addr2 = string.Empty;
            Addr3 = string.Empty;
            Addr4 = string.Empty;
            Addr5 = string.Empty;
            this.AddressType = AddresType;
            AddressLine = new List<string>();
            ObjectName = "Address";
        }
        public Address()
        {

            Addr1 = string.Empty;
            Addr2 = string.Empty;
            Addr3 = string.Empty;
            Addr4 = string.Empty;
            Addr5 = string.Empty;
            this.AddressType = Bill_Address;
            AddressLine = new List<string>();
            ObjectName = "Address";
        }

        public void isDefaultShipToAddress(bool isDefault)
        {
            IsDefault = isDefault;
        }

        public string getAddressLine(int indice)
        {
            if (AddressLine.Count == 0)
                return "";
            return AddressLine[indice - 1];
        }

        public void addAddrLine(string line)
        {
            if (AddressLine.Count >= 5)
                return;
            AddressLine.Add(line);
        }

        public string formatString()
        {
            if (AddressType == Bill_Address)
            {
                string dir = "";
                for (int i = 0; i < AddressLine.Count; i++)
                {
                    string v = AddressLine[i];
                    if (v == "")
                        continue;

                    dir += v + Environment.NewLine;
                }
                /*if (City != "" || State != "" || PostalCode != "")
                {
                    bool soloPostal = false;
                    if (City != "" && State != "")
                    {
                        dir += City + ", " + State;
                    }
                    else if (City != "")
                    {
                        dir += City;
                    }
                    else if (State != "")
                    {
                        dir += State;
                    }
                    else if (PostalCode != "")
                    {
                        soloPostal = true;
                        dir += PostalCode + Environment.NewLine;
                    }

                    if (!soloPostal && PostalCode != "")
                    {
                        dir += " " + PostalCode + Environment.NewLine;
                    }
                }
                if (Country != "")
                {
                    dir += Country + Environment.NewLine;
                }
                if (Note != "")
                {
                    dir += Note + Environment.NewLine;
                }
                */
                return dir;
            }
            return "";
        }

        public string toXml()
        {
            string xml = "";

            if (Name != "")
                xml += "<Name>" + Name + "</Name>";

            string[] tags = { "Addr1", "Addr2", "Addr3", "Addr4", "Addr5" };

            int c = 0;
            for (int i = 0; i < AddressLine.Count; i++)
            {
                string v = AddressLine[i];
                if (v == "")
                    continue;

                xml += "<" + tags[c] + ">" + v + "</" + tags[c] + ">";
                c++;
            }
            if (City != "")
                xml += "<City >" + City + "</City>";
            if (State != "")
                xml += "<State >" + State + "</State>";
            if (PostalCode != "")
                xml += "<PostalCode >" + PostalCode + "</PostalCode>";
            if (Country != "")
                xml += "<Country >" + Country + "</Country>";
            if (Note != "")
                xml += "<Note >" + Note + "</Note>";

            if (AddressType == Ship_To_Address && IsDefault == true)
            {
                xml += "<DefaultShipTo >true</NoDefaultShipTote>";
            }

            if (AddressType == Bill_Address)
            {
                xml = "<BillAddress>" + xml + "</BillAddress>";
            }
            else if (AddressType == Ship_Address)
            {
                xml = "<ShipAddress>" + xml + "</ShipAddress>";
            }
            else if (AddressType == Ship_To_Address)
            {
                xml = "<ShipToAddress>" + xml + "</ShipToAddress>";
            }

            return xml;
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet Address";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Address";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Address";
            return new List<Abstract>();
        }

    }
}
