using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public class TaxOnSalesAccountRef
    {
        public string ListID { get; set; }
        public string FullName { get; set; }


        public string toXmlRef()
        {
            StringBuilder xml = new StringBuilder();
            
            xml.Append("<TaxOnSalesAccountRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {                
                string value = Functions.htmlEntity(FullName);
                xml.Append("<FullName>" + value + "</FullName>"); //-- required -->
            }

            xml.Append("</TaxOnSalesAccountRef>");

            return xml.ToString();
        }
    }
}
