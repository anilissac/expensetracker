using System;
using System.Text.Json.Serialization;
using System.IO;


using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Microsoft.AspNetCore.Http.Features;
namespace ExpenseTracker
{
    public class Startup
    {
        private IWebHostEnvironment CurrentEnvironment { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            CurrentEnvironment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // public void ConfigureServices(IServiceCollection services, IWebHostEnvironment env)
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });
            services.AddLogging(config =>
            {
                config.SetMinimumLevel(LogLevel.Debug);
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
            });
            services.AddHttpContextAccessor();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            // services.AddControllersWithViews();
            services.AddTransient<IAppVersionService, AppVersionService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers()
            .AddJsonOptions(options =>
             options.JsonSerializerOptions.PropertyNamingPolicy = null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));   // Read Level of logging
            //loggerFactory.AddDebug(); // Log everything //anil
           // loggerFactory.AddFile($"{env.WebRootPath}/logs/{DateTime.Today:MMM_yyyy}.txt");  // FileName to log error in a folder logs

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/NotFound/{0}");
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/NotFound/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //to read or get client IP address of the client programmatically in restricted resources.
            //https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-5.0
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                //ForwardedHeaders.XForwardedFor – Identifies which forwarders should be processed.
                //ForwardedHeaders.XForwardedProto – The middleware updates the Request.Scheme for security policies to works properly.
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();
            app.UseSession();
            ServerConfiguration.ConfigureHttpContext(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            app.UseRouting();

            app.UseAuthorization();

            //----------------------
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
