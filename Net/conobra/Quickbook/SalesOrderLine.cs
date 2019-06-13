using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;

namespace Quickbook
{
    public class SalesOrderLine
    {

        public List<string> ItemRef;
                /*
        <ItemRef>
        <ListID>80000929-1438041005</ListID>
        
        </ItemRef>
         */
        public string Desc;
        public float? Quantity ;
        public float? Rate;
        public string InventorySite = "";


        public SalesOrderLine()
        {
        }

        public string toXMLAdd()
        {
            string xml = "" +
                "<SalesOrderLineAdd>" +
                    "<ItemRef>" +
                        "<ListID>" + ItemRef[0] + "</ListID>" +
                    "</ItemRef>" +
                    "<Desc>" + Desc + "</Desc>" +
                    ( Quantity != null ? "<Quantity>" + Quantity + "</Quantity>" : "" ) +
                    "<Rate>" + Rate + "</Rate>";

            if (InventorySite != "")
            {
                xml += "<InventorySiteRef>" +
                    "<ListID >" + InventorySite + "</ListID>" +
                    "</InventorySiteRef>";
            }


            xml += "</SalesOrderLineAdd>";
            return xml;
        }
        
        

    }
}
