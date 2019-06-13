using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public class VendorAddress
    {
        
        public string Addr1 {get;set;}
        public string Addr2 {get;set;}
        public string Addr3 {get;set;}
        public string Addr4 {get;set;}
        public string Addr5 {get;set;}
        public string City {get;set;}
        public string State {get;set;}
        public string PostalCode {get;set;}
        public string Country {get;set;}
        public string Note {get;set;}

        public string toXmlRef()
        {
            StringBuilder xml = new StringBuilder();
            XmlElement ele = (new XmlDocument()).CreateElement("test");
            xml.Append("<VendorAddress>");
            if (Addr1 != string.Empty)
            {
                 ele.InnerText = Addr1 + "";
                xml.Append("<Addr1>" + ele.InnerXml + "</Addr1>");
            }
            if (Addr2 != string.Empty)
            {
                ele.InnerText = Addr2 + "";
                xml.Append("<Addr2>" + ele.InnerXml + "</Addr2>");
            }
            if (Addr3 != string.Empty)
            {
                ele.InnerText = Addr3 + "";
                xml.Append("<Addr3>" + ele.InnerXml + "</Addr3>");
            }

             if (Addr4 != string.Empty)
            {
                ele.InnerText = Addr4 + "";
                xml.Append("<Addr4>" + ele.InnerXml + "</Addr4>");
            }

             if (Addr5 != string.Empty)
            {
                ele.InnerText = Addr5 + "";
                xml.Append("<Addr5>" + ele.InnerXml + "</Addr5>");
            }
             if (City != string.Empty)
            {
                ele.InnerText = City + "";
                xml.Append("<City>" + ele.InnerXml + "</City>");
            }
             if (State != string.Empty)
            {
                ele.InnerText = State + "";
                xml.Append("<State>" + ele.InnerXml + "</State>");
            }
              if (PostalCode != string.Empty)
            {
                ele.InnerText = PostalCode + "";
                xml.Append("<PostalCode>" + ele.InnerXml + "</PostalCode>");
            }

              if (Country != string.Empty)
            {
                ele.InnerText = Country + "";
                xml.Append("<Country>" + ele.InnerXml + "</Country>");
            }
            if (Note != string.Empty)
            {
                ele.InnerText = Note + "";
                xml.Append("<Note>" + ele.InnerXml + "</Note>");
            }

            xml.Append("</VendorAddress>");
                                

            return xml.ToString();
        }
        
    }
}
