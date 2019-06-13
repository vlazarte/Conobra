using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace SmartService.Quickbase
{
    public class Record
    {

        public Hashtable fields { get; set; }

        public Record()
        {
            fields = new Hashtable();
        }

        public void setFieldValue(int fid, string value)
        {
            fields[fid + ""] = value;
        }

        public DateTime getDateValue(int fid)
        {
            var milliseconds = "" + fields[fid + ""];
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return new DateTime(date.Ticks + (Int64.Parse(milliseconds) * TimeSpan.TicksPerMillisecond));
        }


        public string getFieldValue(int fid)
        {
            if (fields.ContainsKey(fid + ""))
                return fields[fid + ""] + "";
            else
                return null;
        }

    }
}