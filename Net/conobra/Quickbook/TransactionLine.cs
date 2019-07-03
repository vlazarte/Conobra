using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class TransactionLine : Abstract
    {
        public static string TYPE_SUBTOTAL = "subtotal";
        public static string TYPE_DISCOUNT = "descuento";
        public static string TYPE_TAX = "impuesto";
        public static string TYPE_ITEM = "item";

        public string TxnLineID;
        public Item ItemRef;
        public string Desc;
        public decimal? Quantity;
        public string UnitOfMeasure = "";
        public UnitOfMeasureSet UnitOfMeasureSetRef;

        public decimal? Rate;
        // or
        public decimal? RatePercent;
        // or
        public PriceLevel PriceLevelRef;

        public Class ClassRef;
        public decimal? Amount;
        public InventorySite InventorySiteRef;
        //InventorySiteLocationRef

        public string SerialNumber;
        // or
        public string LotNumber;

        public DateTime? ServiceDate;
        public SalesTaxCode SalesTaxCodeRef;

        public string Other1;
        public string Other2;

        public int indice = 0;

        public bool isService = false;

        public decimal AmountDiscount = 0;
        public string ReasonDiscount = "";

        public string codigoImpuesto = "";
        public decimal montoImpuesto = 0;


        public List<TransactionLine> Discounts = new List<TransactionLine>();
        public List<TransactionLine> Taxes = new List<TransactionLine>();

        public string Type = "";
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet TransactionLine";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err, bool includeSublevel)
        {
            err = "No implemented yet TransactionLine";
            return new List<Abstract>();
        }
        public override List<Abstract> GetRecordsCVS(ref string err, bool includeSublevel)
        {
            err = "No implemented yet TransactionLine";
            return new List<Abstract>();
        }
    }
}
