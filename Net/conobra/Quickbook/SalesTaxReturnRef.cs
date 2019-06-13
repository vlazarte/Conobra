﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public class SalesTaxReturnRef
    {
        public string ListID { get; set; }
        public string FullName { get; set; }


        public string toXmlRef()
        {
            StringBuilder xml = new StringBuilder();
            XmlElement ele = (new XmlDocument()).CreateElement("test");
            xml.Append("<SalesTaxReturnRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {
                ele.InnerText = FullName + "";
                xml.Append("<FullName>" + ele.InnerXml + "</FullName>"); //-- required -->
            }

            xml.Append("</SalesTaxReturnRef>");

            return xml.ToString();
        }
         
    }
}
