using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class SalesRep
    {
        public string ListID;
        public string FullName;
        /*
         
         <SalesRepRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</SalesRepRef>
         */
        public string toXmlRef()
        {
            return "";
        }
    }
}
