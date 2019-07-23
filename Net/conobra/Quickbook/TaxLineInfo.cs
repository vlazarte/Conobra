using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public class TaxLineInfo : Abstract
    {

        public string FullName;
 
        public string toXmlRef()
        {

            StringBuilder xml = new StringBuilder();
            
            xml.Append("<TaxLineInfoRet>");
            if (ListID != string.Empty)
            {
                xml.Append("<TaxLineID>" + ListID + "</TaxLineID>");
            }
            if (FullName != string.Empty)
            {                
                string value = Functions.htmlEntity(FullName);
                xml.Append("<TaxLineName>" + value + "</TaxLineName>"); //-- required -->
            }

            xml.Append("</TaxLineInfoRet>");
            return xml.ToString();

        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet PriceLevel";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet PriceLevel";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet PriceLevel";
            return new List<Abstract>();
        }
    }
}
