﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class CustomerType : Abstract 
    {
        public string FullName;

        /*
        <CustomerTypeRef> <!-- optional -->
<ListID >IDTYPE</ListID> <!-- optional -->
<FullName >STRTYPE</FullName> <!-- optional -->
</CustomerTypeRef>
         */

        public string toXmlRef()
        {
            return "";
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet CustomerType";
            return false;
        }

    }
}
