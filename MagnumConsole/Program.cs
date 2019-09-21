using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Magnum.Consoles.Factories;
using NDesk.Options;

using Serilog;
using Its.Onix.Core.Factories;
using Its.Onix.Erp.Services;
using Its.Onix.Core.Applications;

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

            FactoryBusinessOperation.ClearRegisteredItems();
            FactoryBusinessOperation.RegisterBusinessOperations(BusinessErpOperations.GetInstance().ExportedServicesList());

            FactoryCacheContext.ClearRegisteredItems();
            FactoryCacheContext.RegisterCaches(BusinessErpCaches.GetInstance().ExportedServicesList());

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
            IApplication app = FactoryConsoleApplication.CreateConsoleApplicationObject(appName);    

            OptionSet opt = app.CreateOptionSet();
            opt.Parse(args);

            app.Run();
        }          
    }
}
