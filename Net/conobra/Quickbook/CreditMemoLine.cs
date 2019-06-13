using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class CreditMemoLine : TransactionLine
    {

        public bool isValid()
        {
            return true;
        }

        public string ToLine()
        {
            string[] vals = new string[16];
            //vals[0] = TxnLineID;
            vals[0] = "" + indice;
            vals[1] = ItemRef.ListID;
            vals[2] = "" + Quantity;
            vals[3] = "" + UnitOfMeasure;
            vals[4] = "" + Desc;
            vals[5] = "" + Rate;
            vals[6] = ""; // Descuento 
            vals[7] = ""; // Naturaleza de Descuento
            vals[8] = isService == true ? "1" : "0"; //1: Si el articulo en un servicio ó 0 si el articulo es un material
            vals[9] = ""; // Si no hay impuesto se debe llenar al informacion de la Exoneracion
            vals[10] = "" ; //Si no hay exoneracion debe esta la informacion del impuesto aplicado
            vals[11] = "" ; //Si no hay exoneracion debe esta la informacion del impuesto aplicado
            vals[12] = "" ; //Si no hay exoneracion debe esta la informacion del impuesto aplicado
            vals[13] = "" ; //Si no hay exoneracion debe esta la informacion del impuesto aplicado
            vals[14] = "" ; //Si no hay exoneracion debe esta la informacion del impuesto aplicado
            vals[15] = "" ; //Si no hay exoneracion debe esta la informacion del impuesto aplicado

            return String.Join("|", vals);
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
