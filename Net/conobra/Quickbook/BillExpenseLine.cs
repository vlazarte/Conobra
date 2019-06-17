using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class BillExpenseLine
    {
        public Account account;
        public float amount;
        public string memo;
        public string classRef;

        public string TxnLineID;

        public BillExpenseLine( Account _account, float _amount, string _memo, string _class, string txnID )
        {
            account = _account;
            amount = _amount;
            memo = _memo;
            classRef = _class;
            TxnLineID = txnID;
            
        }

        public void copy( BillExpenseLine line )
        {
            account = line.account;
            amount = line.amount;
            memo = line.memo;
            classRef = line.classRef;
        }

        public string toXmlAdd()
        {
            string xml = Environment.NewLine + "<ExpenseLineAdd>";
            if (account != null)
            {
                xml += Environment.NewLine + "<AccountRef>";
                xml += Environment.NewLine + "<ListID >" + account.ListID + "</ListID>";
                //xml += "<FullName >STRTYPE</FullName>";
                xml += Environment.NewLine + "</AccountRef>";
            }

            xml += Environment.NewLine + "<Amount >" + amount.ToString("0.00") + "</Amount>";
            if (memo != string.Empty)
            {
                xml += Environment.NewLine + "<Memo >" + memo + "</Memo>";
            }

            /*xml += Environment.NewLine + "<CustomerRef>";
                xml += Environment.NewLine + "<ListID >IDTYPE</ListID>";
                xml += Environment.NewLine + "<FullName >STRTYPE</FullName>";
            xml += Environment.NewLine + "</CustomerRef>"; */

            if (classRef != string.Empty)
            {
                xml += Environment.NewLine + "<ClassRef>";
                xml += Environment.NewLine + "<FullName >" + classRef + "</FullName>";
                xml += Environment.NewLine + "</ClassRef>";
            }



            xml += Environment.NewLine + "</ExpenseLineAdd>";

            return xml;
        }

        public string toXmlMod()
        {
            string xml = Environment.NewLine + "<ExpenseLineMod>";
            xml += Environment.NewLine + "<TxnLineID>" + TxnLineID + "</TxnLineID>";
            if (account != null)
            {
                xml += Environment.NewLine + "<AccountRef>";
                xml += Environment.NewLine + "<ListID >" + account.ListID + "</ListID>";
                //xml += "<FullName >STRTYPE</FullName>";
                xml += Environment.NewLine + "</AccountRef>";
            }

            xml += Environment.NewLine + "<Amount >" + amount.ToString("0.00") + "</Amount>";
            if (memo != string.Empty)
            {
                xml += Environment.NewLine + "<Memo >" + memo + "</Memo>";
            }

            /*xml += Environment.NewLine + "<CustomerRef>";
                xml += Environment.NewLine + "<ListID >IDTYPE</ListID>";
                xml += Environment.NewLine + "<FullName >STRTYPE</FullName>";
            xml += Environment.NewLine + "</CustomerRef>"; */

            if (classRef != string.Empty)
            {
                xml += Environment.NewLine + "<ClassRef>";
                xml += Environment.NewLine + "<FullName >" + classRef + "</FullName>";
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
