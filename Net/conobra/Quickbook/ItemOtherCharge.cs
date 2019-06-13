using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace Quickbook
{
    public class ItemOtherCharge : ItemAbstract
    {

        public DateTime TimeCreated;
        public DateTime TimeModified;
        public string EditSequence = string.Empty;
        public string Name = string.Empty;
        public string FullName = string.Empty;
        public string BarCodeValue = string.Empty;
        public bool IsActive;

        public Class ClassRef;
        public ItemOtherCharge ParentRef;
        public int Sublevel;
        
        public SalesTaxCode SalesTaxCodeRef;

        public string ServiceType = string.Empty;
        #region SalesOrPurchase
        public string Desc = string.Empty;
        
        public float? Price;
        // or
        public float? PricePercent;

        public Account AccountRef;
        #endregion
        // or
        #region SalesAndPurchase
        public string SalesDesc = string.Empty;
        public float? SalesPrice;

        public IncomeAccount IncomeAccountRef;
        public string PurchaseDesc = string.Empty;
        public float? PurchaseCost;
        public ExpenseAccount ExpenseAccountRef;
        public PrefVendor PrefVendorRef;
        #endregion

        
        //FinanceCharge, ReimbursableExpenseGroup, ReimbursableExpenseSubtotal
        public string SpecialItemType = string.Empty;

        public string ExternalGUID = string.Empty;


        public ItemOtherCharge()
        {
        }

        public string getType()
        {
            return ServiceType;
        }

        public bool LoadByListID(string lid, ref string err)
        {
            string xml = "" +
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + 
            "<?qbxml version=\"13.0\"?>" + 
            "<QBXML>" + 
            "<QBXMLMsgsRq onError=\"stopOnError\">" +
            "<ItemOtherChargeQueryRq>" + 
            "<ListID >" + lid + "</ListID>" + 
            "<OwnerID>0</OwnerID>" +
            "</ItemOtherChargeQueryRq>" + 
            "</QBXMLMsgsRq>" + 
            "</QBXML>" ;
            var qbook = new Connector(Quickbook.Config.App_Name, Quickbook.Config.File);

            if (qbook.Connect())
            {
                string response = qbook.sendRequest(xml);

                XmlDocument res = new XmlDocument();
                res.LoadXml(response);

                string code = "";
                string statusMessage = "";

                code = res["QBXML"]["QBXMLMsgsRs"]["ItemOtherChargeQueryRs"].Attributes["statusCode"].Value;
                statusMessage = res["QBXML"]["QBXMLMsgsRs"]["ItemOtherChargeQueryRs"].Attributes["statusMessage"].Value;

                if (code == "0")
                {
                    var node = res["QBXML"]["QBXMLMsgsRs"]["ItemOtherChargeQueryRs"]["ItemOtherChargeRet"];

                    ListID = "" + node["ListID"].InnerText;
                    TimeCreated = DateTime.Parse("" + node["TimeCreated"].InnerText);
                    TimeModified = DateTime.Parse("" + node["TimeModified"].InnerText);
                    EditSequence = "" + node["EditSequence"].InnerText;
                    Name = "" + node["Name"].InnerText;
                    FullName = "" + node["FullName"].InnerText;
                    if (node["BarCodeValue"] != null)
                        BarCodeValue = "" + node["BarCodeValue"].InnerText;
                    if (node["IsActive"] != null)
                        IsActive = ("" + node["IsActive"].InnerText == "true" ? true : false);

                    if (node["ClassRef"] != null)
                    {
                        ClassRef = new Class();
                        ClassRef.ListID = "" + node["ClassRef"]["ListID"].InnerText;
                        ClassRef.FullName = "" + node["ClassRef"]["FullName"].InnerText;
                    }

                    if (node["ParentRef"] != null)
                    {
                        ParentRef = new ItemOtherCharge();
                        ParentRef.ListID = "" + node["ParentRef"]["ListID"].InnerText;
                        ParentRef.FullName = "" + node["ParentRef"]["FullName"].InnerText;
                    }

                    Sublevel = Int32.Parse("" + node["Sublevel"].InnerText);

                    if (node["SalesTaxCodeRef"] != null)
                    {
                        SalesTaxCodeRef = new SalesTaxCode();
                        SalesTaxCodeRef.ListID = "" + node["SalesTaxCodeRef"]["ListID"].InnerText;
                        SalesTaxCodeRef.FullName = "" + node["SalesTaxCodeRef"]["FullName"].InnerText;
                    }

                    if (node["SalesOrPurchase"] != null)
                    {
                        ServiceType = "SalesOrPurchase";
                        if (node["SalesOrPurchase"]["Desc"] != null)
                            Desc = "" + node["SalesOrPurchase"]["Desc"].InnerText;

                        if (node["SalesOrPurchase"]["Price"] != null)
                            Price = Functions.ParseFloat("" + node["SalesOrPurchase"]["Price"].InnerText);
                        //or
                        if (node["SalesOrPurchase"]["PricePercent"] != null)
                            PricePercent = Functions.ParseFloat("" + node["SalesOrPurchase"]["PricePercent"].InnerText);

                        if (node["SalesOrPurchase"]["AccountRef"] != null)
                        {
                            AccountRef = new Account();
                            AccountRef.ListID = "" + node["SalesOrPurchase"]["AccountRef"]["ListID"].InnerText;
                            AccountRef.FullName = "" + node["SalesOrPurchase"]["AccountRef"]["FullName"].InnerText;
                        }
                    }

                    if (node["SalesAndPurchase"] != null)
                    {
                        ServiceType = "SalesAndPurchase";
                        if (node["SalesAndPurchase"]["SalesDesc"] != null)
                            SalesDesc = "" + node["SalesAndPurchase"]["SalesDesc"].InnerText;
                        if (node["SalesAndPurchase"]["SalesPrice"] != null)
                            SalesPrice = Functions.ParseFloat("" + node["SalesAndPurchase"]["SalesPrice"].InnerText);

                        if (node["SalesAndPurchase"]["IncomeAccountRef"] != null)
                        {
                            IncomeAccountRef = new IncomeAccount();
                            IncomeAccountRef.ListID = "" + node["SalesAndPurchase"]["IncomeAccountRef"]["ListID"].InnerText;
                            IncomeAccountRef.FullName = "" + node["SalesAndPurchase"]["IncomeAccountRef"]["FullName"].InnerText;
                        }

                        if (node["SalesAndPurchase"]["PurchaseDesc"] != null)
                            PurchaseDesc = "" + node["SalesAndPurchase"]["PurchaseDesc"].InnerText;
                        if (node["SalesAndPurchase"]["PurchaseCost"] != null)
                            PurchaseCost = Functions.ParseFloat("" + node["SalesAndPurchase"]["PurchaseCost"].InnerText);

                        if (node["SalesAndPurchase"]["ExpenseAccountRef"] != null)
                        {
                            ExpenseAccountRef = new ExpenseAccount();
                            ExpenseAccountRef.ListID = "" + node["SalesAndPurchase"]["ExpenseAccountRef"]["ListID"].InnerText;
                            ExpenseAccountRef.FullName = "" + node["SalesAndPurchase"]["ExpenseAccountRef"]["FullName"].InnerText;
                        }

                        if (node["SalesAndPurchase"]["PrefVendorRef"] != null)
                        {
                            PrefVendorRef = new PrefVendor();
                            PrefVendorRef.ListID = "" + node["SalesAndPurchase"]["PrefVendorRef"]["ListID"].InnerText;
                            PrefVendorRef.FullName = "" + node["SalesAndPurchase"]["PrefVendorRef"]["FullName"].InnerText;
                        }

                    }

                    if (node["SpecialItemType"] != null)
                        SpecialItemType = "" + node["SpecialItemType"].InnerText;

                    if (node["ExternalGUID"] != null)
                        ExternalGUID = "" + node["ExternalGUID"].InnerText;

                    XmlNodeList __extras = node.SelectNodes("DataExtRet");
                    foreach (XmlNode ex in __extras)
                    {
                        var name = ex["DataExtName"].InnerText;
                        var value = ex["DataExtValue"].InnerText;
                        AddDataEx(name, value);
                    }
                    return true;

                }
                else
                {
                    err = statusMessage;
                }
                qbook.Disconnect();
            }
            else
            {
                err = "QuickBook no conecto";
            }

            return false;
        }

    }
}
