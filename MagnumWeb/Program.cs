using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using Magnum.Api.Storages;
using Magnum.Api.NoSql;
using Magnum.Api.Factories;

namespace Magnum.Web
{
    public static class Program
    {
        private static void SetFirebaseContext()
        {
            string host = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_URL");
            string key = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_KEY");
            string user = Environment.GetEnvironmentVariable("MAGNUM_DB_USERNAME");
            string password = Environment.GetEnvironmentVariable("MAGNUM_DB_PASSWORD");
            string bucket = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_BUCKET");

            INoSqlContext ctx = null;
            ctx = new FirebaseNoSqlContext();
            ctx.Authenticate(host, key, user, password);

            var storageCtx = new FirebaseStorageContext();
            storageCtx.Authenticate(bucket, key, user, password);

            FactoryBusinessOperation.SetStorageContext(storageCtx);
            FactoryBusinessOperation.SetNoSqlContext(ctx);
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
                    options.ListenAnyIP(80);
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
