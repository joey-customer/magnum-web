﻿using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Magnum.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {            
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
