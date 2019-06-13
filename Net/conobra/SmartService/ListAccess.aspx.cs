using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartService
{
    public partial class ListAccess : System.Web.UI.Page
    {
        public Root root = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Config conf = new Config();
            this.root = new Root();
            root.cargar();

        }
    }
}