using System;
using System.IO;
using System.Collections;
using System.Drawing;

using QRCoder;
using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.Utils;
using Magnum.Api.Businesses.Barcodes;

using NDesk.Options;

namespace Magnum.Consoles.Commons
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
