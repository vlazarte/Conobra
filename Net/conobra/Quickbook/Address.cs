using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class Address
    {
        public static int Bill_Address = 0 ;
        public static int Ship_Address = 1 ;
        public static int Ship_To_Address = 2 ;

        private int AddressType = 0 ;

        public List<string> AddressLine;

        public string Name;
        public string City ;
        public string State ;
        public string PostalCode;
        public string Country;
        public string Note;
        public bool IsDefault = false;

        public Address( int AddresType )
        {
            this.AddressType = AddresType;
            AddressLine = new List<string>();
        }

        public void isDefaultShipToAddress(bool isDefault)
        {
            IsDefault = isDefault;
        }

        public string getAddressLine(int indice)
        {
            if (AddressLine.Count == 0)
                return "";
            return AddressLine[indice-1];
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
  
    }
}
