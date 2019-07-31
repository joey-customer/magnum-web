using System;
using System.IO;
using System.Collections;
using System.Drawing;

using QRCoder;
using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.Barcodes;
using Magnum.Api.NoSql;

using NDesk.Options;

namespace Magnum.Consoles.Commons
{
    public class ConsoleAppBaseMocked : ConsoleAppBase
    {
        private string pv = "firebase";

        public ConsoleAppBaseMocked(string provider)
        {
            pv = provider;
        }

        protected override int Execute()
        {
            var o = GetNoSqlContext(pv, "", "", "", "");
            if (o == null)
            {
                return 0;
            }

            return 1;
        }

        public override OptionSet CreateOptionSet()
        {
            return null;
        }
    }
}
