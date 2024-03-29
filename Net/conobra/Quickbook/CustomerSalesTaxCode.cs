﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class CustomerSalesTaxCode : Abstract
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
            return "<CustomerSalesTaxCodeRef>" +
            "<ListID >" + ListID + "</ListID>" +
            "</CustomerSalesTaxCodeRef>";
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet CustomerSalesTaxCode";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet CustomerSalesTaxCode";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet CustomerSalesTaxCode";
            return new List<Abstract>();
        }


    }
}
