using System;
using NDesk.Options;

using Its.Onix.Core.Applications;

namespace Magnum.Consoles.Miscs
{
	public class DummyApplication : ConsoleAppBase
	{
        public override OptionSet CreateOptionSet()
        {
            ClearArgument();
            var options = new OptionSet();
            PopulateCustomOptionSet(options);
            return options;
        }

        protected override OptionSet PopulateCustomOptionSet(OptionSet options)
        {
            //Do nothing
            return options;
        }        

        protected override int Execute()
        {
            Console.WriteLine("This mocked application !!!");
            return 0;
        }
    }
}
