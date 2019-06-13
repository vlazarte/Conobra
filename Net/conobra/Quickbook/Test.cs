using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    static class Test
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
                //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Dashboard());
            
            Vars.qb = new Connector("", "");
            Vars.qb.setLocalhost(true);
            //Vars.isLocalhost = true;
            //var t = Vendor.getVendors();
            //var a = Account.getAccounts();
            var xx = Vendor.getVendors();
        }
    }
}


/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EntregaAsientos
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Dashboard());
        }
    }
}*/