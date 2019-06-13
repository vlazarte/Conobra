using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartService
{
    public partial class EdditAccess : System.Web.UI.Page
    {
        public Config conf;
        public string id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "GET")
            {
                id = Request.Params["id"];
                Root root = new Root();
                string err = "";
              //  conf = root.getAccess(id, err);


            }
        }
        public string name(string id) 
        {
          
            switch (id)
            {
                case "add":
                    return "Agregar";
                case "edit":
                    return "Editar";
                case "doquery":
                    return "DoQuery";
                case "delet":
                    return "Eliminar";
                case "fetchrow":
                    return "FetchRow";
                case "import":
                    return "Import";  
            }
            return "";
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            Root root = new Root();
            if (Request.HttpMethod == "POST")
            {
                conf = new Config();
                root.cargar();

                DateTime currentDate = DateTime.Now;
                string ID = root.list.Count() + currentDate.Ticks.ToString();

                conf.id = Request.Params["id"];
                conf.token = Request.Params["token"];
                conf.realm = Request.Params["realm"];
                conf.type = Request.Params["type"];
                conf.dbid = Request.Params["dbid"];
                conf.query = Request.Params["query"];
                conf.qid = Int32.Parse(Request.Params["qid"]);
                conf.clist = Request.Params["clist"];
                conf.paramethers = new List<Paramether>();
                for (int i = 1; i <= Int32.Parse(Request.Params["cantidad"]); i++)
                {
                    SmartService.Paramether param = new SmartService.Paramether();
                    var name = Request.Params["name" + i];
                    var fid = Request.Params["fid" + i];
                    var Require = Request.Params["require" + i];
                    if (name == "")
                    {
                        continue;
                    }
                    param.name = name;
                    if (fid != "")
                    {
                        param.fid = Int32.Parse(fid);
                    }
                    if (Require == "true")
                    {
                        param.require = true;
                    }

                    conf.paramethers.Add(param);
                }
                root.editAccess(conf.id, conf);
                string err = "";
              //  conf = root.getAccess(conf.id, err);


            }
        }
    }
}