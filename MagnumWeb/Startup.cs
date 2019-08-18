using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Magnum.Api.Factories;

using Serilog;

using Magnum.Api.Storages;
using Magnum.Api.NoSql;

namespace Magnum.Web
{
    public class Startup
    {
        private static void SetupFactory(ILoggerFactory logFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
                            
            string host = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_URL");
            string key = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_KEY");
            string user = Environment.GetEnvironmentVariable("MAGNUM_DB_USERNAME");
            string password = Environment.GetEnvironmentVariable("MAGNUM_DB_PASSWORD");
            string bucket = Environment.GetEnvironmentVariable("MAGNUM_FIREBASE_BUCKET");

            Microsoft.Extensions.Logging.ILogger logger = logFactory.CreateLogger<FirebaseNoSqlContext>();

            FirebaseNoSqlContext ctx = new FirebaseNoSqlContext();
            ctx.SetLogger(logger);            
            ctx.Authenticate(host, key, user, password);

            var storageCtx = new FirebaseStorageContext();
            storageCtx.Authenticate(bucket, key, user, password);

            FactoryBusinessOperation.SetStorageContext(storageCtx);
            FactoryBusinessOperation.SetNoSqlContext(ctx);
            FactoryBusinessOperation.SetLoggerFactory(logFactory);
        }

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddLogging(builder => builder.AddSerilog());
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            SetupFactory(loggerFactory);            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseHsts();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
