﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class ItemAbstract : Abstract
    {
        public override bool AddRecord(ref string err, ref string xmlSend, ref string xmlRecived)
        {
            err = "No implemented yet ItemAbstract";
            return false;
        }
    }
}
