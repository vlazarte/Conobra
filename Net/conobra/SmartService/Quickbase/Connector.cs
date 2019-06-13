using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Intuit.QuickBase.Core;
using System.Collections;
using System.Xml;

namespace SmartService.Quickbase
{
    public class Connector
    {
        private string message;

        private string __username;
        private string __password;
        private string __token;
        private string __domain;
        private string __ticket;

        private long start_connection = 0;

        public string getMessage()
        {
            return message;
        }

        public Connector(string username, string password, string realm)
        {
            __username = username;
            __password = password;
            __domain = realm + ".quickbase.com";
            start_connection = 0;
        }

        public void setToken(string token)
        {
            __token = token;
        }

        public bool IsLogin()
        {
            if (start_connection == 0)
                return false;
            long now = DateTime.Now.Ticks;

            TimeSpan duration = new TimeSpan(start_connection - now);
            return (duration.TotalHours < 23);
        }

        public bool Login()
        {
            try
            {
                if (IsLogin())
                    return true;

                var auth = new Authenticate(__username, __password, __domain);
                /////// error en el post create navigator
                var response = auth.Post().CreateNavigator();
                //*****
                var errcode = response.SelectSingleNode("/qdbapi/errcode").ToString();
                if (errcode == "0")
                {
                    __ticket = response.SelectSingleNode("/qdbapi/ticket").ToString();
                    start_connection = DateTime.Now.Ticks;
                    return true;
                }
                else
                {
                    message = "Error " + response.SelectSingleNode("/qdbapi/errcode").ToString() + ": " + response.SelectSingleNode("/qdbapi/errtext").ToString();
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return false;

        }

        public bool deleteRecords(string db, string query)
        {
            try
            {
                PurgeRecords.Builder builder = new PurgeRecords.Builder(__ticket, __token, __domain, db);
                if (query != "")
                    builder.SetQuery(query);

                var response = builder.Build().Post().CreateNavigator();
                var errcode = response.SelectSingleNode("/qdbapi/errcode").ToString();
                if (errcode == "0")
                {
                    return true;
                }
                else
                {
                    message = "Error " + response.SelectSingleNode("/qdbapi/errcode").ToString() + ": " + response.SelectSingleNode("/qdbapi/errtext").ToString();
                }

            }
            catch (Intuit.QuickBase.Core.Exceptions.CannotChangeValueOfFieldException ex)
            {
                message = ex.Message;
            }
            return false;
        }

        public List<Record> DoQuery(string db, string query, string clist, int qid = 0, string qname = "", string slist = "", string options = "")
        {
            List<Record> records = new List<Record>();

            try
            {
                DoQuery.Builder doQuery = new DoQuery.Builder(__ticket, __token, __domain, db);

                if (query != string.Empty)
                    doQuery.SetQuery(query);

                doQuery.SetCList(clist);

                if (qid > 0)
                    doQuery.SetQid(qid);
                else if (qname != string.Empty)
                    doQuery.SetQName(qname);

                if (slist != string.Empty)
                    doQuery.SetSList(slist);

                doQuery.SetFmt(true);

                if (options != string.Empty)
                    doQuery.SetOptions(options);

                var response = doQuery.Build().Post().CreateNavigator();
                var errcode = response.SelectSingleNode("/qdbapi/errcode").ToString();
                if (errcode == "0")
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(response.OuterXml);

                    XmlNodeList list = xml.SelectNodes("/qdbapi/table/records/record");

                    foreach (XmlNode node in list)
                    {
                        Record R = new Record();
                        var fields = node.SelectNodes("f");

                        foreach (XmlNode f in fields)
                        {
                            R.setFieldValue(Int32.Parse(f.Attributes["id"].InnerText), f.InnerText);
                        }
                        records.Add(R);
                    }
                    return records;
                }
                else
                {
                    message = "Error " + response.SelectSingleNode("/qdbapi/errcode").ToString() + ": " + response.SelectSingleNode("/qdbapi/errtext").ToString();
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return null;

        }

        public bool EditRecord(string db, int rid, List<IField> fields)
        {
            try
            {
                EditRecord.Builder builder = new EditRecord.Builder(__ticket, __token, __domain, db, rid, fields);
                var response = builder.Build().Post().CreateNavigator();
                var errcode = response.SelectSingleNode("/qdbapi/errcode").ToString();
                if (errcode == "0")
                {
                    return true;
                }
                else
                {
                    message = "Error " + response.SelectSingleNode("/qdbapi/errcode").ToString() + ": " + response.SelectSingleNode("/qdbapi/errtext").ToString();
                }

            }
            catch (Intuit.QuickBase.Core.Exceptions.CannotChangeValueOfFieldException ex)
            {
                message = ex.Message;
            }
            catch (Intuit.QuickBase.Core.Exceptions.InvalidInputException ex2)
            {
                message = "Revise el valor de los campos enviados. " + ex2.Message;
            }
            catch (Exception ex3)
            {
                message = ex3.Message;
            }
            return false;
        }

        public int AddRecord(string db, List<IField> fields)
        {
            try
            {
                AddRecord.Builder builder = new AddRecord.Builder(__ticket, __token, __domain, db, fields);
                var response = builder.Build().Post().CreateNavigator();
                var errcode = response.SelectSingleNode("/qdbapi/errcode").ToString();
                if (errcode == "0")
                {
                    return Int32.Parse(response.SelectSingleNode("/qdbapi/rid").ToString());
                }
                else
                {
                    message = "Error " + response.SelectSingleNode("/qdbapi/errcode").ToString() + ": " + response.SelectSingleNode("/qdbapi/errtext").ToString();
                }

            }
            catch (Intuit.QuickBase.Core.Exceptions.CannotChangeValueOfFieldException ex1)
            {
                message = ex1.Message;
            }
            catch (Intuit.QuickBase.Core.Exceptions.InvalidInputException ex2)
            {
                message = "Revise el valor de los campos enviados. " + ex2.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return -1;
        }
        //----------------------------------------------------

        public bool ImportFromCSV(string db, string csv, string cList)
        {
            try
            {
                ImportFromCSV.Builder builder = new ImportFromCSV.Builder(__ticket, __token, __domain, db, csv);
                builder.SetCList(cList);
                var response = builder.Build().Post().CreateNavigator();
                var errcode = response.SelectSingleNode("/qdbapi/errcode").ToString();
                if (errcode == "0")
                {
                    return true;
                }
                else
                {
                    message = "Error " + response.SelectSingleNode("/qdbapi/errcode").ToString() + ": " + response.SelectSingleNode("/qdbapi/errtext").ToString();
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return false;
        }

        public bool CleanTable(string db)
        {
            try
            {
                PurgeRecords.Builder purge = new PurgeRecords.Builder(__ticket, __token, __domain, db);
                purge.SetQid(1);
                var response = purge.Build().Post().CreateNavigator();

                var errcode = response.SelectSingleNode("/qdbapi/errcode").ToString();
                if (errcode == "0")
                {
                    return true;
                }
                else
                {
                    message = "Error " + response.SelectSingleNode("/qdbapi/errcode").ToString() + ": " + response.SelectSingleNode("/qdbapi/errtext").ToString();
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return false;
        }

        public static DateTime getDate(string time)
        {
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var date = new DateTime(startTime.Ticks + (Int64.Parse(time + "") * TimeSpan.TicksPerMillisecond));
            return date;
        }

        public Hashtable RecordInfo(string db, int recordId)
        {
            Hashtable fields = new Hashtable();
            try
            {
                GetRecordInfo query = new GetRecordInfo(__ticket, __token, __domain, db, recordId);
                var response = query.Post().CreateNavigator();
                var errcode = response.SelectSingleNode("/qdbapi/errcode").ToString();
                if (errcode == "0")
                {

                    var fs = response.Select("/qdbapi/field");
                    XmlDocument doc = new XmlDocument();
                    while (fs.MoveNext())
                    {
                        doc.LoadXml(fs.Current.OuterXml);
                        fields.Add("f-" + doc["field"]["fid"].InnerText, doc["field"]["value"].InnerText);
                    }
                }
                else
                {
                    message = "Error " + response.SelectSingleNode("/qdbapi/errcode").ToString() + ": " + response.SelectSingleNode("/qdbapi/errtext").ToString();
                    fields = null;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                fields = null;
            }
            return fields;
        }
    }
}