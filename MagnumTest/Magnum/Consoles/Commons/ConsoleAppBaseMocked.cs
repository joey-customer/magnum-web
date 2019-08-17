using System;

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

        protected override OptionSet PopulateCustomOptionSet(OptionSet options)
        {
            //Do nothing
            return options;
        }           
    }
}
