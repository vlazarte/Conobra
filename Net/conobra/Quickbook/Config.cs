using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class Config
    {
        public static bool IsProduction = true;
        public static bool SaveLogXML = false;
        public static string CompaniaDB = "";
        public static Connector quickbooks=null;
        //public static bool IsProduction = false;
        //public static bool SaveXML = false;

       // public static Connector qbook = null;
        public static string App_Name = string.Empty;
        public static string File = string.Empty;
              
    }
}
