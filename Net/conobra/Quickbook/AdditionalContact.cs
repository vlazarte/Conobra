using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Quickbook
{
    public class AdditionalContact
    {

    
      public string ContactName { get; set; }
      public  string ContactValue{get;set;}

      public bool isValid() {
          return ContactName != string.Empty && ContactValue != string.Empty;
      }
        
      public string toXmlRef()
      {
          StringBuilder xml = new StringBuilder();
          XmlElement ele = (new XmlDocument()).CreateElement("test");
          xml.Append("<AdditionalContactRef>");
          if (ContactName != string.Empty)
          {
              ele.InnerText = ContactName + "";
              xml.Append("<ContactName>" + ele.InnerXml + "</ContactName>");
          }
          if (ContactValue != string.Empty)
          {
              ele.InnerText = ContactValue + "";
              xml.Append("<ContactValue>" + ele.InnerXml + "</ContactValue>"); 
          }

          xml.Append("</AdditionalContactRef>");

          

          return xml.ToString();
      }
    }
}
