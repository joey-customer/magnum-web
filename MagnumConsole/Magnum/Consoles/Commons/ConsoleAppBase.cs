using System;
using System.Collections;

using NDesk.Options;
using Magnum.Api.NoSql;

namespace Magnum.Consoles.Commons
{
	public abstract class ConsoleAppBase : IConsoleApp
	{
        private readonly Hashtable arguments = new Hashtable();
        private INoSqlContext context = null;
        
        protected abstract int Execute();
        public abstract OptionSet CreateOptionSet();        
        
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

        public INoSqlContext GetNoSqlContext()
        {
            return context;
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
       
    }
}
