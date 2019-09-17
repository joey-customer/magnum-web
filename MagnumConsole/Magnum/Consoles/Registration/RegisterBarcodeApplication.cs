using System;
using System.Collections;

using Magnum.Consoles.Commons;
using Magnum.Consoles.Factories;

using Its.Onix.Erp.Models;
using Its.Onix.Erp.Businesses.Registrations;
using Its.Onix.Core.Factories;
using Its.Onix.Core.NoSQL;

using NDesk.Options;

namespace Magnum.Consoles.Registrations
{
	public class RegisterBarcodeApplication : ConsoleAppBase
	{
        public RegisterBarcodeApplication()
        {
        }

        protected override OptionSet PopulateCustomOptionSet(OptionSet options)
        {
            options.Add("s=|serial=", "Serial number", s => AddArgument("serial", s))
            .Add("p=|pin=", "PIN number", s => AddArgument("pin", s))
            .Add("i=|ip=", "IP Address", s => AddArgument("ip", s))
            .Add("path=",  "Virtual path (Firebase path)", s => AddArgument("path", s));
            
            return options;
        }

        protected override int Execute()
        {
            Hashtable args = GetArguments();
            string serial = args["serial"].ToString();
            string pin = args["pin"].ToString();
            string ip = args["ip"].ToString();

            INoSqlContext ctx = GetNoSqlContextWithAuthen("firebase");

            FactoryBusinessOperation.SetNoSqlContext(ctx);
            FactoryBusinessOperation.SetLoggerFactory(FactoryConsoleApplication.GetLoggerFactory());
            CreateRegistration opr = (CreateRegistration) FactoryBusinessOperation.CreateBusinessOperationObject("CreateRegistration");

            MRegistration param = new MRegistration();
            param.IP = ip;
            param.Pin = pin;
            param.SerialNumber = serial;

            try
            {
                opr.Apply(param);
                Console.WriteLine("Done register barcode [{0}] [{1}]", serial, pin);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : {0}", e.Message);
            }
            
            return 0;
        }
    }
}
