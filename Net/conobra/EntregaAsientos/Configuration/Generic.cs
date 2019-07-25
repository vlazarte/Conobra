using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using Quickbook;
using SmartQuickbook.Helper;

namespace SmartQuickbook.Configuration
{
    public class Generic
    {
        public static object SetFields(List<string> fieldNames, List<object> fieldValues, object obj, ref string err)
        {
            err = "";
            for (int i = 0; i < fieldNames.Count; i++)
            {
                if (fieldNames[i] != null)
                {


                    PropertyInfo _propertyInfo = obj.GetType().GetProperty(fieldNames[i]);
                    if (_propertyInfo == null)
                    {
                        if (fieldNames[i] == "Custom")
                        {
                            ((Abstract)obj).AddDataEx("ID", fieldValues[i].ToString());
                        }
                        else
                        {
                            
                                err = "Property with names " + fieldNames[i] + " does not exists in " + obj.GetType().ToString();
                                return null;
                            
                        }



                    }
                    else
                    {


                        if (fieldNames[i] == "AdditionalContactRef")
                        {
                            if (((Vendor)obj).AdditionalContactRef == null)
                            {
                                ((Vendor)obj).AdditionalContactRef = new List<AdditionalContact>();
                                AdditionalContact adicional = new AdditionalContact();
                                adicional.ContactName = "Main Phone";
                                adicional.ContactValue = fieldValues[i].ToString();
                                ((Vendor)obj).AdditionalContactRef.Add(adicional);
                            }
                        }
                        else {
                            _propertyInfo.SetValue(obj, fieldValues[i], null);
                        }
                        
                    }

                }

            }
            return obj;
        }

        public static object SetField(string fieldName, object fieldValue, object obj, ref string err)
        {
            err = "";
            PropertyInfo _propertyInfo = obj.GetType().GetProperty(fieldName);
            if (_propertyInfo == null)
            {
                err = "Property with names " + fieldName + " does not exists in " + obj.GetType().ToString();
                return null;
            }
            _propertyInfo.SetValue(obj, fieldValue, null);
            return obj;
        }
        public static string GetFieldsToCsv(List<ProcesoParametros> parametros, string keyExternalValue, object obj, ref string err)
        {
            try
            {
                err = "";
                 List< String> fieldValues = new List<string>();


                for (int i = 0; i < parametros.Count; i++)
                {
                    if (!parametros[i].isKey)
                    {
                        if (parametros[i].Type == null)
                        {
                            PropertyInfo[] ifnoo = obj.GetType().GetProperties();
                            PropertyInfo _propertyInfo = obj.GetType().GetProperty(parametros[i].fieldName);
                            if (_propertyInfo == null)
                            {
                                err = "Property with names " + parametros[i].fieldName + " does not exists in " + obj.GetType().ToString() + Environment.NewLine;
                                return null;
                            }
                            
                                object objectValue = _propertyInfo.GetValue(obj, null);
                                Abstract ObjectQuickbook = objectValue as Abstract;
                                if (ObjectQuickbook != null)
                                {
                                    fieldValues.Add( ObjectQuickbook.ListID);
                                }
                                else
                                {
                                    fieldValues.Add(Convert.ToString(objectValue).Replace("\"", "\"\""));
                                }

                        }
                        else
                        {

                            string[] field = parametros[i].Type.ToString().Split('/');
                            string fieldPropertyCustom = field[0];//custom
                            string fieldPropertyName = field[1];//ID
                            if (fieldPropertyCustom == "Custom")
                            {
                                Abstract newValueObject = obj as Abstract;
                                string value = newValueObject.getDataExValue(fieldPropertyName);

                                if (value != null)
                                {

                                    fieldValues.Add( value.Replace("\"", "\"\""));

                                }
                                else
                                {
                                    fieldValues.Add( "");
                                }
                            }
                            else
                            {                               
                                    if (fieldPropertyCustom == "AdditionalContactRef")
                                    {
                                        if (((Vendor)obj).AdditionalContactRef == null)
                                        {
                                            fieldValues .Add("");
                                        }
                                        else {
                                            AdditionalContact newValue=  ((Vendor)obj).AdditionalContactRef.Find(d => d.ContactName == "Main Phone");
                                            if (newValue != null)
                                            {
                                                fieldValues.Add( newValue.ContactValue.Replace("\"", "\"\""));
                                            }
                                            else {
                                                   fieldValues.Add( "");
                                            }

                                        }
                                    }
                                    else {
                                        ////si es otro dato compuesto obtener el tipo
                                        string value = HelperTask.getObjectValue(parametros[i].fieldName, fieldPropertyCustom, obj, fieldPropertyName, ref err);
                                        if (value != null)
                                        {
                                               fieldValues.Add( value.Replace("\"", "\"\""));

                                        }
                                        else
                                        {
                                           fieldValues.Add( "");
                                        }
                                    }
                            }

                        }

                    }
                    else
                    {

                        if (parametros[i].Type == "Configuration")
                        {
                            fieldValues.Add(Config.CompaniaDB);
                        }
                        else
                        {
                            if (keyExternalValue != string.Empty)
                            {
                                fieldValues.Add(keyExternalValue);
                            }
                            else {
                                fieldValues.Add("");
                            }
                       
                            
                        }

                       
                    }

                }

                string fila = "\"" + String.Join("\",\"", fieldValues.ToArray()) + "\"";
                //fila="\"
                return fila;
            }
            catch (Exception ex)
            {
                err = ex.Message + Environment.NewLine;
                return string.Empty;

            }


        }

        public static Hashtable getPairValues(List<Par> list)
        {
            Hashtable data = new Hashtable();
            foreach (Par P in list)
            {
                data.Add(P.Key, P.Value);
            }
            return data;
        }
        public static string[] getFieldValues(List<ProcesoParametros> parametros, object obj, ref string err)
        {
            err = "";
            int countToCreate = parametros.Count * 2;
            String[] fieldValues = new string[countToCreate];


            int index = 0;
            int i = 0;
            while (index < countToCreate)
            {
                PropertyInfo[] ifnoo = obj.GetType().GetProperties();
                PropertyInfo _propertyInfo = obj.GetType().GetProperty(parametros[i].fieldName);
                if (_propertyInfo == null)
                {
                    err = "Property with names " + parametros[index].fieldName + " does not exists in " + obj.GetType().ToString();
                    return null;
                }

                object objectValue = _propertyInfo.GetValue(obj, null);
                Abstract ObjectQuickbook = objectValue as Abstract;
                if (ObjectQuickbook != null)
                {
                    fieldValues[index] = parametros[i].fieldName;
                    fieldValues[index + 1] = ObjectQuickbook.ListID;
                }
                else
                {
                    fieldValues[index] = parametros[i].fieldName;
                    fieldValues[index + 1] = Convert.ToString(objectValue);
                }
                index = index + 2;
                i++;
            }



            return fieldValues;
        }
    }
}
