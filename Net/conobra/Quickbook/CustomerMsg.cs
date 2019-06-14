﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class CustomerMsg : Abstract
    {

        public string ListID { get; set; }
        public string FullName { get; set; }
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet CustomerMsg";
            return false;
        }
        public override List<Abstract> GetRecords(ref string err)
        {
            err = "No implemented yet CustomerMsg";
            return new List<Abstract>();
        }

  
    }
}
