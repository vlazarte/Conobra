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
            XmlElement ele = (new XmlDocument()).CreateElement("test");
            xml.Append("<TaxLineInfoRet>");
            if (ListID != string.Empty)
            {
                xml.Append("<TaxLineID>" + ListID + "</TaxLineID>");
            }
            if (FullName != string.Empty)
            {
                ele.InnerText = FullName + "";
                xml.Append("<TaxLineName>" + ele.InnerXml + "</TaxLineName>"); //-- required -->
            }

            xml.Append("</TaxLineInfoRet>");
            return xml.ToString();

        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet PriceLevel";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err)
        {
            err = "No implemented yet PriceLevel";
            return new List<Abstract>();
        }
    }
}
