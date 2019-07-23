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
            xml.Append("<Contacts>");
            if (Salutation != string.Empty)
            {
                  
                  string value = Functions.htmlEntity(Salutation);
                  xml.Append("<Salutation >" + value + "</Salutation>");
            }
            if (FirstName != string.Empty)
            {               
                string value = Functions.htmlEntity(FirstName);
                xml.Append("<FirstName>" + value + "</FirstName>"); //-- required -->
            }

              if (MiddleName != string.Empty)
            {                
                string value = Functions.htmlEntity(MiddleName);
                xml.Append("<MiddleName>" + value + "</MiddleName>"); //-- required -->
            }

              if (LastName != string.Empty)
            {                
                string value = Functions.htmlEntity(LastName);
                xml.Append("<LastName>" + value + "</LastName>"); //-- required -->
            }


               if (JobTitle != string.Empty)
            {
                
                string value = Functions.htmlEntity(JobTitle);
                xml.Append("<JobTitle>" + value + "</JobTitle>"); //-- required -->
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
