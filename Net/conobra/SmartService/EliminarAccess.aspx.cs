using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartService
{
    public partial class EliminarAccess : System.Web.UI.Page
    {
  
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "GET")
            {
                Root root = new Root();
                root.cargar();
                string err = "";
                //que acceso eliminara
                bool sw = root.eliminarAccess(Request.Params["id"], out err);
                if (sw == true)
                {
                    Response.Redirect("ListAccess.aspx");

                }
                else
                {
                    Response.Write(err);
                }
            }
        }
    }
}