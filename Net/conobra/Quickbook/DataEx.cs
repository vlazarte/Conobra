using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class DataEx
    {
        public string ListDataExtType;
        public string ListObjRef_ListID;
        public string DataExtName;
        public string DataExtValue;
        public string OwnerID;

        public string toXmlAdd()
        {
            
            if (!(DataExtName != "" && DataExtValue != "" && ListDataExtType != "" && ListObjRef_ListID != ""))
                return "";
            string xml = "";
            xml += "<DataExtAddRq>" +
                "<DataExtAdd>" +
                    "<OwnerID >0</OwnerID>" +
                    "<DataExtName>" + DataExtName + "</DataExtName>" +
                    "<ListDataExtType >" + ListDataExtType + "</ListDataExtType>" +
                    "<ListObjRef>" +
                        "<ListID >" + ListObjRef_ListID + "</ListID>" +
                    "</ListObjRef>" +
                    "<DataExtValue >" + DataExtValue + "</DataExtValue>" +
                "</DataExtAdd>" +
            "</DataExtAddRq>";

            return xml;
        }
    }
}
