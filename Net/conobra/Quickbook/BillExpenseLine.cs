using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class BillExpenseLine:Abstract
    {
        public Account AccountRef{get;set;}
        public Double Amount{get;set;}
        public string Memo{get;set;}
        public Customer CustomerRef{get;set;}
        public Class ClassRef{get;set;}
        public string BillableStatus{get;set;}
        public string TxnLineID;
        public BillExpenseLine()
        {
            AccountRef = null;
            Memo = string.Empty;
            CustomerRef = null;
            ClassRef = null;
            TxnLineID = string.Empty;
            
        }

        public void copy( BillExpenseLine line )
        {
            AccountRef = line.AccountRef;
            Amount = line.Amount;
            Memo = line.Memo;
            CustomerRef = line.CustomerRef;
            ClassRef = line.ClassRef;
            BillableStatus = string.Empty;
            TxnLineID = string.Empty;
        }

        public string toXmlAdd()
        {
            string xml = Environment.NewLine + "<ExpenseLineAdd>";
            if (AccountRef != null)
            {
                xml += Environment.NewLine + AccountRef.toXmlRef();               
            }

            System.Globalization.CultureInfo myInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            
            xml += Environment.NewLine + "<Amount>" + Amount.ToString("0.00", myInfo) + "</Amount>";
            if (Memo != string.Empty)
            {
                xml += Environment.NewLine + "<Memo>" + Memo + "</Memo>";
            }

            if (CustomerRef != null) {
                xml += CustomerRef.toXmlRef();
            }
            if (ClassRef != null) {
                xml += ClassRef.toXmlRef();
            }

            if (BillableStatus != null &&  BillableStatus!=string.Empty) {
                xml += Environment.NewLine + "<BillableStatus>" + BillableStatus + "</BillableStatus> ";            
            }


            xml += Environment.NewLine + "</ExpenseLineAdd>";

            return xml;
        }

        public string toXmlMod()
        {
            string xml = Environment.NewLine + "<ExpenseLineMod>";
            xml += Environment.NewLine + "<TxnLineID>" + TxnLineID + "</TxnLineID>";
            if (AccountRef != null)
            {
                xml += Environment.NewLine + "<AccountRef>";
                xml += Environment.NewLine + "<ListID >" + AccountRef.ListID + "</ListID>";
                //xml += "<FullName >STRTYPE</FullName>";
                xml += Environment.NewLine + "</AccountRef>";
            }

            xml += Environment.NewLine + "<Amount >" + Amount.ToString("0.00") + "</Amount>";
            if (Memo != string.Empty)
            {
                xml += Environment.NewLine + "<Memo >" + Memo + "</Memo>";
            }

            /*xml += Environment.NewLine + "<CustomerRef>";
                xml += Environment.NewLine + "<ListID >IDTYPE</ListID>";
                xml += Environment.NewLine + "<FullName >STRTYPE</FullName>";
            xml += Environment.NewLine + "</CustomerRef>"; */

            if (ClassRef != null)
            {
                xml += Environment.NewLine + "<ClassRef>";
                xml += Environment.NewLine + "<FullName >" + ClassRef.FullName + "</FullName>";
                xml += Environment.NewLine + "</ClassRef>";
            }



            xml += Environment.NewLine + "</ExpenseLineMod>";

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

        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet BillExpenseLine";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err)
        {
            err = "No implemented yet BillExpenseLine";
            return new List<Abstract>();
        }
    
        /*

		<CustomerRef> <!-- optional -->
			<ListID >IDTYPE</ListID> <!-- optional -->
			<FullName >STRTYPE</FullName> <!-- optional -->
		</CustomerRef>

		<SalesRepRef> <!-- optional -->
			<ListID >IDTYPE</ListID> <!-- optional -->
			<FullName >STRTYPE</FullName> <!-- optional -->
		</SalesRepRef>
		<DataExt> <!-- optional, may repeat -->
			<OwnerID >GUIDTYPE</OwnerID> <!-- required -->
			<DataExtName >STRTYPE</DataExtName> <!-- required -->
			<DataExtValue >STRTYPE</DataExtValue> <!-- required -->
		</DataExt>
         
         */
    }
}
