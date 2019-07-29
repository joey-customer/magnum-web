using System;
using Magnum.Consoles.Commons;
using NDesk.Options;

namespace Magnum.Consoles.Miscs
{
	public class DummyApplication : ConsoleAppBase
	{
        public override OptionSet CreateOptionSet()
        {
            ClearArgument();
            var options = new OptionSet();
            return options;
        }

        protected override int Execute()
        {
            Console.WriteLine("This mocked application !!!");
            return 0;
        }
    }
}
