using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using Quickbook;

namespace SmartQuickbook.Configuration
{
    public class Generic
    {
        public static object SetFields( List<string> fieldNames, List<object> fieldValues, object obj, ref string err )
        {
            err = "";
            for (int i = 0; i < fieldNames.Count; i++)
            {
                if (fieldNames[i] != null)
                {
                    PropertyInfo _propertyInfo = obj.GetType().GetProperty(fieldNames[i]);
                    if (_propertyInfo == null)
                    {
                        err = "Property with names " + fieldNames[i] + " does not exists in " + obj.GetType().ToString();
                        return null;
                    }
                    _propertyInfo.SetValue(obj, fieldValues[i], null);
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
            err = "";
            String fieldValues = "";

        
             for (int i = 0; i < parametros.Count; i++)                                    
            {
                if (!parametros[i].isKey)
                {
                    PropertyInfo[] ifnoo= obj.GetType().GetProperties();
                    PropertyInfo _propertyInfo = obj.GetType().GetProperty(parametros[i].fieldName);
                    if (_propertyInfo == null)
                    {
                        err = "Property with names " + parametros[i].fieldName + " does not exists in " + obj.GetType().ToString();
                        return null;
                    }
                    if (fieldValues == string.Empty)
                    {
                        fieldValues = Convert.ToString(_propertyInfo.GetValue(obj, null));
                    }
                    else
                    {
                        object objectValue = _propertyInfo.GetValue(obj, null);
                        Abstract ObjectQuickbook = objectValue as Abstract;
                        if (ObjectQuickbook != null)
                        {
                            fieldValues = fieldValues + "," + ObjectQuickbook.ListID;
                        }else{
                        fieldValues = fieldValues+"," + Convert.ToString(objectValue);
                        }
                        
                        
                        
                        
                    }
                }
                else
                {
                    if (fieldValues == string.Empty)
                    {
                        fieldValues = keyExternalValue;
                    }
                    else
                    {
                        if (keyExternalValue!=string.Empty) {
                            fieldValues = fieldValues + "," + keyExternalValue;
                        }
                        
                    }
                }
                
                
                
            }
            return fieldValues;
        }

        public static Hashtable getPairValues( List<Par> list )
        {
            Hashtable data = new Hashtable();
            foreach (Par P in list)
            {
                data.Add(P.Key, P.Value);
            }
            return data;
        }
        public static string[] getFieldValues(List<ProcesoParametros> parametros,  object obj, ref string err)
        { 
            err = "";
            String[] fieldValues = new string[parametros.Count];

        
             for (int i = 0; i < parametros.Count; i++)                                    
            {
                
                    PropertyInfo[] ifnoo= obj.GetType().GetProperties();
                    PropertyInfo _propertyInfo = obj.GetType().GetProperty(parametros[i].fieldName);
                    if (_propertyInfo == null)
                    {
                        err = "Property with names " + parametros[i].fieldName + " does not exists in " + obj.GetType().ToString();
                        return null;
                    }
                   
                        object objectValue = _propertyInfo.GetValue(obj, null);
                        Abstract ObjectQuickbook = objectValue as Abstract;
                        if (ObjectQuickbook != null)
                        {
                            fieldValues[i] = ObjectQuickbook.ListID;
                        }else{
                        fieldValues[i]= Convert.ToString(objectValue);
                        }
                    
                
            }



             return fieldValues;
        }
    }
}
