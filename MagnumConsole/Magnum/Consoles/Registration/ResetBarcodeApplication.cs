using System;
using System.Collections;

using Its.Onix.Erp.Models;
using Its.Onix.Erp.Businesses.Registrations;
using Its.Onix.Core.Factories;
using Its.Onix.Core.NoSQL;

using Magnum.Consoles.Commons;
using Magnum.Consoles.Factories;

using NDesk.Options;

namespace Magnum.Consoles.Registrations
{
	public class ResetBarcodeApplication : ConsoleAppBase
	{
        public ResetBarcodeApplication()
        {
        }

        protected override OptionSet PopulateCustomOptionSet(OptionSet options)
        {
            options.Add("s=|serial=", "Serial number", s => AddArgument("serial", s))
            .Add("p=|pin=", "PIN number", s => AddArgument("pin", s));
            
            return options;
        }

        protected override int Execute()
        {
            Hashtable args = GetArguments();
            string serial = args["serial"].ToString();
            string pin = args["pin"].ToString();

            INoSqlContext ctx = GetNoSqlContextWithAuthen("firebase");

            FactoryBusinessOperation.SetNoSqlContext(ctx);
            FactoryBusinessOperation.SetLoggerFactory(FactoryConsoleApplication.GetLoggerFactory());
            ResetRegistration opr = (ResetRegistration) FactoryBusinessOperation.CreateBusinessOperationObject("ResetRegistration");

            MRegistration param = new MRegistration();
            param.Pin = pin;
            param.SerialNumber = serial;

            try
            {
                opr.Apply(param);
                Console.WriteLine("Done reset barcode [{0}] [{1}]", serial, pin);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : {0}", e.Message);
            }
            
            return 0;
        }
    }
}
