using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quickbook
{
    public class Vars
    {
        public static Connector qb = null;

        public static bool isLocalhost()
        {
            if (qb == null)
                return false;

            return qb.getLocalhost();
        }
    }
}
