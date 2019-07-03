using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class TaxVendor : Abstract
    {

        public string FullName;

        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived) {
            err = "No implemented yet TaxVendor";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet TaxVendor";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet TaxVendor";
            return new List<Abstract>();
        }
    }
}
