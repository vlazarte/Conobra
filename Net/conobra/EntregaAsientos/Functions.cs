using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartQuickbook
{
    class Functions
    {

        public static bool UMExists(string um)
        {
            List<string> lista = new List<string>();
            lista.Add("Sp");
            lista.Add("m");
            lista.Add("kg");
            lista.Add("s");
            lista.Add("A");
            lista.Add("K");
            lista.Add("mol");
            lista.Add("cd");
            lista.Add("mÃ‚Â²");
            lista.Add("mÃ‚Â³");
            lista.Add("m/s");
            lista.Add("m/sÃ‚Â²");
            lista.Add("1/m");
            lista.Add("kg/mÃ‚Â³");
            lista.Add("A/mÃ‚Â²");
            lista.Add("A/m");
            lista.Add("mol/mÃ‚Â³");
            lista.Add("cd/mÃ‚Â²");
            lista.Add("1");
            lista.Add("rad");
            lista.Add("sr");
            lista.Add("Hz");
            lista.Add("N");
            lista.Add("Pa");
            lista.Add("J");
            lista.Add("W");
            lista.Add("C");
            lista.Add("V");
            lista.Add("F");
            lista.Add("Ã¢â€žÂ¦");
            lista.Add("S");
            lista.Add("Wb");
            lista.Add("T");
            lista.Add("H");
            lista.Add("Ã‚Â°C");
            lista.Add("lm");
            lista.Add("lx");
            lista.Add("Bq");
            lista.Add("Gy");
            lista.Add("Sv");
            lista.Add("kat");
            lista.Add("PaÃ‚Â·s");
            lista.Add("NÃ‚Â·m");
            lista.Add("N/m");
            lista.Add("rad/s");
            lista.Add("rad/sÃ‚Â²");
            lista.Add("W/mÃ‚Â²");
            lista.Add("J/K");
            lista.Add("J/(kgÃ‚Â·K)");
            lista.Add("J/kg");
            lista.Add("W/(mÃ‚Â·K)");
            lista.Add("J/mÃ‚Â³");
            lista.Add("V/m");
            lista.Add("C/mÃ‚Â³");
            lista.Add("C/mÃ‚Â²");
            lista.Add("F/m");
            lista.Add("H/m");
            lista.Add("J/mol");
            lista.Add("J/(molÃ‚Â·K)");
            lista.Add("C/kg");
            lista.Add("Gy/s");
            lista.Add("W/sr");
            lista.Add("W/(mÃ‚Â²Ã‚Â·sr)");
            lista.Add("kat/mÃ‚Â³");
            lista.Add("min");
            lista.Add("h");
            lista.Add("d");
            lista.Add("Ã‚Âº");
            lista.Add("Ã‚Â´");
            lista.Add("Ã‚Â´Ã‚Â´");
            lista.Add("L");
            lista.Add("t");
            lista.Add("Np");
            lista.Add("B");
            lista.Add("eV");
            lista.Add("u");
            lista.Add("ua");
            lista.Add("Unid");
            lista.Add("Gal");
            lista.Add("g");
            lista.Add("Km");
            lista.Add("ln");
            lista.Add("cm");
            lista.Add("mL");
            lista.Add("mm");
            lista.Add("Oz");
            lista.Add("Otros");

            foreach (string s in lista)
            {
                if (s == um)
                    return true;
            }
            return false;


        }

        public static string htmlEntity( string val )
        {
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

            return c;
        }

    }
}
