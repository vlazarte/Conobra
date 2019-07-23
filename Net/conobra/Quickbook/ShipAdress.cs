using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public class ShipAdress
    {
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Addr3 { get; set; }
        public string Addr4 { get; set; }
        public string Addr5 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Note { get; set; }

        public string toXmlRef()
        {
            StringBuilder xml = new StringBuilder();            
            xml.Append("<ShipAddress>");
            if (Addr1 != string.Empty)
            {                 
                 string value = Functions.htmlEntity(Addr1);
                 xml.Append("<Addr1 >" + value + "</Addr1>");
            }
            if (Addr2 != string.Empty)
            {                
                string value = Functions.htmlEntity(Addr2);
                xml.Append("<Addr2>" + value + "</Addr2>"); 
            }

             if (Addr3 != string.Empty)
            {                
                string value = Functions.htmlEntity(Addr3);
                xml.Append("<Addr3>" + value + "</Addr3>"); 
            }


             if (Addr4 != string.Empty)
            {
                string value = Functions.htmlEntity(Addr4);
                xml.Append("<Addr4>" + value + "</Addr4>"); 
            }

                if (Addr5 != string.Empty)
            {                
                string value = Functions.htmlEntity(Addr5);
                xml.Append("<Addr5>" + value + "</Addr5>"); 
            }

                if (City != string.Empty)
            {                
                string value = Functions.htmlEntity(City);
                xml.Append("<City>" + value + "</City>"); 
            }

             if (State != string.Empty)
            {
                
                string value = Functions.htmlEntity(State);
                xml.Append("<State>" + value + "</State>"); 
            }
             if (PostalCode != string.Empty)
            {
                
                string value = Functions.htmlEntity(PostalCode);
                xml.Append("<PostalCode>" + value + "</PostalCode>"); 
            }

             if (Country != string.Empty)
            {                
                string value = Functions.htmlEntity(Country);
                xml.Append("<Country>" + value + "</Country>"); 
            }

            if (Note != string.Empty)
            {                
                string value = Functions.htmlEntity(Note);
                xml.Append("<Note>" + value + "</Note>"); 
            }

            xml.Append("</ShipAddress>");
                                

            return xml.ToString();
        }
    }
}
