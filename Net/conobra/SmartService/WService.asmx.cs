using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SmartService.Quickbase;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Collections;

namespace SmartService
{
    /// <summary>
    /// Summary description for WService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WService : System.Web.Services.WebService
    {


        private string sendResponse(object data)
        {
            return (new JavaScriptSerializer()).Serialize(data);
        }

        [WebMethod]
        public string doQuery(string id, List<string> paramethers, out string err)
        {
            err = "";
            Config config = new Config();
            Root root = new Root();
            config = root.getAccess(id,  err);

            if (config != null)
            {
                if (config.type == "doquery" || config.type == "fetchrow")
                {
                    return doQuerySimple(config, id, paramethers, out err);
                }
                else if (config.type == "doquery_complex")
                {
                    return doQueryDetails(config, id, paramethers, out err);
                }
            }

            return null;
        }


        private string doQueryDetails(Config config, string id, List<string> paramethers, out string err)
        {
            err = "";

            Connector cnx = new Connector(Constants.username, Constants.password, config.realm);
            cnx.setToken(config.token);

            if (!cnx.Login())
            {
                err = "Authentication failed. " + cnx.getMessage();
                return null;
            }
            // reemplazar XD
            string QUERY = config.query;

            for (int i = 0; i < config.paramethers.Count; i++)
            {
                if (paramethers != null)
                {
                    for (int j = 0; j < paramethers.Count; j++)
                    {

                        if (config.paramethers[i].name == paramethers[j])
                        {
                            if (j + 1 < paramethers.Count)
                            {
                                QUERY = QUERY.Replace("__" + config.paramethers[i].name + "__", paramethers[j + 1]);

                            }

                        }
                    }
                }

            }

            var list = cnx.DoQuery(config.dbid, QUERY, config.clist + ".3", config.qid);

            if (list != null)
            {
                if (list.Count == 0)
                {
                    return sendResponse(new List<List<KeyValuePair<string, string>>>());
                }
                else
                {

                    List<List<KeyValuePair<string, string>>> lista = new List<List<KeyValuePair<string, string>>>();

                    foreach (Record R in list)
                    {
                        List<KeyValuePair<string, string>> obj = new List<KeyValuePair<string, string>>();

                        string QUERY_CHILD = config.details.query;

                        foreach (Paramether P in config.response)
                        {
                            obj.Add(new KeyValuePair<string, string>(P.name, R.getFieldValue(P.fid)));
                            QUERY_CHILD = QUERY_CHILD.Replace("__" + P.name + "__", R.getFieldValue(P.fid));
                        }


                        // Buscamos los hijos
                        {

                            var childs = cnx.DoQuery(config.details.dbid, QUERY_CHILD, config.details.clist + ".3", -1);


                            List<List<KeyValuePair<string, string>>> listaChilds = new List<List<KeyValuePair<string, string>>>();

                            foreach (Record RChild in childs)
                            {
                                List<KeyValuePair<string, string>> objCh = new List<KeyValuePair<string, string>>();

                                foreach (Paramether PCh in config.details.response)
                                {
                                    objCh.Add(new KeyValuePair<string, string>(PCh.name, RChild.getFieldValue(PCh.fid)));
                                }

                                listaChilds.Add(objCh);

                            }

                            string json = sendResponse(listaChilds);
                            // Agregamos todos los hijos como Elemento al padre
                            obj.Add(new KeyValuePair<string, string>("CHILDS", json));

                        }



                        lista.Add(obj);

                    }

                    return sendResponse(lista);
                }
            }


            return null;

        }


        private string doQuerySimple(Config config, string id, List<string> paramethers, out string err)
        {
            err = "";

            Connector cnx = new Connector(Constants.username, Constants.password, config.realm);
            cnx.setToken(config.token);

            if (!cnx.Login())
            {
                err = "Authentication failed. " + cnx.getMessage();
                return null ;
            }
            // reemplazar XD
            string QUERY = config.query;

            for (int i = 0; i < config.paramethers.Count; i++)
            {
                if (paramethers != null)
                {
                    for (int j = 0; j < paramethers.Count; j++)
                    {

                        if (config.paramethers[i].name == paramethers[j])
                        {
                            if (j + 1 < paramethers.Count)
                            {
                                QUERY = QUERY.Replace("__" + config.paramethers[i].name + "__", paramethers[j + 1]);

                            }

                        }
                    }
                }

            }

            var list = cnx.DoQuery(config.dbid, QUERY, config.clist + ".3", config.qid);

            if (list != null)
            {
                if (list.Count == 0)
                {
                    return sendResponse(new List<List<KeyValuePair<string, string>>>());
                }
                else
                {
                    if (config.type == "doquery")
                    {
                        List<List<KeyValuePair<string, string>>> lista = new List<List<KeyValuePair<string, string>>>();

                        foreach (Record R in list)
                        {
                            List<KeyValuePair<string, string>> obj = new List<KeyValuePair<string, string>>();

                            obj.Add(new KeyValuePair<string, string>("RECORD_ID", R.getFieldValue(3)));

                            foreach (Paramether P in config.response)
                            {
                                obj.Add(new KeyValuePair<string, string>(P.name, R.getFieldValue(P.fid)));
                            }

                            lista.Add(obj);

                        }

                        return sendResponse(lista);
                    }
                    else if (config.type == "fetchrow")
                    {

                        List<KeyValuePair<string, string>> obj = null;
                        foreach (Record R in list)
                        {
                            obj = new List<KeyValuePair<string, string>>();

                            obj.Add(new KeyValuePair<string, string>("RECORD_ID", R.getFieldValue(3)));

                            foreach (Paramether P in config.response)
                            {
                                obj.Add(new KeyValuePair<string, string>(P.name, R.getFieldValue(P.fid)));
                            }

                            break;
                        }

                        return sendResponse(obj);
                    }
                }
            }

                   
            return null;

        }

        /// <summary>
        /// MEtodo importFromCsv para agregar varios datos ordenados en formato csv
        /// dependiendo el orden del clist de el xml
        /// </summary>
        /// <param name="id"></param> => identificador del nodo en el cual se encuentra
        /// los datos necesarios para para agregar el csv
        /// <param name="csv"></param>  => string de datos en formato csv para registrar
        /// <param name="err"></param> => mensaje de salida si el procdimiento falla 
        /// esta variable sera contenedora de tal problema
        /// <returns></returns>este metodo retornara true si el proceso se ejecuto correctamente
        /// de lo contrario retornara false y el erro en (err)
        [WebMethod]
        public bool importFromCsv(string id, string csv, out string err)
        {
            err = "";
            //Buscar el nodo access con el id 
            Config config = Config.LoadConfig(id, ref err);
            //comprobar si existe el nodo access
            if (config != null)
            {
                //comprobar si el nodo access es del tipo requerido
                //para proceder con el siguiente paso de lo contrario
                //terminara el processo  
                if (config.type == "import")
                {
                    //Realiza la conexion con quicbase 
                    Connector cnx = new Connector(Constants.username, Constants.password, config.realm);
                    cnx.setToken(config.token);
                    //comprobacion de conexion
                    if (!cnx.Login())
                    {
                        //si no logra ser conectar se retorna el error en la variable (err)
                        err = "Authentication failed. " + cnx.getMessage();
                        return false;
                    }
                    //Realizara el procedimiento requerido
                    bool s = cnx.ImportFromCSV(config.dbid, csv, config.clist);
                    //Dependendiendo del resultado se retornara la confirmacion
                    if (s)
                    {
                        return s;
                    }
                    else
                    {
                        //en caso de algun erro la variable (err) dentra los detalles
                        err = cnx.getMessage();
                        return false;
                    }
                }
                else
                {
                    err = "Tipo Invalido";
                    return false;
                }
            }
            return false;
        }

      

        /// <summary>
        /// addRecord metodo para agregar record a la tabla designada
        /// </summary>
        /// <param name="id"></param> => parametro de identificacodr unico de un nodo access
        /// <param name="list"></param> => lista de datos para ser agregados en el siguiente orden: "NOMBRE","Pablo"
        /// <param name="err"></param> =>mensaje de salida si el procdimiento falla 
        /// esta variable sera contenedora de tal problema
        /// <returns></returns>
        [WebMethod]
        public int addRecord(string id, List<string> list, out string err)
        {
            err = "";
            //obtener el nodo requerido
            Config config = Config.LoadConfig(id,  ref err);
            if (config != null)
            {
                //comprobar el tipo requerido para el proceso
                if (config.type == "add")
                {
                    //realizar conexion 
                    Connector cnx = new Connector(Constants.username, Constants.password, config.realm);
                    cnx.setToken(config.token);
                    //confirmar conexion
                    if (!cnx.Login())
                    {
                        err = "Authentication failed. " + cnx.getMessage();
                        return -1;
                    }
                    //Crea lista para agregar los datos correspondientes 
                    List<Intuit.QuickBase.Core.IField> fieldsRec = new List<Intuit.QuickBase.Core.IField>();
                    //ciclo para recorrer lo parametros del proceso
                    for (int i = 0; i < config.paramethers.Count; i++)
                    {
                        //Si el parametro es requerido sera obligario que 
                        //la lista tenga un dato correspondiente
                        if (config.paramethers[i].require == true)
                        {
                            int ctrl = 0;
                            for (int j = 0; j < list.Count; j++)
                            {
                                if (config.paramethers[i].name == list[j])
                                {
                                    if (j + 1 < list.Count)
                                    {
                                        fieldsRec.Add(new Intuit.QuickBase.Core.Field(config.paramethers[i].fid, list[j + 1]));
                                        ctrl++;
                                    }

                                }
                            }
                            if (ctrl == 0)
                            {
                                err = "El parametro " + config.paramethers[i].name + "es requerido";
                                return -1;
                            }
                        }
                        //ciclo para el llenado de los parametros no necesariamente requerido
                        for (int j = 0; j < list.Count; j++)
                        {

                            if (config.paramethers[i].name == list[j])
                            {
                                if (j + 1 < list.Count)
                                {
                                    fieldsRec.Add(new Intuit.QuickBase.Core.Field(config.paramethers[i].fid, list[j + 1]));
                                }
                            }
                        }
                    }
                    //si hay uno o mas datos para agregar se realiara el proceso add
                    if (fieldsRec.Count > 0)
                    {
                        int Rid = cnx.AddRecord(config.dbid, fieldsRec);
                        if (Rid > 0)
                        {
                            return Rid;
                        }
                        else
                        {
                            err = cnx.getMessage();
                        }
                    }
                    else
                    {
                        err = "Almenos uno de los datos se debe enviar";
                        return -1;
                    }
                }
                else
                {
                    err = "Tipo Invalido";
                }
            }
            return -1;
        }

        /// <summary>
        /// Metodo creado para Editar un Record Existente con los parametros requeridos obligatoriamente
        /// como tambien los campos no requeridos
        /// </summary>
        /// <param name="id"></param> => parametro de identificacodr unico de un nodo access
        /// <param name="list"></param> => lista de pararametros conformada por el campo y el valor a editar: ej. "NOMBRE","Roy Rivera"
        /// <param name="recordID"></param> => parametro del campo RecorID para 
        /// <param name="err"></param> =>mensaje de salida si el procdimiento falla 
        /// esta variable sera contenedora de tal problema
        /// <returns></returns> => este metodo retornara true si el proceso se ejecuto correctamente
        /// de lo contrario retornara false y el erro en (err)
        [WebMethod]
        public bool edditRecord(string id, List<string> list, int recordID, out string err)
        {
            err = "";
            //obtenenmos el nodo access por su id
            Config config = Config.LoadConfig(id, ref err);
            if (config != null)
            {
                //comprobar si el nodo es del tipo correcto
                if (config.type == "edit")
                {
                    //realizar conexion con quickbase
                    Connector cnx = new Connector(Constants.username, Constants.password, config.realm);
                    cnx.setToken(config.token);
                    //verificar si la conexion se realizo correctamente
                    if (!cnx.Login())
                    {
                        //si la conexion falla se retornara el error en la variable (err)
                        err = "Authentication failed. " + cnx.getMessage();
                        return false;
                    }
                    //***variable creada para guardar los datos que seran editados
                    List<Intuit.QuickBase.Core.IField> fieldsRec = new List<Intuit.QuickBase.Core.IField>();
                    for (int i = 0; i < config.paramethers.Count; i++)
                    {
                        //si algun parametro es requerido tiene que estar en la lista list de lo contrario
                        //No procedera a realizar el proceso
                        if (config.paramethers[i].require == true)
                        {
                            bool exist = false;
                            for (int j = 0; j < list.Count; j++)
                            {
                                if (config.paramethers[i].name == list[j])
                                {
                                    if (j + 1 < list.Count)
                                    {
                                        exist = true;
                                        fieldsRec.Add(new Intuit.QuickBase.Core.Field(config.paramethers[i].fid, list[j + 1]));
                                    }
                                }
                            }
                            //si el registro requerido no se encuentra termina el proceso
                            //el erro es guardado en la variable (err)
                            if (exist == false)
                            {
                                err = "El campo es " + config.paramethers[i].name + " requerido";
                                return false;
                            }
                            continue;
                        }
                        //en caso de no ser requerido el campo se realiza a buscar sus datos
                        //pero si no son encontrados no sera un error
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (config.paramethers[i].name == list[j])
                            {
                                if (j + 1 < list.Count)
                                {
                                    fieldsRec.Add(new Intuit.QuickBase.Core.Field(config.paramethers[i].fid, list[j + 1]));
                                }
                            }
                        }
                    }
                    //Si la variable fieldsRec tiene elementos se procedera a realizar la consulta
                    if (fieldsRec.Count > 0)
                    {
                        //ejecucion del Proceso con todos los datoa  editar en el fieldsRec
                        if (cnx.EditRecord(config.dbid, recordID, fieldsRec))
                        {
                            return true;
                        }
                        else
                        {
                            err = "Error al editar el registro. " + cnx.getMessage();
                            return false;
                        }
                    }
                    else
                    {

                        err = "Almenos uno de los datos se debe enviar";
                        return false;
                    }
                }
                else
                {
                    err = "Tipo invalido";
                }
            }

            return false;

        }

    }
}
