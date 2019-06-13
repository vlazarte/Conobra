using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartService
{
    public class Root
    {
        public List<Config> list { get; set; }
        public Root()
        {
            list = new List<Config>();
        }

        public Config getAccess(string id,string err)
        {
            err = "";
            Config conf = Config.LoadConfig(id,  ref err);
            return conf;
        }

        internal bool eliminarAccess(string ID,  out string err)
        {
            err = "";
            Config conf = new Config();
            conf = getAccess(ID,  err);

            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].id == ID)
                {
                    try
                    {
                        this.list.RemoveAt(i);

                        guardar();
                        return true;
                    }
                    catch (Exception e)
                    {
                        err = e.Message;
                        return false;

                    }

                }
            }
            err = "elemento no encontrado";
            return false;



        }

        public void editAccess(string id, Config conf)
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].id == id)
                {
                    this.list[i].token = conf.token;
                    this.list[i].realm = conf.realm;
                    this.list[i].type = conf.type;
                    this.list[i].dbid = conf.dbid;
                    this.list[i].query = conf.query;
                    this.list[i].qid = conf.qid;
                    this.list[i].clist = conf.clist;
                    this.list[i].paramethers = conf.paramethers;

                }
            }
            guardar();
        }
        public void cargar()
        {
            Config config = new Config();
            string err = "";
            this.list = config.LoadAll(ref err);
        }
        public void guardar()
        {
            string pathBase = AppDomain.CurrentDomain.BaseDirectory + "\\config.xml";
            string XML = System.IO.File.ReadAllText(pathBase);

            string xml = "";
            xml = "<?xml version=\"1.0\" standalone=\"yes\"?>" + Environment.NewLine;
            xml += "<root>" + Environment.NewLine;
            if (list.Count() > 0)
            {


                for (int i = 0; i < list.Count; i++)
                {

                    xml += list[i].toXml();

                }


            }
            xml += "</root>";

            System.IO.File.WriteAllText(pathBase, xml);


        }

        public bool eliminar(string id)
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].id == id)
                {
                    //      bool s = this.list.Remove();

                }
            }
            return true;
        }
    }
}