using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public class PriceLevel : Abstract
    {

        public string FullName;
        
        public string toXmlRef()
        {
         

            StringBuilder xml = new StringBuilder();            
            xml.Append("<PriceLevelRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {                
                string value = Functions.htmlEntity(FullName);
                xml.Append("<FullName>" + value + "</FullName>");
            }

            xml.Append("</PriceLevelRef>");
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
