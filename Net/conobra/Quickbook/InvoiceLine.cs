using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class InvoiceLine : TransactionLine
    {
        
        
        //DateExRet

        public bool isValid()
        {
            return true;
        }

      
        public string toXmlAdd()
        {
            string xml = "<InvoiceLineAdd>";
            if (ItemRef != null)
                xml += ItemRef.toXmlRef();
            if (Desc != "")
                xml += "<Desc>" + Desc + "</Desc>";
            if (Quantity != null)
                xml += "<Quantity>" + Functions.FloatToString((float)Quantity) + "</Quantity>";
            if (UnitOfMeasure != "")
                xml += "<UnitOfMeasure>" + UnitOfMeasure + "</UnitOfMeasure>";

            if (Rate != null)
            {
                xml += "<Rate>" + Functions.FloatToString((float)Rate) + "</Rate>";
            }
            else if (RatePercent != null)
            {
                xml += "<RatePercent>" + Functions.FloatToString((float)RatePercent) + "</RatePercent>";
            }
            else if (PriceLevelRef != null)
            {
                xml += PriceLevelRef.toXmlRef();
            }
            if (ClassRef != null)
            {
                xml += ClassRef.toXmlRef();
            }
            if (Amount != null)
                xml += "<Amount>" + Functions.FloatToString((float)Amount) + "</Amount>";

            if (InventorySiteRef != null)
            {
                xml += InventorySiteRef.toXmlRef();
            }
            //OptionForPriceRuleConflict 
            xml += "</InvoiceLineAdd>";
            return xml;
        }

    }
}
