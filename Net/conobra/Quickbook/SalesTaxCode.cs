using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class SalesTaxCode : Abstract
    {
        public string FullName;

        /*
         
         <SalesTaxCodeRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</SalesTaxCodeRef>
         */

        public string toXmlRef()
        {
            return "<SalesTaxCodeRef>" +
            "<ListID >" + ListID + "</ListID>" +
            "</SalesTaxCodeRef>";
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet SalesTaxCode";
            return false;
        }
    }
}
