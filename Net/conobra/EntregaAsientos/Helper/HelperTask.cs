using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartQuickbook.Configuration;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;
using System.IO;
using Quickbook;
using System.Web.Script.Serialization;

namespace SmartQuickbook.Helper
{
    public static class HelperTask
    {

        public static string[] GetFieldNamesResponse(List<ProcesoParametros> parametros)
        {
            string[] fieldResponseNames = new string[parametros.Count];

            for (int i = 0; i < parametros.Count; i++)
            {
                //obtener los field que nos interesan registrar en quickbase
                fieldResponseNames[i] = parametros[i].fieldName;
            }
            return fieldResponseNames;
        }

        public static Dictionary<string, string> GetFieldNameKeyExternal(List<ProcesoRespuesta> Respuestas)
        {
            Dictionary<string, string> fieldResponseNames = new Dictionary<string, string>();

            foreach (var Respuesta in Respuestas)
            {
                String fieldResponseName = string.Empty;

                foreach (var parametro in Respuesta.parametros)
                {
                    //obtener los field que nos interesan registrar en quickbase
                    if (parametro.isKey)
                    {
                        fieldResponseName = parametro.fieldName;
                        break;
                    }
                }
                //Si no encuentra colocara vacio
                fieldResponseNames.Add(Respuesta.quickbaseAccessToken, fieldResponseName);
            }


            return fieldResponseNames;
        }

        public static List< string> GetFieldNameKeyExternalDetalle(List<ProcesoParametros> detalle)
        {
            List< string> fieldResponseNames = new List< string>();

           
                String fieldResponseName = string.Empty;

                foreach (var parametro in detalle)
                {
                    //obtener los field que nos interesan registrar en quickbase
                    if (parametro.isKey)
                    {
                        fieldResponseName = parametro.fieldName;
                        fieldResponseNames.Add(fieldResponseName);
                    }
                }
                //Si no encuentra colocara vacio
                
            
            return fieldResponseNames;
        }

        public static object ConvertType(string value, string type)
        {



            if (type == "DateTime")
            {

                return getDateValue(value);
            }
            if (type == "Double")
            {                                
                System.Globalization.CultureInfo myInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
                return  Double.Parse(value, myInfo);

                
            }
            return null;


        }
        public static object ConvertType(string fieldPropertyCurrency, object value, string type, ref string error)
        {


            try
            {
                string fullPath = System.IO.Directory.GetCurrentDirectory();

                Assembly testAssembly = Assembly.LoadFile(fullPath + "\\Quickbook.dll");

                Type difineType = testAssembly.GetType("Quickbook." + type);

                // create instance of class Calculator
                object calcInstance = Activator.CreateInstance(difineType);

                List<string> fieldNames = new List<string>();
                fieldNames.Add(fieldPropertyCurrency);


                List<object> fieldValues = new List<object>();

                fieldValues.Add(value);


                string err = "";
                var objectValue = Generic.SetFields(fieldNames, fieldValues, calcInstance, ref err);
                error = err;
                return objectValue;


            }
            catch (Exception ex)
            {
                error = ex.Message;
                throw new Exception("Error al conectar a Quickbook: " + ex.Message);

            }

        }

        public static string getObjectValue(string fieldProperty, object obj, string fieldValue, ref string err)
        {


            PropertyInfo _propertyInfo = obj.GetType().GetProperty(fieldProperty);
            if (_propertyInfo == null)
            {
                err = "Property with names " + fieldProperty + " does not exists in " + obj.GetType().ToString();
                return null;
            }
            object objectValue = _propertyInfo.GetValue(obj, null);
            string value = string.Empty;
            if (objectValue.GetType().GetProperties().Count() > 1)
            {
                PropertyInfo _propertyInfo2 = objectValue.GetType().GetProperty(fieldValue);
                if (_propertyInfo2 == null)
                {
                    err = "Property with names " + fieldValue + " does not exists in " + obj.GetType().ToString();
                    return null;
                }
                value = Convert.ToString(_propertyInfo2.GetValue(objectValue, null));
            }
            else
            {
                value = Convert.ToString(_propertyInfo.GetValue(obj, null));
            }

            return value;
        }

        private static DateTime getDateValue(String value)
        {
            var milliseconds = value;
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return new DateTime(date.Ticks + (Int64.Parse(milliseconds) * TimeSpan.TicksPerMillisecond));
        }
        public static void AddConfigSave(List<ProcesoRespuesta> ConfiguracionRespuestaSave, Dictionary<string, string> llaveQuickbase, Abstract ObjectQuickbook, ref string err, ref  Dictionary<string, List<string>> RespuestasSave)
        {
            try
            {
                foreach (var respuesta in ConfiguracionRespuestaSave)
                {
                    string KeyQuickBase = string.Empty;
                    if (llaveQuickbase.ContainsKey(respuesta.quickbaseAccessToken))
                    {
                        KeyQuickBase = llaveQuickbase[respuesta.quickbaseAccessToken];
                    }

                  
                     String csv = Generic.GetFieldsToCsv(respuesta.parametros, KeyQuickBase, ObjectQuickbook, ref err);
                     if (csv != string.Empty)
                     {
                         List<string> responses = null;

                         if (RespuestasSave.ContainsKey(respuesta.quickbaseAccessToken))
                         {
                             responses = RespuestasSave[respuesta.quickbaseAccessToken];
                         }
                         else
                         {
                             responses = new List<string>();
                         }


                         responses.Add(csv);
                         if (RespuestasSave.ContainsKey(respuesta.quickbaseAccessToken))
                         {
                             RespuestasSave[respuesta.quickbaseAccessToken] = responses;
                         }
                         else
                         {
                             RespuestasSave.Add(respuesta.quickbaseAccessToken, responses);
                         }
                     }

                }
            }
            catch (Exception ex)
            {
                
                err += ex.Message; 
            }
            
        }
        public static void AddConfigLog(List<ProcesoRespuesta> ConfiguracionRespuestaLog, String log, ref  Dictionary<string, List<string>> RespuestasLog)
        {
            foreach (var respuestaLog in ConfiguracionRespuestaLog)
            {

                List<string> responses = null;

                if (RespuestasLog.ContainsKey(respuestaLog.quickbaseAccessToken))
                {
                    responses = RespuestasLog[respuestaLog.quickbaseAccessToken];
                }
                else
                {
                    responses = new List<string>();
                }


                responses.Add(log);
                if (RespuestasLog.ContainsKey(respuestaLog.quickbaseAccessToken))
                {
                    RespuestasLog[respuestaLog.quickbaseAccessToken] = responses;
                }
                else
                {
                    RespuestasLog.Add(respuestaLog.quickbaseAccessToken, responses);
                }
            }
        }
        public static void ImportToQuickBase(List<ProcesoRespuesta> ConfiguracionRespuestaSave, List<ProcesoRespuesta> ConfiguracionRespuestaLog, Dictionary<string, List<string>> RespuestasSave, Dictionary<string, List<string>> RespuestasLog, ref string err)
        {
            //Realizar el import
            wClient.WService ws = new wClient.WService();

            if (RespuestasSave.Count > 0)
            {
                foreach (var respuesta in ConfiguracionRespuestaSave)
                {
                    if (respuesta.tipo == "import_quickbase")
                    {
                        List<string> Responses = RespuestasSave[respuesta.quickbaseAccessToken];
                        string csv = string.Join(Environment.NewLine, Responses.ToArray());
                        bool import = ws.importFromCsv(respuesta.quickbaseAccessToken, csv, out err);

                    }
                }
            }

            if (ConfiguracionRespuestaLog != null && RespuestasLog != null && RespuestasLog.Count > 0)
            {

                foreach (var respuestaLog in ConfiguracionRespuestaLog)
                {
                    if (respuestaLog.tipo == "import_quickbase")
                    {
                        List<string> Responses = RespuestasLog[respuestaLog.quickbaseAccessToken];
                        string csv = string.Join(Environment.NewLine, Responses.ToArray());
                        bool import = ws.importFromCsv(respuestaLog.quickbaseAccessToken, csv, out err);

                    }
                }
            }
        }
        public static void GetValuesToAdd(ProcesoAccion accion, Hashtable pairs, ref List<string> fieldNames, ref  List<object> fieldValues, ref Dictionary<string, string> fieldNameExternals, ref Dictionary<string, string> llaveQuickbase, ref string fieldRiquiered, ref bool Requiered, ref StringBuilder mostrarMensaje, ref string err)
        {
            try
            {
                for (int i = 0; i < accion.parametros.Count; i++)
                {


                    bool isValue = false;
                    if (!accion.parametros[i].isKey)
                    {
                        if (accion.parametros[i].fieldId.ToString().Contains("/"))//campo
                        { //Elemento compuesto 

                            string[] field = accion.parametros[i].fieldId.ToString().Split('/');
                            string fieldPropertyCustomer = field[0];//parenref
                            string fieldPropertyCurrency = field[1];//listID

                            if (fieldPropertyCustomer == "Custom")
                            {

                                fieldNames.Add(fieldPropertyCustomer);
                                fieldValues.Add(pairs[accion.parametros[i].fieldName].ToString());
                                isValue = true;
                            }
                            else
                            {

                                fieldNames.Add(fieldPropertyCustomer);

                                if (accion.parametros[i].Type != null)
                                {

                                    string value = pairs[fieldPropertyCustomer].ToString();
                                    if (accion.parametros[i].Type == "AdditionalContactRef")
                                    {
                                        fieldValues.Add(value);
                                        isValue = true;

                                    }
                                    else
                                    {

                                        if (value != string.Empty)
                                        {

                                            var newValueObject = HelperTask.ConvertType(fieldPropertyCurrency, value, accion.parametros[i].Type, ref err);
                                            if (err != string.Empty)
                                            {

                                                mostrarMensaje.Append("Convirtiendo valores Error:" + err + Environment.NewLine);

                                            }

                                            fieldValues.Add(newValueObject);
                                            isValue = true;
                                        }
                                        else
                                        {
                                            if (accion.parametros[i].Required)
                                            {

                                                Requiered = true;
                                                fieldRiquiered = accion.parametros[i].fieldId;
                                                break;
                                            }
                                            else
                                            {
                                                fieldValues.Add(null);
                                                isValue = true;
                                            }


                                        }
                                    }

                                }
                                else
                                {
                                    fieldValues.Add(pairs[accion.parametros[i].fieldName]);
                                    isValue = true;
                                }

                            }

                        }
                        else
                        {
                            fieldNames.Add(accion.parametros[i].fieldId);
                        }
                        if (isValue == false)
                        {
                            if (pairs.ContainsKey(accion.parametros[i].fieldName))
                            {

                                if (pairs[accion.parametros[i].fieldName] != null && accion.parametros[i].Type != null)
                                {

                                    string value = pairs[accion.parametros[i].fieldName].ToString();
                                    if (value != string.Empty)
                                    {
                                        var newDate = HelperTask.ConvertType(value, accion.parametros[i].Type);
                                        fieldValues.Add(newDate);
                                    }
                                    else
                                    {
                                        fieldValues.Add(null);
                                    }

                                }
                                else
                                {
                                    fieldValues.Add(pairs[accion.parametros[i].fieldName]);
                                }


                            }
                            else
                            {
                                fieldValues.Add(null);
                            }
                        }

                    }
                    else
                    {  //si es el campo llave debe ser igual con fieldNameExternal


                        var value = Array.Find(fieldNameExternals.ToArray(), element => element.Value.Equals(accion.parametros[i].fieldName));

                        if (value.Value != string.Empty && pairs[value.Value] != null)
                        {
                            //valor de la llave externa
                            if (llaveQuickbase.ContainsKey(value.Key))
                            {
                                llaveQuickbase[value.Key] = pairs[value.Value].ToString();
                            }
                            else
                            {
                                llaveQuickbase.Add(value.Key, pairs[value.Value].ToString());
                            }

                        }

                    }


                }
            }
            catch (Exception e)
            {
                err = e.Message;

            }


        }
        public static void GetValuesToAddAndUpdate(ProcesoAccion accion, Hashtable pairs, ref List<string> fieldNames, ref  List<object> fieldValues, ref Dictionary<string, string> fieldNameExternals, ref Dictionary<string, string> fieldNameExternalsCreate, ref Dictionary<string, string> llaveQuickbase, ref string fieldRiquiered, ref bool Requiered, ref StringBuilder mostrarMensaje, ref string err)
        {


            for (int i = 0; i < accion.parametros.Count; i++)
            {


                bool isValue = false;
                if (!accion.parametros[i].isKey)
                {
                    if (accion.parametros[i].fieldId.ToString().Contains("/"))//campo
                    { //Elemento compuesto 

                        string[] field = accion.parametros[i].fieldId.ToString().Split('/');
                        string fieldPropertyCustomer = field[0];//parenref
                        string fieldPropertyCurrency = field[1];//listID
                        fieldNames.Add(fieldPropertyCustomer);

                        if (accion.parametros[i].Type != null)
                        {

                            string value = pairs[fieldPropertyCustomer].ToString();
                            if (value != string.Empty)
                            {



                                var newValueObject = HelperTask.ConvertType(fieldPropertyCurrency, value, accion.parametros[i].Type, ref err);
                                if (err != string.Empty)
                                {

                                    mostrarMensaje.Append("Convirtiendo valores Error:" + err + Environment.NewLine);

                                }

                                fieldValues.Add(newValueObject);
                                isValue = true;
                            }
                            else
                            {
                                if (accion.parametros[i].Required)
                                {

                                    Requiered = true;
                                    fieldRiquiered = accion.parametros[i].fieldId;
                                    break;
                                }
                                else
                                {
                                    fieldValues.Add(null);
                                    isValue = true;
                                }


                            }

                        }
                        else
                        {
                            fieldValues.Add(pairs[accion.parametros[i].fieldName]);
                            isValue = true;
                        }
                    }
                    else
                    {
                        fieldNames.Add(accion.parametros[i].fieldId);
                    }
                    if (isValue == false)
                    {
                        if (pairs.ContainsKey(accion.parametros[i].fieldName))
                        {

                            if (pairs[accion.parametros[i].fieldName] != null && accion.parametros[i].Type != null)
                            {

                                string value = pairs[accion.parametros[i].fieldName].ToString();
                                if (value != string.Empty)
                                {
                                    var newDate = HelperTask.ConvertType(value, accion.parametros[i].Type);
                                    fieldValues.Add(newDate);
                                }
                                else
                                {
                                    fieldValues.Add(null);
                                }

                            }
                            else
                            {
                                fieldValues.Add(pairs[accion.parametros[i].fieldName]);
                            }


                        }
                        else
                        {
                            fieldValues.Add(null);
                        }
                    }

                }
                else
                {  //si es el campo llave debe ser igual con fieldNameExternal


                    var value = Array.Find(fieldNameExternals.ToArray(), element => element.Value.Equals(accion.parametros[i].fieldName));

                    if (value.Value != string.Empty && pairs[value.Value] != null)
                    {
                        //valor de la llave externa
                        if (llaveQuickbase.ContainsKey(value.Key))
                        {
                            llaveQuickbase[value.Key] = pairs[value.Value].ToString();
                        }
                        else
                        {
                            llaveQuickbase.Add(value.Key, pairs[value.Value].ToString());
                        }

                    }

                    var valueCreate = Array.Find(fieldNameExternalsCreate.ToArray(), element => element.Value.Equals(accion.parametros[i].fieldName));

                    if (valueCreate.Value != string.Empty && pairs[valueCreate.Value] != null)
                    {
                        //valor de la llave externa
                        if (llaveQuickbase.ContainsKey(valueCreate.Key))
                        {
                            llaveQuickbase[valueCreate.Key] = pairs[valueCreate.Value].ToString();
                        }
                        else
                        {
                            llaveQuickbase.Add(valueCreate.Key, pairs[valueCreate.Value].ToString());
                        }

                    }

                }


            }
        }
        public static bool ImportToQuickBaseClassFromQuickBookCVS(ref string err, ref string cvs)
        {
            string keyImportToQuickbaseCreate = "3a847e17ba9e87559ad0bb71fac10015";//"3a847e17ba9e87559ad0bb71fac10008";
            string keyImportToQuickbaseUpdate = "0823871b4f16ca3cd7d6f6259690c002";
            string keyQueryFromQuickbase = "3a847e17ba9e87559ad0bb71fac10009";


            Class ClassImport = new Class();


            Quickbook.Config.App_Name = Properties.Settings.Default.qbook_app_name;
            Quickbook.Config.File = Properties.Settings.Default.qbook_file;
            Quickbook.Config.IsProduction = true;
            List<Abstract> classToCreate = ClassImport.GetRecordsCVS(ref err);

            Dictionary<string, string> registrados = registrosSincronizados(keyQueryFromQuickbase, ref err);

            bool registroExitoso = false;
            if (registrados.Count > 0)
            {
                registroExitoso = UpdateQuickbookToQuickbaseClass(registrados, ref classToCreate, keyImportToQuickbaseUpdate, ref err);
                //quedan registros nuevos por adicionar?
                if (classToCreate.Count > 0)
                {
                    registroExitoso = CreateQuickbookToQuickbaseNewsClass(classToCreate, keyImportToQuickbaseCreate, ref  err);
                }
                else
                {
                    err = "No se encontraron registros nuevos para Registrar";
                }
            }
            else
            {
                if (classToCreate.Count > 0)
                {
                    registroExitoso = CreateQuickbookToQuickbaseNewsClass(classToCreate, keyImportToQuickbaseCreate, ref  err);
                }
                else
                {
                    err = "No se encontraron registros nuevos para Registrar";
                }
            }

            return registroExitoso;

        }
        private static bool UpdateQuickbookToQuickbaseClass(Dictionary<string, string> registrados, ref  List<Abstract> classToCreate, string keyImportToQuickbaseUpdate, ref string err)
        {
            bool registroExitoso = false;
            string cvs = string.Empty;
            var ws = new wClient.WService();
            Dictionary<string, Class> classToSincronize = new Dictionary<string, Class>();
            foreach (var item in registrados)
            {
                Class toUpdate = (Class)classToCreate.Find(d => ((Class)d).ListID == item.Key);
                if (toUpdate != null)
                {
                    classToSincronize.Add(item.Value, toUpdate);
                    classToCreate.Remove(toUpdate);
                }
            }

            //Actualizar
            List<string> quickbookListClass = new List<string>();
            if (classToSincronize.Count > 0)
            {
                foreach (var item in classToSincronize)
                {
                    if (item.Value.ParentRef == null)
                    {
                        quickbookListClass.Add(item.Key + "," + item.Value.ListID + "," + item.Value.FullName + "," + "");
                    }
                    else
                    {
                        quickbookListClass.Add(item.Key + "," + item.Value.ListID + "," + item.Value.FullName + "," + item.Value.ParentRef.ListID);
                    }
                }
            }


            if (quickbookListClass.Count > 0)
            {

                cvs = string.Join(Environment.NewLine, quickbookListClass.ToArray());
                registroExitoso = ws.importFromCsv(keyImportToQuickbaseUpdate, cvs, out err);
            }
            else
            {
                err = "No se encontraron registros para actualizar";
            }

            return registroExitoso;
        }
        private static bool CreateQuickbookToQuickbaseNewsClass(List<Abstract> classToCreate, string keyImportToQuickbaseCreate, ref string err)
        {
            var ws = new wClient.WService();
            List<string> quickbookListClass = new List<string>();
            string cvs = string.Empty;
            bool registroExitoso = false;

            if (classToCreate.Count > 0)
            {
                foreach (var item in classToCreate)
                {
                    Class item2 = item as Class;
                    if (item2.ParentRef == null)
                    {
                        quickbookListClass.Add(item2.ListID + "," + item2.FullName + "," + "");
                    }
                    else
                    {
                        quickbookListClass.Add(item.ListID + "," + item2.FullName + "," + item2.ParentRef.ListID);
                    }
                }
            }

            if (quickbookListClass.Count > 0)
            {
                cvs = string.Join(Environment.NewLine, quickbookListClass.ToArray());
                registroExitoso = ws.importFromCsv(keyImportToQuickbaseCreate, cvs, out err);
            }
            else
            {
                err = "No se encontraron registros de class para crear";
            }

            return registroExitoso;
        }
        public static string ImportToQuickBaseVendedoresFromQuickBookCVS(ref string err, ref string cvs)
        {
             string keyImportToQuickbaseCreate = "3a847e17ba9e87559ad0bb71fac10010"; 
             Vendor VendorImport = new Vendor();
             var ws = new wClient.WService();
          

             Quickbook.Config.App_Name = Properties.Settings.Default.qbook_app_name;
             Quickbook.Config.File = Properties.Settings.Default.qbook_file;
             Quickbook.Config.IsProduction = true;
             List<Abstract> vendorToCreate = VendorImport.GetRecordsCVS(ref err);
             

             if (vendorToCreate.Count > 0)
             {
                
                 //obtener el listado de quickbase, separar los que tienen listID
             //    string mensaje = HelperProcesor.ProcesarRegistros(proceso, vendorToCreate);

                 return string.Empty;
             }
             else
             {
                 err = "No se encontraron registros para sincronizar";
                 return err;
             }
        }
        private static bool UpdateQuickbookToQuickbaseVendor(Dictionary<string, string> registrados, ref List<Vendor> vendorToCreate)
        {
            /*  //ya registrados actualizar
              foreach (var item in registrados)
              {
                  Class toUpdate = classToCreate.Find(d => d.ListID == item.Key);
                  if (toUpdate != null)
                  {
                      classToSincronize.Add(item.Value, toUpdate);
                      classToCreate.Remove(toUpdate);
                  }

              }

              //Actualizar
              List<string> quickbookListClass = new List<string>();
              if (classToSincronize.Count > 0)
              {
                  foreach (var item in classToSincronize)
                  {
                      if (item.Value.ParentRef == null)
                      {
                          quickbookListClass.Add(item.Key + "," + item.Value.ListID + "," + item.Value.FullName + "," + "");
                      }
                      else
                      {
                          quickbookListClass.Add(item.Key + "," + item.Value.ListID + "," + item.Value.FullName + "," + item.Value.ParentRef.ListID);
                      }
                  }
              }


              if (quickbookListClass.Count > 0)
              {

                  cvs = string.Join(Environment.NewLine, quickbookListClass.ToArray());
                  registroExitoso = ws.importFromCsv(keyImportToQuickbaseUpdate, cvs, out err);
              }
              quickbookListClass = new List<string>();

              if (classToCreate.Count > 0)
              {
                  foreach (var item in classToCreate)
                  {
                      if (item.ParentRef == null)
                      {
                          quickbookListClass.Add(item.ListID + "," + item.FullName + "," + "");
                      }
                      else
                      {
                          quickbookListClass.Add(item.ListID + "," + item.FullName + "," + item.ParentRef.ListID);
                      }
                  }
              }

              if (quickbookListClass.Count > 0)
              {

                  cvs = string.Join(Environment.NewLine, quickbookListClass.ToArray());
                  registroExitoso = ws.importFromCsv(keyImportToQuickbaseCreate, cvs, out err);
              }*/
            return false;
        }
        private static bool CreateQuickbookToQuickbaseNewsVendor()
        {
            return false;
        }
        private static bool UpdateQuickbookVendorQuickBaseCode()
        {
            return false;
        }

        private static Dictionary<string, string> registrosSincronizados(string keyCondifiguracion, ref string err)
        {
            Dictionary<string, string> registroSincronizados = new Dictionary<string, string>();

            var ws = new wClient.WService();

            string resp = ws.doQuery(keyCondifiguracion, null, out err);
            if (err != string.Empty)
            {
                err = "Ejecutando accion Error en el servicio:" + err;
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
                err = "Ejecutando accion Error des serializando:" + ex.Message;

            }
            if (data == null)
            {
                err = "Ejecutando accion data:null";
            }


            if (data != null && data.Count > 0)
            {
                //obtener las llaves generadas de quickbase

                for (int j = 0; j < data.Count; j++)
                {
                    Hashtable pairs = Generic.getPairValues(data[j]);
                    registroSincronizados.Add(pairs["ListID"].ToString(), pairs["RecordID"].ToString());

                }

                //  Fin conexion a Quickbook
            }
            return registroSincronizados;


        }
        private static Dictionary<string, string> registrosParaEmparejar(string keyCondifiguracion, ref string err)
        {
            Dictionary<string, string> registroSincronizados = new Dictionary<string, string>();

            var ws = new wClient.WService();

            string resp = ws.doQuery(keyCondifiguracion, null, out err);
            if (err != string.Empty)
            {
                err = "Ejecutando accion Error en el servicio:" + err;
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
                err = "Ejecutando accion Error des serializando:" + ex.Message;

            }
            if (data == null)
            {
                err = "Ejecutando accion data:null";
            }


            if (data != null && data.Count > 0)
            {
                //obtener las llaves generadas de quickbase

                for (int j = 0; j < data.Count; j++)
                {
                    Hashtable pairs = Generic.getPairValues(data[j]);
                    registroSincronizados.Add(pairs["Nombre"].ToString(), pairs["RecordID"].ToString());

                }

                //  Fin conexion a Quickbook
            }
            return registroSincronizados;


        }

        public static string UpdateQuickbookToquickbaseVendorCreated(List<List<Par>> data, ProcesoAccion accion, Dictionary<string, string> llaveQuickbase, ref string err, ref List<Abstract> quickbookRecords)
        {
            //obtener los campos de respuesta para los parametros a enviar
            //el token sera la llave
            List<ProcesoRespuesta> ConfiguracionRespuestaSave = accion.respuestas.FindAll(d => d.categoria == "Save");
            List<ProcesoRespuesta> ConfiguracionRespuestaLog = accion.respuestas.FindAll(d => d.categoria == "Log");
            Dictionary<string, List<string>> RespuestasSave = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> RespuestasLog = new Dictionary<string, List<string>>();

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
                StringBuilder mostrarMensaje = new StringBuilder();
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

                            if (err == string.Empty && ObjectQuickbook.ListID != "")
                            {
                                Abstract objectToUpdate = quickbookRecords.Find(d => d.ListID == ObjectQuickbook.ListID);

                                if (objectToUpdate != null)
                                {
                                    if (ConfiguracionRespuestaSave.Count > 0)
                                    {
                                        //update
                                        //obtener el List]ID de quickbook adicionar al cvs
                                        HelperTask.AddConfigSave(ConfiguracionRespuestaSave, llaveQuickbase, objectToUpdate, ref err, ref RespuestasSave);
                                        //El remover generico 
                                        quickbookRecords.Remove(objectToUpdate);

                                    }
                                    if (ConfiguracionRespuestaLog.Count > 0)
                                    {
                                        string log = accion.tipo + accion.quickbookTabla + "," + xmlSend + "," + xmlRecived + "," + "1";
                                        HelperTask.AddConfigLog(ConfiguracionRespuestaLog, log, ref RespuestasLog);
                                    }
                                }

                                if (quickbookRecords.Count == 0)
                                {
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ConfiguracionRespuestaLog.Count > 0)
                            {
                                string log = accion.tipo + accion.quickbookTabla + "," + "Al crear el objeto" + "," + "Se requiere la libreria Quickbook" + "," + "0";
                                HelperTask.AddConfigLog(ConfiguracionRespuestaLog, log, ref RespuestasLog);
                            }
                            throw new Exception("Error al conectar a Quickbook: " + ex.Message);
                        }

                    }
                    else
                    {

                        if (ConfiguracionRespuestaLog.Count > 0)
                        {//record id
                            string log = accion.tipo + accion.quickbookTabla + ", Registro:Requerido:" + fieldRiquiered + "," + "Se requiere el campo se encuentra vacio o null" + "," + "0";
                            HelperTask.AddConfigLog(ConfiguracionRespuestaLog, log, ref RespuestasLog);
                        }
                    }
                }

            }
            //Toupdate
            HelperTask.ImportToQuickBase(ConfiguracionRespuestaSave, ConfiguracionRespuestaLog, RespuestasSave, RespuestasLog, ref err);
            return "Finalizo proceso de actualizacion " + Environment.NewLine;
        }
        public static string CreateQuickbookToQuickbaseNewVendor(ProcesoAccion accion, List<Abstract> quickbookRecords, ref string err)
        {
            //creando la coleccion que almacenara las respuestas de la accion.
            List<ProcesoRespuesta> ConfiguracionRespuestaCreate = accion.respuestas.FindAll(d => d.categoria == "Create");
            Dictionary<string, string> fieldNameExternalsCreate = HelperTask.GetFieldNameKeyExternal(ConfiguracionRespuestaCreate);
            Dictionary<string, List<string>> RespuestasCreate = new Dictionary<string, List<string>>();


            Dictionary<string, string> llaveQuickbase = new Dictionary<string, string>();

            wClient.WService ws = new wClient.WService();
            foreach (var objectToCreate in quickbookRecords)
            {
                if (ConfiguracionRespuestaCreate.Count > 0)
                {
                   
                    //obtener el List]ID de quickbook adicionar al cvs
                    HelperTask.AddConfigSave(ConfiguracionRespuestaCreate, llaveQuickbase, objectToCreate, ref err, ref RespuestasCreate);


                   
                }
            }

            HelperTask.ImportToQuickBase(ConfiguracionRespuestaCreate, null, RespuestasCreate, null, ref err);

            return "Finalizo proceso de registro " + Environment.NewLine;
        }
        public static bool CargadoDetalle(Abstract quickbookRecord, ProcesoAccion accion, string Details, ref string err)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            List<List<Par>> data = null;
            StringBuilder mostrarMensaje = new StringBuilder();
            try
            {
                data = serializer.Deserialize<List<List<Par>>>(Details);


                if (data != null && data.Count > 0)
                {
                    Dictionary<string, List<string>> RespuestasSave = new Dictionary<string, List<string>>();
                    Dictionary<string, List<string>> RespuestasLog = new Dictionary<string, List<string>>();
                    string llaveQuickbook = "";
                    Dictionary<string, string> llaveQuickbase = new Dictionary<string, string>();

                    //obtener los campos de respuesta para los parametros a enviar
                    //el token sera la llave

                    List< string> fieldNameExternals = HelperTask.GetFieldNameKeyExternalDetalle(accion.details);
                    
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
                        
                        
                          HelperTask.GetValuesToAddDetails(accion.details, pairs, ref fieldNames, ref fieldValues, ref fieldNameExternals, ref llaveQuickbase, ref fieldRiquiered, ref Requiered, ref mostrarMensaje, ref err);

                        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                        if (Requiered == false)
                        {

                            try
                            {
                                string fullPath = System.IO.Directory.GetCurrentDirectory();

                                Assembly testAssembly = Assembly.LoadFile(fullPath + "\\Quickbook.dll");

                                Type difineType = testAssembly.GetType("Quickbook." + accion.quickbookTablaDetalle);
                                                               
                                object objQuickbookInstance = Activator.CreateInstance(difineType);
                                Abstract ObjectQuickbookItem = (Abstract)Generic.SetFields(fieldNames, fieldValues, objQuickbookInstance, ref err);
                                
                                ((Bill)quickbookRecord).addExpenseLine(ObjectQuickbookItem);
                                
                            }
                            catch (Exception ex)
                            {                                
                               mostrarMensaje.Append( "Error al conectar a Quickbook: " + ex.Message);
                            }

                        }
                        else
                        {
                            mostrarMensaje.Append("Error campos del detalle faltantes: " );
                            
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                err = "Ejecutando accion Error des serializando detalles:" + ex.Message;

            }
            return true;

        }

        public static void GetValuesToAddDetails(List<ProcesoParametros> parametrosDetalle, Hashtable pairs, ref List<string> fieldNames, ref  List<object> fieldValues, ref List< string> fieldNameExternals, ref Dictionary<string, string> llaveQuickbase, ref string fieldRiquiered, ref bool Requiered, ref StringBuilder mostrarMensaje, ref string err)
        {
            try
            {
                for (int i = 0; i < parametrosDetalle.Count; i++)
                {


                    bool isValue = false;
                    if (!parametrosDetalle[i].isKey)
                    {
                        if (parametrosDetalle[i].fieldId.ToString().Contains("/"))//campo
                        { //Elemento compuesto 

                            string[] field = parametrosDetalle[i].fieldId.ToString().Split('/');
                            string fieldPropertyCustomer = field[0];//parenref
                            string fieldPropertyCurrency = field[1];//listID

                            if (fieldPropertyCustomer == "Custom")
                            {

                                fieldNames.Add(fieldPropertyCustomer);
                                fieldValues.Add(pairs[parametrosDetalle[i].fieldName].ToString());
                                isValue = true;
                            }
                            else
                            {

                                fieldNames.Add(fieldPropertyCustomer);

                                if (parametrosDetalle[i].Type != null)
                                {

                                    string value = pairs[fieldPropertyCustomer].ToString();
                                    if (parametrosDetalle[i].Type == "AdditionalContactRef")
                                    {
                                        fieldValues.Add(value);
                                        isValue = true;

                                    }
                                    else
                                    {

                                        if (value != string.Empty)
                                        {

                                            var newValueObject = HelperTask.ConvertType(fieldPropertyCurrency, value, parametrosDetalle[i].Type, ref err);
                                            if (err != string.Empty)
                                            {

                                                mostrarMensaje.Append("Convirtiendo valores Error:" + err + Environment.NewLine);

                                            }

                                            fieldValues.Add(newValueObject);
                                            isValue = true;
                                        }
                                        else
                                        {
                                            if (parametrosDetalle[i].Required)
                                            {

                                                Requiered = true;
                                                fieldRiquiered = parametrosDetalle[i].fieldId;
                                                break;
                                            }
                                            else
                                            {
                                                fieldValues.Add(null);
                                                isValue = true;
                                            }


                                        }
                                    }

                                }
                                else
                                {
                                    fieldValues.Add(pairs[parametrosDetalle[i].fieldName]);
                                    isValue = true;
                                }

                            }

                        }
                        else
                        {
                            fieldNames.Add(parametrosDetalle[i].fieldId);
                        }
                        if (isValue == false)
                        {
                            if (pairs.ContainsKey(parametrosDetalle[i].fieldName))
                            {

                                if (pairs[parametrosDetalle[i].fieldName] != null && parametrosDetalle[i].Type != null)
                                {

                                    string value = pairs[parametrosDetalle[i].fieldName].ToString();
                                    if (value != string.Empty)
                                    {
                                        var newDate = HelperTask.ConvertType(value, parametrosDetalle[i].Type);
                                        fieldValues.Add(newDate);
                                    }
                                    else
                                    {
                                        fieldValues.Add(null);
                                    }

                                }
                                else
                                {
                                    fieldValues.Add(pairs[parametrosDetalle[i].fieldName]);
                                }


                            }
                            else
                            {
                                fieldValues.Add(null);
                            }
                        }

                    }
                    else
                    {  //si es el campo llave debe ser igual con fieldNameExternal


                        var value = Array.Find(fieldNameExternals.ToArray(), element => element.Equals(parametrosDetalle[i].fieldName));

                        if (value != string.Empty && pairs[value] != null)
                        {
                            //valor de la llave externa
                            
                                llaveQuickbase.Add(value, pairs[value].ToString());
                            

                        }

                    }


                }
            }
            catch (Exception e)
            {
                err = e.Message;

            }


        }
    }
}
