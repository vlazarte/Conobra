using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class Template : Abstract
    {
        public string FullName;

        public string toXmlRef()
        {
            string xml = "<TemplateRef>" ;
            if (ListID != "" && ListID != null)
                xml += "<ListID >" + ListID + "</ListID>";
            else if (FullName != "" && FullName != null)
                xml += "<FullName >" + FullName + "</FullName>";

            xml += "</TemplateRef>";

            return xml;
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet Template";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Template";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet Template";
            return new List<Abstract>();
        }

    }
}
