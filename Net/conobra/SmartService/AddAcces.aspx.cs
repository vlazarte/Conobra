using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;


namespace SmartService
{
 
    public partial class AddAcces : System.Web.UI.Page
    {
        public bool registrado = false;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            Root root = new Root();
            if (Request.HttpMethod == "POST")
            {
                Config conf = new Config();
                root.cargar();

                DateTime currentDate = DateTime.Now;
                string ID = root.list.Count() + currentDate.Ticks.ToString();

                conf.id = MD5Hash(ID);
                conf.token = Request.Params["token"];
                conf.realm = Request.Params["realm"];
                conf.type = Request.Params["type"];
                conf.dbid = Request.Params["dbid"];
                conf.query = Request.Params["query"];
                if (Request.Params["qid"] != "")
                {
                    conf.qid = Int32.Parse(Request.Params["qid"]);
                }

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
                root.list.Add(conf);
                root.guardar();
                Response.Redirect("ListAccess.aspx");

            }

        }
        public string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}