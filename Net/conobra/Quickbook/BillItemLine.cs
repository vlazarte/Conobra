using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class BillItemLine
    {

        /*
<ItemLineAdd>
<ItemRef>
	<FullName>WOLFRAM</FullName>
</ItemRef>
<Desc>Test</Desc>
<Quantity>1.1</Quantity>
<Cost>100</Cost>
<ClassRef>
	<FullName>4001 - OPERACION</FullName>
</ClassRef>
<DataExt>
	<OwnerID >0</OwnerID>
	<DataExtName >Ley</DataExtName>
	<DataExtValue >60.25</DataExtValue>
</DataExt>
</ItemLineAdd>
         */

        private string itemName;
        private string desc;
        private float quantity;
        private float cost;
        private string className;

        public string TxnLineID;

        public BillItemLine(string _itemName, string _desc, float _qty, float _price, string _className, string txnID = "")
        {

            itemName = _itemName;
            desc = _desc;
            quantity = _qty;
            cost = _price;
            className = _className;
            TxnLineID = txnID;
        }

        public void copy(BillItemLine line)
        {
            itemName = line.itemName;
            desc = line.desc;
            quantity = line.quantity;
            cost = line.cost;
            className = line.className;
        }

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public string toXmlAdd()
        {

            /*
            <ItemLineAdd>
            <ItemRef>
                <FullName>WOLFRAM</FullName>
            </ItemRef>
            <Desc>Test</Desc>
            <Quantity>1.1</Quantity>
            <Cost>100</Cost>
            <ClassRef>
                <FullName>4001 - OPERACION</FullName>
            </ClassRef>
            <DataExt>
                <OwnerID >0</OwnerID>
                <DataExtName >Ley</DataExtName>
                <DataExtValue >60.25</DataExtValue>
            </DataExt>
            </ItemLineAdd>
                     */

            string xml = Environment.NewLine + "<ItemLineAdd>";
            if (itemName != string.Empty)
            {
                xml += Environment.NewLine + "<ItemRef>";
                xml += Environment.NewLine + "<FullName >" + itemName + "</FullName>";
                xml += Environment.NewLine + "</ItemRef>";
            }

            if (desc != string.Empty)
            {
                xml += Environment.NewLine + "<Desc >" + desc + "</Desc>";
            }

            xml += Environment.NewLine + "<Quantity >" + quantity.ToString("0.00") + "</Quantity>";
            xml += Environment.NewLine + "<Amount >" + cost.ToString("0.00") + "</Amount>";

            if (className != string.Empty)
            {
                xml += Environment.NewLine + "<ClassRef>";
                xml += Environment.NewLine + "<FullName >" + className + "</FullName>";
                xml += Environment.NewLine + "</ClassRef>";
            }

            xml += Environment.NewLine + "</ItemLineAdd>";

            return xml;
        }

        public string toXmlMod()
        {
            string xml = Environment.NewLine + "<ItemLineMod>";

            xml += Environment.NewLine + "<TxnLineID>" + TxnLineID + "</TxnLineID>";

            if (itemName != string.Empty)
            {
                xml += Environment.NewLine + "<ItemRef>";
                xml += Environment.NewLine + "<FullName >" + itemName + "</FullName>";
                xml += Environment.NewLine + "</ItemRef>";
            }

            if (desc != string.Empty)
            {
                xml += Environment.NewLine + "<Desc >" + desc + "</Desc>";
            }

            xml += Environment.NewLine + "<Quantity >" + quantity.ToString("0.00") + "</Quantity>";
            xml += Environment.NewLine + "<Amount >" + cost.ToString("0.00") + "</Amount>";

            if (className != string.Empty)
            {
                xml += Environment.NewLine + "<ClassRef>";
                xml += Environment.NewLine + "<FullName >" + className + "</FullName>";
                xml += Environment.NewLine + "</ClassRef>";
            }

            xml += Environment.NewLine + "</ItemLineMod>";

            return xml;
        }

        public  string toXml()
        {
            if (TxnLineID == string.Empty)
            {
                return toXmlAdd();
            }
            else
            {
                return toXmlMod();
            }
        }
    
    }
}
