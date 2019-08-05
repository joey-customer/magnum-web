using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Magnum.Api.NoSql;
using Magnum.Api.Factories;

namespace MagnumWeb
{
    public static class Program
    {
        private static void SetFirebaseContext()
        {
            string host = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_URL");
            string key = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_KEY");
            string user = Environment.GetEnvironmentVariable("MAGNUM_DB_USERNAME");
            string password = Environment.GetEnvironmentVariable("MAGNUM_DB_PASSWORD");

            INoSqlContext ctx = null;
            ctx = new FirebaseNoSqlContext();
            ctx.Authenticate(host, key, user, password);

            FactoryBusinessOperation.SetContext(ctx);
        }

        public static void Main(string[] args)
        {
            SetFirebaseContext();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
