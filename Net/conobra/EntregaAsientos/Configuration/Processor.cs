using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmartQuickbook.Configuration
{
    public class Processor
    {
        public List<Proceso> procesos = null;
        
        public string pathFile = "";

        public Processor()
        {
            pathFile = System.AppDomain.CurrentDomain.BaseDirectory + "config.xml";
            procesos = new List<Proceso>();
        }

        public void load()
        {
            procesos = new List<Proceso>();

            string xml = System.IO.File.ReadAllText(pathFile);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNodeList fieldsList = doc.SelectNodes("root/proceso");
            foreach (XmlNode node in fieldsList)
            {
                Proceso P = new Proceso();
                P.id = node.Attributes["id"].Value;
                P.nombre = node["nombre"].InnerText;
                if (node["ejecucion"] != null)
                {
                    P.tipoEjecucion = node["ejecucion"].Attributes["tipo"].Value;
                    if (node["ejecucion"]["intervalo"] != null)
                    {
                        P.tipoIntervalo = node["ejecucion"]["intervalo"].Attributes["tipo"].Value;
                        P.tipoIntervaloValor = node["ejecucion"]["intervalo"].InnerText;
                    }
                }

                XmlNodeList nodoList = node.SelectNodes("acciones/accion");
                P.acciones = new List<ProcesoAccion>();
                foreach (XmlNode pNode in nodoList)
                {
                    ProcesoAccion acccion = new ProcesoAccion();
                    acccion.tipo = pNode.Attributes["tipo"].Value;
                    acccion.nombre = pNode["nombre"].InnerText;
                    if (pNode["quickbook_tabla"] != null)
                        acccion.quickbookTabla = pNode["quickbook_tabla"].InnerText;

                    var paramNodoList = pNode.SelectNodes("parametros/p");
                    acccion.parametros = new List<ProcesoParametros>();
                    foreach (XmlNode paramNode in paramNodoList)
                    {
                        var param1 = new ProcesoParametros(
                                paramNode.Attributes["field"].Value, paramNode.InnerText
                            );

                        if (paramNode.Attributes["type"] != null)
                        {
                            param1.Type = paramNode.Attributes["type"].Value;
                        }
                        if (paramNode.Attributes["key"] != null && paramNode.Attributes["key"].Value == "true")
                        {
                            param1.isKey = true;
                        }
                        if (paramNode.Attributes["required"] != null)
                        {
                            if (paramNode.Attributes["required"].Value == "true")
                                param1.Required = true;
                        }
                        acccion.parametros.Add(param1);
                    }

                    var detailNodoList = pNode.SelectNodes("detail");
                    if (detailNodoList.Count>0)
                    {
                        
                        foreach (XmlNode paramNodeDetails in detailNodoList)
                        {
                            if (paramNodeDetails.SelectNodes("quickbook_tabla").Count > 0) {
                                acccion.quickbookTablaDetalle = paramNodeDetails.SelectNodes("quickbook_tabla")[0].InnerText;
                                var paramNodoListDetail = paramNodeDetails.SelectNodes("parametros/p");


                                acccion.details = new List<ProcesoParametros>();
                                foreach (XmlNode paramNode in paramNodoListDetail)
                                {
                                    var param1 = new ProcesoParametros(
                                            paramNode.Attributes["field"].Value, paramNode.InnerText
                                        );

                                    if (paramNode.Attributes["type"] != null)
                                    {
                                        param1.Type = paramNode.Attributes["type"].Value;
                                    }
                                    if (paramNode.Attributes["key"] != null && paramNode.Attributes["key"].Value == "true")
                                    {
                                        param1.isKey = true;
                                    }
                                    if (paramNode.Attributes["required"] != null)
                                    {
                                        if (paramNode.Attributes["required"].Value == "true")
                                            param1.Required = true;
                                    }
                                    acccion.details.Add(param1);
                                }
                            }
                            
                        }
                        //Obtener La informacion de parametros
                    }
                    


                    var paramNodoListRespuestas = pNode.SelectNodes("respuestas/respuesta");
                    acccion.respuestas = new List<ProcesoRespuesta>();
                    foreach (XmlNode paramNode in paramNodoListRespuestas)
                    {
                        
                            ProcesoRespuesta respuesta = new ProcesoRespuesta();
                            respuesta.tipo = paramNode.Attributes["tipo"].Value;
                            respuesta.categoria = paramNode.Attributes["categoria"].Value;
                            respuesta.quickbaseAccessToken = paramNode["access_token"].InnerText;

                            var paramNodoList2 = paramNode.SelectNodes("parametros/p");
                            respuesta.parametros = new List<ProcesoParametros>();
                            foreach (XmlNode paramNode2 in paramNodoList2)
                            {
                                var param2 = new ProcesoParametros(
                                        paramNode2.Attributes["field"].Value, paramNode2.InnerText
                                    );
                                if (paramNode2.Attributes["key"] != null && paramNode2.Attributes["key"].Value == "true")
                                {
                                    param2.isKey = true;
                                }

                                if (paramNode2.Attributes["type"] != null)
                                {
                                    param2.Type = paramNode2.Attributes["type"].Value;
                                }
                                if (paramNode2.Attributes["required"] != null)
                                {
                                    if (paramNode2.Attributes["required"].Value == "true")
                                        param2.Required = true;
                                }

                                
                                respuesta.parametros.Add(param2);
                            }
                            acccion.respuestas.Add(respuesta);
                        }
                   
                    // Respuesta

                    P.acciones.Add(acccion);
                }

                if (node["entrada"] != null)
                {
                    P.entrada = new ProcesoEntrada();
                    P.entrada.tipo = node["entrada"].Attributes["tipo"].Value;

                    if (node["entrada"]["quickbase"] != null)
                    {
                        P.entrada.quickbaseAccessToken = node["entrada"]["quickbase"]["access_token"].InnerText;
                        nodoList = node["entrada"]["quickbase"].SelectNodes("parametros/p");
                        P.entrada.parametros = new List<ProcesoParametros>();
                        foreach (XmlNode pNode in nodoList)
                        {
                            var param = new ProcesoParametros(
                                    pNode.Attributes["field"].Value, pNode.InnerText
                                );

                            if (pNode.Attributes["type"] != null)
                            {
                                param.Type = pNode.Attributes["type"].Value;
                            }
                            if (pNode.Attributes["required"] != null)
                            {
                                if (pNode.Attributes["required"].Value == "true")
                                    param.Required = true;
                            }
                            P.entrada.parametros.Add(param);
                        }
                    }
                }

                procesos.Add(P);
            }
        }





    }

    public class ProcesoParametros
    {
        public string fieldId;
        public string fieldName;
        public bool isKey;
        public string Type = null;
        public bool Required = false;

        public ProcesoParametros(string fieldId, string fieldName)
        {
            this.fieldName = fieldName;
            this.fieldId = fieldId;
        }
    }

    public class ProcesoEntrada
    {

        public string tipo = "";
        public string quickbaseAccessToken = "";
        public List<ProcesoParametros> parametros;
    }

    public class ProcesoAccion
    {

        public string tipo = "";
        public string nombre = "";
        public string quickbookTabla = "";
        public List<ProcesoParametros> parametros;
        public List<ProcesoParametros> details;
        public string quickbookTablaDetalle = "";
        public List<ProcesoRespuesta> respuestas;
    }

    public class ProcesoRespuesta
    {
        public string tipo = "";
        public string categoria = "";
        public string quickbaseAccessToken = "";
        public List<ProcesoParametros> parametros;
    }

    public class Proceso
    {
        public string id;

        public string nombre;
        public ProcesoEntrada entrada;
        public List<ProcesoAccion> acciones;

        public static string TIPO_EJECUCION_MANUAL = "manual";
        public static string TIPO_EJECUCION_INTERVALO = "intervalo";

        public string tipoEjecucion = "";

        public static string TIPO_INTERVALO_SEGUNDOS = "segundo";
        public static string TIPO_INTERVALO_MINUTOS = "minuto";
        public static string TIPO_INTERVALO_HORAS = "hora";
        public static string TIPO_INTERVALO_DIAS = "dia";

        public string tipoIntervalo = "";
        public string tipoIntervaloValor = "";

        // -----------------------------
        public string estado = "";
        public DateTime? ultimaEjecucion = null;
        public DateTime? siguienteEjecucion = null;

        public ProcessControl controlUI = null;
        public bool enabled = true;
        public bool enEjecucion = false;
    }
}
