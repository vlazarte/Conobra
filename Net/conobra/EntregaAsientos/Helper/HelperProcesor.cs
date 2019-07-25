using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Quickbook;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using System.Net;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using SmartQuickbook.Configuration;
using System.Reflection;
using SmartQuickbook.Helper;
using SmartQuickbook.Data;


namespace SmartQuickbook.Helper
{
    class HelperProcesor
    {
        public static string ProcesoEjecutarToQuickBase(Proceso proceso)
        {
            //  Access
            // Procesar Entrada..
            StringBuilder mostrarMensaje = new StringBuilder();


            var ws = new wClient.WService();
            string err = "";
            string resp = ws.doQuery(proceso.entrada.quickbaseAccessToken, null, out err);

            Quickbook.Config.App_Name = Properties.Settings.Default.qbook_app_name;
            Quickbook.Config.File = proceso.file;
            Quickbook.Config.CompaniaDB = proceso.companiaDB;

            Quickbook.Config.IsProduction = true;

            if (err != string.Empty)
            {
                mostrarMensaje.Append("Ejecutando accion Error en el servicio:" + err + Environment.NewLine);
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            List<List<Par>> data = null;
            try
            {
                data = serializer.Deserialize<List<List<Par>>>(resp);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mostrarMensaje.Append("Ejecutando accion Error des serializando:" + ex.Message + Environment.NewLine);

            }
            if (data == null)
            {
                mostrarMensaje.Append("Ejecutando accion data:null" + Environment.NewLine);
            }
            else
            {
                mostrarMensaje.Append("Ejecutando accion cantidad a procesar:" + data.Count + Environment.NewLine);
            }

            if (data != null && data.Count > 0)
            {
                // Inicializar Conexion a Quickbook

                foreach (ProcesoAccion accion in proceso.acciones)
                {
                    //Revisando si tenemos respuesta creada a esta altura
                    if (accion.tipo == "add_quickbook")
                    {

                        mostrarMensaje.Append("Ejecutando accion " + accion.nombre + Environment.NewLine);


                        //creando la coleccion que almacenara las respuestas de la accion.
                        List<ProcesoRespuesta> ConfiguracionRespuestaSave = accion.respuestas.FindAll(d => d.categoria == "Save");
                        List<ProcesoRespuesta> ConfiguracionRespuestaLog = accion.respuestas.FindAll(d => d.categoria == "Log");
                        Dictionary<string, List<string>> RespuestasSave = new Dictionary<string, List<string>>();
                        Dictionary<string, List<string>> RespuestasLog = new Dictionary<string, List<string>>();
                        string llaveQuickbook = "";
                        Dictionary<string, string> llaveQuickbase = new Dictionary<string, string>();

                        //obtener los campos de respuesta para los parametros a enviar
                        //el token sera la llave
                        Dictionary<string, string> fieldNameExternals = HelperTask.GetFieldNameKeyExternal(ConfiguracionRespuestaSave);




                        for (int j = 0; j < data.Count; j++)
                        {

                            // Para Key => Value retornado desde Quickbase ;
                            Hashtable pairs = Generic.getPairValues(data[j]);

                            // Obtener la coleccion Fields => Value que se asignara al Objeto
                            List<string> fieldNames = new List<string>();
                            List<object> fieldValues = new List<object>();
                            string fieldRiquiered = string.Empty;
                            bool Requiered = false;
                            //Get the Values to add to quickbook
                            HelperTask.GetValuesToAdd(accion, pairs, ref fieldNames, ref fieldValues, ref fieldNameExternals, ref llaveQuickbase, ref fieldRiquiered, ref Requiered, ref mostrarMensaje, ref err);

                            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                            string xmlSend = string.Empty;
                            string xmlRecived = string.Empty;


                            if (accion.quickbookTabla != string.Empty)
                            {
                                if (Requiered == false)
                                {

                                    try
                                    {
                                        string fullPath = System.IO.Directory.GetCurrentDirectory();

                                        Assembly testAssembly = Assembly.LoadFile(fullPath + "\\Quickbook.dll");

                                        Type difineType = testAssembly.GetType("Quickbook." + accion.quickbookTabla);

                                        // create instance of class Calculator
                                        object objQuickbookInstance = Activator.CreateInstance(difineType);
                                        Abstract ObjectQuickbook = (Abstract)Generic.SetFields(fieldNames, fieldValues, objQuickbookInstance, ref err);
                                        if (ObjectQuickbook.HasChild) {
                                            if (pairs["CHILDS"] != null) {
                                                string pairsDetails = pairs["CHILDS"].ToString();
                                              
                                                HelperTask.CargadoDetalle(ObjectQuickbook,accion, pairsDetails, ref err);
                                            }                                            
                                        }
                                        if (ObjectQuickbook != null && ObjectQuickbook.AddRecord(ref err, ref xmlSend, ref xmlRecived))
                                        {

                                            if (ConfiguracionRespuestaSave.Count > 0)
                                            {
                                                HelperTask.AddConfigSave(ConfiguracionRespuestaSave, llaveQuickbase, ObjectQuickbook, ref err, ref RespuestasSave);
                                            }                                           
                                        }
                                        else
                                        {

                                            if (err == string.Empty)
                                            {
                                                if (ConfiguracionRespuestaLog.Count > 0)
                                                {
                                                    xmlSend = xmlSend.Replace(",", ".");
                                                    xmlSend = xmlSend.Replace(Environment.NewLine, "");



                                                    xmlRecived = xmlRecived.Replace(",", ".");
                                                    xmlRecived = xmlRecived.Replace(Environment.NewLine, "");

                                                    string accionValue = accion.tipo + accion.nombre;
                                                    accionValue = accionValue.Replace(" ", "");

                                                    string log =accionValue + "," + xmlSend + "," + xmlRecived + "," + "0";
                                                    HelperTask.AddConfigLog(ConfiguracionRespuestaLog, log, ref RespuestasLog);
                                                }
                                            }
                                            else
                                            {

                                                if (ConfiguracionRespuestaLog.Count > 0)
                                                {
                                                    xmlSend = xmlSend.Replace(",", ".");
                                                    xmlSend = xmlSend.Replace(Environment.NewLine, "");
                                                    err = err.Replace(",", ".");
                                                    err = err.Replace(Environment.NewLine, "");
                                                    string accionValue = accion.tipo + accion.nombre;
                                                    accionValue = accionValue.Replace(" ", "");
                                                    string log = accionValue + "," + xmlSend + "," + err + "," + "0";
                                                    HelperTask.AddConfigLog(ConfiguracionRespuestaLog, log, ref RespuestasLog);
                                                }
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {                                        
                                        mostrarMensaje.Append("Error al Procesar los datos: " + ex.Message + Environment.NewLine);
                                        
                                    }

                                }
                                else
                                {

                                    if (ConfiguracionRespuestaLog.Count > 0)
                                    {
                                        string accionValue = accion.tipo + accion.nombre;
                                        accionValue = accionValue.Replace(" ", "");
                                        string log = accionValue + "," + fieldRiquiered + "," + "Se requiere el campo se encuentra vacio o null" + "," + "0";
                                        HelperTask.AddConfigLog(ConfiguracionRespuestaLog, log, ref RespuestasLog);
                                    }
                                }
                            }
                        }
                     mostrarMensaje.Append(   HelperTask.ImportToQuickBase(ConfiguracionRespuestaSave, ConfiguracionRespuestaLog, RespuestasSave, RespuestasLog, ref err));

                    }
                }

                //  Fin conexion a Quickbook
            }
            return mostrarMensaje.ToString();
        }
        public static string ProcesoEjecutarToQuickBook(Proceso proceso)
        {
            //  Access
            // Procesar Entrada..
            StringBuilder mostrarMensaje = new StringBuilder();

            Quickbook.Config.App_Name = Properties.Settings.Default.qbook_app_name;            
            Quickbook.Config.File = proceso.file;
            Quickbook.Config.CompaniaDB = proceso.companiaDB;
            Quickbook.Config.IsProduction = true;
            Quickbook.Config.SaveLogXML = Properties.Settings.Default.qbook_save_log;

            foreach (ProcesoAccion accion in proceso.acciones)
            {
                //Revisando si tenemos respuesta creada a esta altura
                if (accion.tipo == "add_quickbase")
                {

                    mostrarMensaje.Append("Ejecutando accion " + accion.nombre + Environment.NewLine);

                    if (accion.quickbookTabla != string.Empty)
                    {

                        try
                        {
                            string fullPath = System.IO.Directory.GetCurrentDirectory();

                            Assembly testAssembly = Assembly.LoadFile(fullPath + "\\Quickbook.dll");

                            Type difineType = testAssembly.GetType("Quickbook." + accion.quickbookTabla);

                            // create instance of class Calculator
                            object objQuickbookInstance = Activator.CreateInstance(difineType);
                            //que campos
                            string err = string.Empty;
                            List<Abstract> Records=new List<Abstract>();
                            if (proceso.tipoEjecucion == "manual")
                            {//TODO:AComodar GEnerico

                                Records = ((Abstract)objQuickbookInstance).GetRecordsCVS(ref err, proceso.includeSublevel);
                            }
                            else {
                                Records = ((Abstract)objQuickbookInstance).GetRecords(ref err, proceso.includeSublevel);
                            }
                             

                            if (Records.Count > 0)
                            {
                                //obtener el listado de quickbase, separar los que tienen listID
                                string mensaje = HelperProcesor.ProcesarRegistros(proceso, Records);

                                return mensaje;
                            }
                            else
                            {
                                mostrarMensaje.Append("No se encontraron registros para sincronizar" + Environment.NewLine);
                            }


                        }
                        catch (Exception ex)
                        {
                            mostrarMensaje.Append("Error al procesar:" + ex.Message+Environment.NewLine);
                            
                        }

                    }
                }
            }
            return mostrarMensaje.ToString();


        }

        public static string ProcesarRegistros(Proceso proceso, List<Abstract> quickbookRecords)
        {
            //  Access
            // Procesar Entrada..
            StringBuilder mostrarMensaje = new StringBuilder();

            var ws = new wClient.WService();
            string err = "";
            List<List<Par>> data = null;
            if (proceso.entrada.quickbaseAccessToken != string.Empty) {
                string resp = ws.doQuery(proceso.entrada.quickbaseAccessToken, null, out err);
                //El query debe ser los que tengan Quickbook si o si

                if (err != string.Empty)
                {
                    mostrarMensaje.Append("Ejecutando accion Error en el servicio:" + err + Environment.NewLine);
                }

                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
               
                try
                {
                    data = serializer.Deserialize<List<List<Par>>>(resp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    mostrarMensaje.Append("Ejecutando accion Error des serializando:" + ex.Message + Environment.NewLine);

                }
                if (data == null)
                {
                    mostrarMensaje.Append("Ejecutando accion data:null" + Environment.NewLine);
                }
                else
                {
                    mostrarMensaje.Append("Ejecutando accion cantidad a procesar:" + quickbookRecords.Count + Environment.NewLine);
                }
            }
           

            if (data != null && data.Count > 0)
            {
                // Inicializar Conexion a Quickbook

                foreach (ProcesoAccion accion in proceso.acciones)
                {
                    //Revisando si tenemos respuesta creada a esta altura
                    if (accion.tipo == "add_quickbase")
                    {

                        mostrarMensaje.Append("Ejecutando accion " + accion.nombre + Environment.NewLine);

                        Dictionary<string, string> llaveQuickbase = new Dictionary<string, string>();
                        //procesaremos los registros para actualizar                        
                        string resultadoActualizacion = HelperTask.UpdateQuickbookToquickbaseVendorCreated(data, accion, llaveQuickbase, ref err, ref quickbookRecords);
                        mostrarMensaje.Append(resultadoActualizacion);

                        //Existen nuevos registros para crear
                        mostrarMensaje.Append("Cantidad de registros a crear:" + quickbookRecords.Count + Environment.NewLine);
                        if (quickbookRecords.Count > 0)
                        {
                         
                            string resultadoCreados = HelperTask.CreateQuickbookToQuickbaseNewVendor(accion, quickbookRecords, ref err);
                            mostrarMensaje.Append(resultadoCreados);
                        }
                        

                    }
                }
                return mostrarMensaje.ToString();
            }
            else
            {

                mostrarMensaje.Append("Cantidad de registros a crear:" + quickbookRecords.Count);
                if (quickbookRecords.Count > 0) {
                    foreach (ProcesoAccion accion in proceso.acciones)
                    {
                        //Revisando si tenemos respuesta creada a esta altura
                        if (accion.tipo == "add_quickbase")
                        {
                            //Existen nuevos registros de quickbook para quickbase
                            string resultadoCreados = HelperTask.CreateQuickbookToQuickbaseNewVendor(accion, quickbookRecords, ref err);
                            mostrarMensaje.Append(resultadoCreados);
                        }
                    }
                }
                
                return mostrarMensaje.ToString();
            }
        }
    }
}
