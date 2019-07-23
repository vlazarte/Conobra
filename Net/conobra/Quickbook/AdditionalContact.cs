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
          
          xml.Append("<AdditionalContactRef>");
          if (ContactName != string.Empty)
          {   
              string value = Functions.htmlEntity(ContactName);
              xml.Append("<ContactName>" + value + "</ContactName>");
          }
          if (ContactValue != string.Empty)
          {
              string value = Functions.htmlEntity(ContactValue);
              xml.Append("<ContactValue>" + value + "</ContactValue>"); 
          }

          xml.Append("</AdditionalContactRef>");

          

          return xml.ToString();
      }
    }
}
