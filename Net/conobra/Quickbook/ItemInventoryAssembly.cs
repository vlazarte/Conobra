using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace Quickbook
{
    public class ItemInventoryAssembly : ItemAbstract
    {

        public DateTime TimeCreated;
        public DateTime TimeModified;
        public string EditSequence = string.Empty;
        public string Name = string.Empty;
        public string FullName = string.Empty;
        public string BarCodeValue = string.Empty;
        public bool IsActive;

        public Class ClassRef;
        public ItemInventory ParentRef;
        public int Sublevel;
        public string ManufacturerPartNumber = string.Empty;
        public UnitOfMeasureSet UnitOfMeasureSetRef;
        public SalesTaxCode SalesTaxCodeRef;
        public string SalesDesc = string.Empty;
        public float? SalesPrice;
        public IncomeAccount IncomeAccountRef;
        public string PurchaseDesc = string.Empty;
        public float? PurchaseCost;
        public COGSAccount COGSAccountRef;
        public PrefVendor PrefVendorRef;
        public AssetAccount AssetAccountRef;
        public float? BuildPoint;
        public float? Max;
        public float? QuantityOnHand;
        public float AverageCost;
        public float? QuantityOnOrder;
        public float? QuantityOnSalesOrder;
        public List<ItemInventoryAssemblyLine> ItemList;

        public string ExternalGUID = string.Empty;



        public ItemInventoryAssembly()
        {
            ItemList = new List<ItemInventoryAssemblyLine>();
        }

        public bool LoadByListID(string lid, ref string err)
        {
            string xml = "" +
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + 
            "<?qbxml version=\"13.0\"?>" + 
            "<QBXML>" + 
            "<QBXMLMsgsRq onError=\"stopOnError\">" +
            "<ItemInventoryAssemblyQueryRq>" + 
            "<ListID >" + lid + "</ListID>" + 
            "<OwnerID>0</OwnerID>" +
            "</ItemInventoryAssemblyQueryRq>" + 
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

                 code = res["QBXML"]["QBXMLMsgsRs"]["ItemInventoryAssemblyQueryRs"].Attributes["statusCode"].Value;
                 statusMessage = res["QBXML"]["QBXMLMsgsRs"]["ItemInventoryAssemblyQueryRs"].Attributes["statusMessage"].Value;

                 if (code == "0")
                 {
                     var node = res["QBXML"]["QBXMLMsgsRs"]["ItemInventoryAssemblyQueryRs"]["ItemInventoryAssemblyRet"];

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
                         ParentRef = new ItemInventory();
                         ParentRef.ListID = "" + node["ParentRef"]["ListID"].InnerText;
                         ParentRef.FullName = "" + node["ParentRef"]["FullName"].InnerText;
                     }

                     Sublevel = Int32.Parse("" + node["Sublevel"].InnerText);
                     if (node["ManufacturerPartNumber"] != null)
                         ManufacturerPartNumber = "" + node["ManufacturerPartNumber"].InnerText;

                     if (node["UnitOfMeasureSetRef"] != null)
                     {
                         UnitOfMeasureSetRef = new UnitOfMeasureSet();
                         UnitOfMeasureSetRef.ListID = "" + node["UnitOfMeasureSetRef"]["ListID"].InnerText;
                         UnitOfMeasureSetRef.FullName = "" + node["UnitOfMeasureSetRef"]["FullName"].InnerText;
                         UnitOfMeasureSetRef.LoadByListID(UnitOfMeasureSetRef.ListID, ref err);
                     }

                     if (node["SalesTaxCodeRef"] != null)
                     {
                         SalesTaxCodeRef = new SalesTaxCode();
                         SalesTaxCodeRef.ListID = "" + node["SalesTaxCodeRef"]["ListID"].InnerText;
                         SalesTaxCodeRef.FullName = "" + node["SalesTaxCodeRef"]["FullName"].InnerText;
                     }

                     if (node["SalesDesc"] != null)
                         SalesDesc = "" + node["SalesDesc"].InnerText;
                     if (node["SalesPrice"] != null)
                         SalesPrice = Functions.ParseFloat("" + node["SalesPrice"].InnerText);

                     if (node["IncomeAccountRef"] != null)
                     {
                         IncomeAccountRef = new IncomeAccount();
                         IncomeAccountRef.ListID = "" + node["IncomeAccountRef"]["ListID"].InnerText;
                         IncomeAccountRef.FullName = "" + node["IncomeAccountRef"]["FullName"].InnerText;
                     }

                     if (node["PurchaseDesc"] != null)
                         PurchaseDesc = "" + node["PurchaseDesc"].InnerText;
                     if (node["PurchaseCost"] != null)
                         PurchaseCost = Functions.ParseFloat("" + node["PurchaseCost"].InnerText);

                     if (node["COGSAccountRef"] != null)
                     {
                         COGSAccountRef = new COGSAccount();
                         COGSAccountRef.ListID = "" + node["COGSAccountRef"]["ListID"].InnerText;
                         COGSAccountRef.FullName = "" + node["COGSAccountRef"]["FullName"].InnerText;
                     }

                     if (node["PrefVendorRef"] != null)
                     {
                         PrefVendorRef = new PrefVendor();
                         PrefVendorRef.ListID = "" + node["PrefVendorRef"]["ListID"].InnerText;
                         PrefVendorRef.FullName = "" + node["PrefVendorRef"]["FullName"].InnerText;
                     }

                     if (node["AssetAccountRef"] != null)
                     {
                         AssetAccountRef = new AssetAccount();
                         AssetAccountRef.ListID = "" + node["AssetAccountRef"]["ListID"].InnerText;
                         AssetAccountRef.FullName = "" + node["AssetAccountRef"]["FullName"].InnerText;
                     }

                     if (node["BuildPoint"] != null)
                         BuildPoint = Functions.ParseFloat("" + node["BuildPoint"].InnerText);

                     if (node["Max"] != null)
                         Max = Functions.ParseFloat("" + node["Max"].InnerText);

                     if (node["QuantityOnHand"] != null)
                         QuantityOnHand = Functions.ParseFloat("" + node["QuantityOnHand"].InnerText);

                     if (node["AverageCost"] != null)
                         AverageCost = Functions.ParseFloat("" + node["AverageCost"].InnerText);

                     if (node["QuantityOnOrder"] != null)
                         QuantityOnOrder = Functions.ParseFloat("" + node["QuantityOnOrder"].InnerText);

                     if (node["QuantityOnSalesOrder"] != null)
                         QuantityOnSalesOrder = Functions.ParseFloat("" + node["QuantityOnSalesOrder"].InnerText);

                     XmlNodeList __lines = node.SelectNodes("ItemInventoryAssemblyLine");
                     ItemList = new List<ItemInventoryAssemblyLine>();
                     foreach (XmlNode L in __lines)
                     {
                         ItemInventoryAssemblyLine Line = new ItemInventoryAssemblyLine();
                         if (L["ItemInventoryRef"] != null)
                         {
                             Line.ItemIventoryRef = new ItemInventory();
                             Line.ItemIventoryRef.ListID = "" + L["ItemInventoryRef"]["ListID"].InnerText;
                             Line.ItemIventoryRef.FullName = "" + L["ItemInventoryRef"]["FullName"].InnerText;
                         }
                         if (L["Quantity"] != null)
                             Line.Quantity = Functions.ParseFloat("" + L["Quantity"].InnerText);

                         ItemList.Add(Line);
                     }

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
