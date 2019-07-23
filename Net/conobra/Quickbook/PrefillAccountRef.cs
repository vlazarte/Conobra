﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public class PrefillAccountRef
    {

        public string ListID { get; set; }
        public string FullName { get; set; }


        public string toXmlRef()
        {
            StringBuilder xml = new StringBuilder();
            XmlElement ele = (new XmlDocument()).CreateElement("test");
            xml.Append("<PrefillAccountRef>");
            if (ListID != string.Empty)
            {
                xml.Append("<ListID >" + ListID + "</ListID>");
            }
            if (FullName != string.Empty)
            {
                string value = Functions.htmlEntity(FullName);
                xml.Append("<FullName>" + value + "</FullName>"); 
            }

            xml.Append("</PrefillAccountRef>");

            return xml.ToString();
        }

        
    }
}
