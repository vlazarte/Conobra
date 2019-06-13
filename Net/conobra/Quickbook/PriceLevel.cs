using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class PriceLevel : Abstract
    {

        public string FullName;
        /*
         <PriceLevelRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</PriceLevelRef>
         */
        public string toXmlRef()
        {
            return "<PriceLevelRef>" +
            "<ListID >" + ListID + "</ListID>" + 
            "</PriceLevelRef>" ;
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet PriceLevel";
            return false;
        }
    }
}
