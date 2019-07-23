using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public class SalesTaxCode : Abstract
    {
        public string FullName;


        public string toXmlRef()
        {
            StringBuilder xml = new StringBuilder();
            
            xml.Append("<SalesTaxReturnRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {
                
                string value = Functions.htmlEntity(FullName);
                xml.Append("<FullName>" + value + "</FullName>"); //-- required -->
            }

            xml.Append("</SalesTaxReturnRef>");

            return xml.ToString();
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet SalesTaxCode";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet SalesTaxCode";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet SalesTaxCode";
            return new List<Abstract>();
        }
    }
}
