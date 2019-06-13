using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Quickbook
{
    public class Functions
    {

        public static float ParseFloat(string val)
        {
            return float.Parse("" + val, NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        public static decimal ParseDecimal(string val)
        {
            return decimal.Parse("" + val, NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        public static string FloatToString(float val)
        {
            return "" + val;
        }

        public static string DateToString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// Convierte algunos caracteres especiales en su equivalente a HTML
        /// para que sea soportado por QBXMLRP2 al momento de convertirlo en
        /// XML
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string htmlEntity( string val )
        {
            if (!(val != "" && val != null))
                return val;

            string c = val;
            
            c = c.Replace("á", "&#225;");
            c = c.Replace("é", "&#233;");
            c = c.Replace("í", "&#237;");
            c = c.Replace("ó", "&#243;");
            c = c.Replace("ú", "&#250;");

            c = c.Replace("Á", "&#193;");
            c = c.Replace("É", "&#201;");
            c = c.Replace("Í", "&#205;");
            c = c.Replace("Ó", "&#211;");
            c = c.Replace("Ú", "&#218;");

            c = c.Replace("ñ", "&#241;");
            c = c.Replace("Ñ", "&#209;");

            c = c.Replace("&", "&amp;");
            c = c.Replace(">", "&gt;");
            c = c.Replace("<", "&lt;");

            return c;
        }

    }
}
