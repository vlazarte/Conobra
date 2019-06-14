using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class InvoiceLineGroup : Abstract
    {
        public string TxnLineID;
        public ItemGroup ItemGroupRef;
        public string Desc;
        public float Quantity;
        public string UnitOfMeasure;
        //OverrideUOMSetRef
        public bool IsPrintItemsInGroup;
        public float TotalAmount;
        public List<InvoiceLine> InvoiceLineRet ;

        public List<TransactionLine> Discounts = new List<TransactionLine>();
        public List<TransactionLine> Taxes = new List<TransactionLine>();


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
            err = "No implemented yet InvoiceLineGroup";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err)
        {
            err = "No implemented yet InvoiceLineGroup";
            return new List<Abstract>();
        }

    }
}
