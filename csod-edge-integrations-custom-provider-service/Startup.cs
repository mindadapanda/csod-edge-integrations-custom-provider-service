using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using csod_edge_integrations_custom_provider_service.Models;
using Microsoft.DotNet.PlatformAbstractions;
using LiteDB;
using NLog.Extensions.Logging;
using NLog.Web;
using csod_edge_integrations_custom_provider_service.Data;
using csod_edge_integrations_custom_provider_service.Middleware;

namespace csod_edge_integrations_custom_provider_service
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {            
            env.ConfigureNLog("nlog.config");

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Add framework services.
            services.AddMemoryCache();
            services.AddMvc().AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSingleton(x => new LiteRepository(Configuration.GetConnectionString("LiteDbDev")));            
            services.AddSingleton<UserRepository>();
            services.AddSingleton<SettingsRepository>();
            services.AddSingleton<CallbackRepository>();
            services.AddSingleton(x => new CustomCrypto(Configuration.GetSection("Crypto")["Salt"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseBasicAuthentication();

            app.AddNLogWeb();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Run(conext => conext.Response.WriteAsync("Hello from ASP.NET Core!"));
        }
    }
}
