using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public class Contact
    {
        public string ListID { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public List<AdditionalContact> AdicionalContacRef { get; set; }
        public string toXmlRef()
        {
            StringBuilder xml = new StringBuilder();
            XmlElement ele = (new XmlDocument()).CreateElement("test");
            xml.Append("<Contacts>");
            if (Salutation != string.Empty)
            {
                  ele.InnerText = Salutation + "";
                xml.Append("<Salutation >" + ele.InnerXml  + "</Salutation>");
            }
            if (FirstName != string.Empty)
            {
                ele.InnerText = FirstName + "";
                xml.Append("<FirstName>" + ele.InnerXml + "</FirstName>"); //-- required -->
            }

              if (MiddleName != string.Empty)
            {
                ele.InnerText = MiddleName + "";
                xml.Append("<MiddleName>" + ele.InnerXml + "</MiddleName>"); //-- required -->
            }

              if (LastName != string.Empty)
            {
                ele.InnerText = LastName + "";
                xml.Append("<LastName>" + ele.InnerXml + "</LastName>"); //-- required -->
            }


               if (JobTitle != string.Empty)
            {
                ele.InnerText = LastName + "";
                xml.Append("<JobTitle>" + ele.InnerXml + "</JobTitle>"); //-- required -->
            }

             if (AdicionalContacRef!=null && AdicionalContacRef.Count>0)
            {
                for (int j = 0; j < AdicionalContacRef.Count; j++)
                {
                    if (AdicionalContacRef[j].isValid()) {
                        xml.Append(AdicionalContacRef[j].toXmlRef()); //-- required -->
                    }
                    
                }
               
            }
            xml.Append("</Contacts>");

            return xml.ToString();
        }
    }
}
