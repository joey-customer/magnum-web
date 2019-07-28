using System;

using Magnum.Consoles.Commons;
using Magnum.Consoles.Factories;
using NDesk.Options;

namespace Magnum.Consoles
{
    class Program
    {
        static void Main(string[] args)
        {            
            if (args.Length <= 0)
            {
                Console.WriteLine("Missing application name!!!");
                return;
            }

            string appName = args[0];
            IConsoleApp app = FactoryConsoleApplication.CreateConsoleApplicationObject(appName);
            
            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            app.Run();
        }
    }
}
