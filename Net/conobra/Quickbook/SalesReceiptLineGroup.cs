using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class SalesReceiptLineGroup : Abstract
    {
        public string TxnLineID;
        public ItemGroup ItemGroupRef;
        public string Desc;
        public float Quantity;
        public string UnitOfMeasure;
        //OverrideUOMSetRef
        public bool IsPrintItemsInGroup;
        public float TotalAmount;
        public List<SalesReceiptLine> SalesReceiptLineRet ;

        public bool isValid()
        {
            return false;
        }

        public string toXmlAdd()
        {
            return "";
        }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet SalesReceiptLineGroup";
            return false;
        }

    }
}
