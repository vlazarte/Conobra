using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;

namespace SmartService
{
    public class Config
    {

        public string id = "";

        public string token = "";
        public string realm = "";
        public string type = "";
        public string dbid = "";
        public string query = "";
        public int qid = -1;
        public string clist = "";

        public List<Paramether> paramethers = null;
        public List<Paramether> response = null;       


        public static Config LoadConfig(string id, ref string err)
        {
            Config config = new Config();
            err = "";
            string pathBase = AppDomain.CurrentDomain.BaseDirectory + "\\config.xml";
            string xml = System.IO.File.ReadAllText(pathBase);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);


            // Obtenemos todos los NODOS access de la configuracion
            XmlNodeList fieldsList = doc.SelectNodes("root/access");
            foreach (XmlNode item in fieldsList)
            {
                if (item.Attributes["id"].Value == id)
                {

                    config.id = item.Attributes["id"].Value;
                    config.token = item["token"].InnerXml;
                    config.realm = item["realm"].InnerXml;
                    config.type = item["type"].InnerXml;
                    config.dbid = item["dbid"].InnerXml;
                    config.query = item["query"].InnerXml;
                    config.qid = Int32.Parse(item["qid"].InnerXml);

                    config.paramethers = new List<Paramether>();
                    XmlNodeList nodo = item.SelectNodes("params/p");
                    if (nodo.Count > 0)
                    {
                        for (int i = 0; i < nodo.Count; i++)
                        {
                            Paramether param = new Paramether();
                            param.name = nodo[i].InnerXml;
                            if (nodo[i].Attributes["fid"] != null && Int32.Parse(nodo[i].Attributes["fid"].Value) != -1)
                            {
                                param.fid = Int32.Parse(nodo[i].Attributes["fid"].Value);
                            }
                            if (nodo[i].Attributes["require"] != null && nodo[i].Attributes["require"].Value == "true")
                            {
                                param.require = true;
                            }
                            config.paramethers.Add(param);
                        }
                    }
                    config.clist = item["clist"].InnerXml;  

                    config.response = new List<Paramether>();
                    nodo = item.SelectNodes("response/p");
                    if (nodo.Count > 0)
                    {
                        for (int i = 0; i < nodo.Count; i++)
                        {
                            Paramether param = new Paramether();
                            param.name = nodo[i].InnerXml;
                            if (nodo[i].Attributes["fid"] != null && Int32.Parse(nodo[i].Attributes["fid"].Value) != -1)
                            {
                                param.fid = Int32.Parse(nodo[i].Attributes["fid"].Value);
                            }
                            config.response.Add(param);
                        }
                    }

                                      
                    
                }


            }
            return config;
        }

        public List<Config> LoadAll(ref string err)
        {
            Config config = null;
            err = "";
            List<Config> list = new List<Config>();
            // Directorio base del proyecto
            string pathBase = AppDomain.CurrentDomain.BaseDirectory + "\\config.xml";
            string xml = System.IO.File.ReadAllText(pathBase);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);


            // Obtenemos todos los NODOS access de la configuracion
            XmlNodeList fieldsList = doc.SelectNodes("root/access");
            foreach (XmlNode item in fieldsList)
            {
                config = new Config();
                config.id = item.Attributes["id"].Value;
                config.token = item["token"].InnerXml;
                config.realm = item["realm"].InnerXml;
                config.type = item["type"].InnerXml;
                config.dbid = item["dbid"].InnerXml;
                config.query = item["query"].InnerXml;
                config.qid = Int32.Parse(item["qid"].InnerXml);

                config.paramethers = new List<Paramether>();
                XmlNodeList nodo = item.SelectNodes("params/p");
                if (nodo.Count > 0)
                {
                    for (int i = 0; i < nodo.Count; i++)
                    {
                        Paramether param = new Paramether();
                        param.name = nodo[i].InnerXml;
                        if (nodo[i].Attributes["fid"] != null && Int32.Parse(nodo[i].Attributes["fid"].Value) != -1)
                        {
                            param.fid = Int32.Parse(nodo[i].Attributes["fid"].Value);
                        }


                        if (nodo[i].Attributes["require"] != null && nodo[i].Attributes["require"].Value == "true")
                        {
                            param.require = true;
                        }
                        config.paramethers.Add(param);
                    }
                }

                config.clist = item["clist"].InnerXml;
                list.Add(config);
            }

            return list;
        }
        public string toXml()
        {
            string xml = "";
            xml += "<access " + "id=\"" + this.id + "\">" + Environment.NewLine;
            xml += "<token>" + this.token + "</token>" + Environment.NewLine;
            xml += "<realm>" + this.realm + "</realm>" + Environment.NewLine;
            xml += "<type>" + this.type + "</type>" + Environment.NewLine;
            xml += "<dbid>" + this.dbid + "</dbid>" + Environment.NewLine;
            xml += "<query>" + this.query + "</query>" + Environment.NewLine;
            xml += "<qid>" + this.qid + "</qid>" + Environment.NewLine;
            xml += "<clist>" + this.clist + "</clist>" + Environment.NewLine;

            xml += "<params>" + Environment.NewLine;

            if (this.paramethers != null && this.paramethers.Count > 0)
            {
                foreach (Paramether DR in this.paramethers)
                {

                    xml += DR.toXml() + Environment.NewLine;
                }
            }
            xml += "</params>" + Environment.NewLine;
            xml += "</access>" + Environment.NewLine;

            return xml;
        }

    }
    public class Paramether
    {
        public int fid { get; set; }
        public string name { get; set; }
        public bool require { get; set; }
        public Paramether()
        {
            fid = -1;
            require = false;

        }
        public string toXml()
        {
            string xml;
            if (this.fid == -1)
            {
                if (this.require == true)
                {
                    xml = "<p require=\"true\">" + this.name + "</p>";
                    return xml;
                }
                xml = "<p>" + this.name + "</p>";
                return xml;
            }
            if (this.require == true)
            {
                xml = "<p fid=\"" + this.fid + "\" require=\"true\" >" + this.name + "</p>";
                return xml;
            }
            else if (this.require == false)
            {
                xml = "<p fid=\"" + this.fid + "\">" + this.name + "</p>";
                return xml;
            }

            return "";


        }


    }
   
}