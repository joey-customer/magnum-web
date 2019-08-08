using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            string certMode = Environment.GetEnvironmentVariable("MAGNUM_CERTIFICATE_ON");
            if (string.IsNullOrEmpty(certMode))
            {
                certMode = "Y";
            }

            var builder = WebHost.CreateDefaultBuilder(args)   
                .UseStartup<Startup>();

            if (certMode.Equals("Y"))
            {
                //.pfx file
                string certFile = Environment.GetEnvironmentVariable("MAGNUM_CERTIFICATE_FILE");
                string password = Environment.GetEnvironmentVariable("MAGNUM_CERTIFICATE_PASSWORD");

                builder.ConfigureKestrel((context, options) =>
                {
                    options.ListenAnyIP(443, listenOptions =>
                    {
                        listenOptions.UseHttps(certFile, password);
                    });
                }); 
            }
 
            return builder;
        }
    }
}
