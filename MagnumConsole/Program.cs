using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Magnum.Consoles.Commons;
using Magnum.Consoles.Factories;
using NDesk.Options;

using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Magnum.Consoles
{
    public static class Program
    {   
        public static void Main(string[] args)
        {   
            if (args.Length <= 0)
            {
                Console.WriteLine("Missing application name!!!");
                return;
            }

            string appName = args[0];

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.AddSerilog());
        
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            FactoryConsoleApplication.SetLoggerFactory(loggerFactory);
            IConsoleApp app = FactoryConsoleApplication.CreateConsoleApplicationObject(appName);    

            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            app.Run();
        }          
    }
}
