using System;
using System.IO;
using System.Collections;

using Microsoft.Extensions.Logging;

using NDesk.Options;
using Magnum.Api.NoSql;
using Magnum.Api.Storages;
using Magnum.Api.Commons.Table;
using Magnum.Api.Utils.Serializers;

namespace Magnum.Consoles.Commons
{
	public abstract class ConsoleAppBase : IConsoleApp
	{
        private ILogger appLogger;
        private readonly Hashtable arguments = new Hashtable();
        private INoSqlContext context = null;
        private IStorageContext storageContext = null;
        
        protected abstract int Execute();

        protected abstract OptionSet PopulateCustomOptionSet(OptionSet options);

        public virtual OptionSet CreateOptionSet()
        {
            ClearArgument();

            var options = new OptionSet();

            options.Add("h=|host", "Firebase URL", s => AddArgument("host", s))
                .Add("bucket=", "Storage bucket URL", s => AddArgument("bucket", s))
                .Add("k=|key=", "Oauth key to access Firebase", s => AddArgument("key", s))
                .Add("user=", "Firebase username", s => AddArgument("user", s))
                .Add("password=", "Firebase password", s => AddArgument("password", s));

            PopulateCustomOptionSet(options);

            return options;
        }   
        
        protected void ClearArgument()
        {
            arguments.Clear();
        }

        public void AddArgument(string key, string value)
        {
            arguments[key] = value;
        }

        public void SetNoSqlContext(INoSqlContext context)
        {
            this.context = context;
        }

        public void SetStorageContext(IStorageContext context)
        {
            this.storageContext = context;
        }

        public IStorageContext GetStorageContext()
        {
            return storageContext;
        }        

        public void SetLogger(ILogger logger)
        {
            appLogger = logger;
        }

        public ILogger GetLogger()
        {
            return appLogger;
        }

        public INoSqlContext GetNoSqlContext()
        {
            return context;
        }

        public INoSqlContext GetNoSqlContextWithAuthen(string provider)
        {
            INoSqlContext ctx = context;
            if (context == null)
            {
                string host = (string) arguments["host"];
                string key = (string) arguments["key"];
                string user = (string) arguments["user"];
                string password = (string) arguments["password"];
                ctx = GetNoSqlContext(provider, host, key, user, password);
            }

            return ctx;
        }

        public IStorageContext GetStorageContextWithAuthen(string provider)
        {
            IStorageContext ctx = storageContext;
            if (storageContext == null)
            {
                string bucket = (string) arguments["bucket"];
                string key = (string) arguments["key"];
                string user = (string) arguments["user"];
                string password = (string) arguments["password"];
                ctx = GetStorageContext(provider, bucket, key, user, password);
            }

            return ctx;
        }

        public int Run()
        {
            int code = Execute();
            return code;
        }

        public void DumpParameter()
        {
            foreach (string key in arguments.Keys)
            {
                string v = (string) arguments[key];
                Console.WriteLine("Param : {0} - {1}", key, v);
            }            
        }

        public Hashtable GetArguments()
        {
            return arguments;
        }

        protected INoSqlContext GetNoSqlContext(string provider, string host, string key, string user, string password)
        {
            INoSqlContext ctx = null;
            if (provider.Equals("firebase"))
            {
                ctx = new FirebaseNoSqlContext();
                ctx.Authenticate(host, key, user, password); 
            }

            return ctx;
        }
       
        protected IStorageContext GetStorageContext(string provider, string host, string key, string user, string password)
        {
            IStorageContext ctx = null;
            if (provider.Equals("firebase"))
            {
                ctx = new FirebaseStorageContext();
                ctx.Authenticate(host, key, user, password); 
            }

            return ctx;
        }   

        protected CTable XmlToCTable()
        {
            Hashtable args = GetArguments();
            string infile = args["infile"].ToString();
            string basedir = args["basedir"].ToString();

            string[] paths = {basedir, infile};
            string importFile = Path.Combine(paths);
            string xml = File.ReadAllText(importFile);

            XmlToCTable ds = new XmlToCTable(xml);
            CRoot root = ds.Deserialize();
            CTable t = root.Data;

            return t;        
        }
    }
}
