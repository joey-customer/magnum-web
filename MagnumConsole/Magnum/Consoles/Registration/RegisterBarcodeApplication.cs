using System;
using System.IO;
using System.Collections;

using Magnum.Api.Models;
using Magnum.Consoles.Commons;
using Magnum.Api.Factories;
using Magnum.Api.Businesses.Registrations;
using Magnum.Api.NoSql;

using NDesk.Options;

namespace Magnum.Consoles.Registrations
{
	public class RegisterBarcodeApplication : ConsoleAppBase
	{
        public RegisterBarcodeApplication()
        {
        }

        public override OptionSet CreateOptionSet()
        {
            ClearArgument();

            var options = new OptionSet() 
            {
                { "h=|host",    "Firebase URL", s => AddArgument("host", s) },
                { "k=|key=",    "Oauth key to access Firebase", s => AddArgument("key", s) },
                { "s=|serial=", "Serial number", s => AddArgument("serial", s) },
                { "p=|pin=",    "PIN number", s => AddArgument("pin", s) },
                { "i=|ip=",     "IP Address", s => AddArgument("ip", s) },
                { "path=",      "Virtual path (Firebase path)", s => AddArgument("path", s) },
                { "user=",      "Firebase username", s => AddArgument("user", s) },
                { "password=",  "Firebase password", s => AddArgument("password", s) },                
            };

            return options;
        }

        protected override int Execute()
        {
            Hashtable args = GetArguments();
            string key = args["key"].ToString();
            string host = args["host"].ToString();
            string serial = args["serial"].ToString();
            string pin = args["pin"].ToString();
            string ip = args["ip"].ToString();
            string user = args["user"].ToString();
            string password = args["password"].ToString();
            string path = args["path"].ToString();

            INoSqlContext ctx = GetNoSqlContext();
            if (ctx == null)
            {
                ctx = GetNoSqlContext("firebase", host, key, user, password);
            }

            FactoryBusinessOperation.SetContext(ctx);
            CreateRegistration opr = (CreateRegistration) FactoryBusinessOperation.CreateBusinessOperationObject("CreateRegistration");

            MRegistration param = new MRegistration();
            param.IP = ip;
            param.Pin = pin;
            param.SerialNumber = serial;
            param.Path = path;

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
