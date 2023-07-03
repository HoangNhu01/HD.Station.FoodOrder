using HD.Station.FoodOrder.Abstractions.FeatureBuilder;
using HD.Station.FoodOrder.SqlServer;
using HD.Station.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HD.Station.FoodOrder.Demo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging();
            services.AddFoodOrderFeature(Configuration.GetSection("HD:Station:Feature:FoodOrder"))
                .AddManagers()
                .UseSqlServer(Configuration);
            services.AddHttpContextAccessor();
            services.AddSession(s =>
            {
                s.Cookie.Name = Configuration["AppName"] ?? Assembly.GetAssembly(typeof(Startup)).FullName;
                Configuration.GetSection("SessionOptions").Bind(s);
            });

            var sqlLocalizerConnectionString = Configuration.GetConnectionString("Localizer");
            services.AddSqlLocalization(opt =>
            {
                opt.ConnectionString = sqlLocalizerConnectionString;
                opt.SquareLocalizedString = true;
                opt.SaveLocalizableString = true;
            });
            services.AddHdMvc(Configuration);
            services.AddKendo();
            services.AddSecurityFeature(Configuration.GetSection("HD:Station:Security"))
                .AddHdSecurityData(Configuration)
                .AddSecurityCore()
                 .AddSecurityGroup()
                .AddSecurityGroupStore()
                .AddSecurityGroupMvc()
                .AddHdAuthentication()
                .AddHdPermissionAuthorization();
            services.AddHumanResourceFeature(Configuration.GetSection("HD:Station:HR"))
                 .AddHumanResourceManager(Configuration)
                 .UseSqlServer(Configuration);
            services.AddNotificationFeature(Configuration.GetSection("HD:Station:Notification"))
              .UseSqlServer(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            var cultures = new CultureInfo[]
           {
                                new CultureInfo("de-DE"),
                new CultureInfo("en-US"),

                new CultureInfo("vi-VN")
           };
            var opt = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("vi-VN"),
                FallBackToParentCultures = true,
                FallBackToParentUICultures = true,
                SupportedCultures = cultures,
                SupportedUICultures = cultures,

            };
            app.UseRequestLocalization(opt);
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "fullArea",
                    template: "{area:exists}/{controller=Home}/{id:guid}/{action=Index}"
                );
                routes.MapRoute(
                    name: "auth",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id:guid?}"
                );
                routes.MapRoute(
                   name: "home",
                   template: "{area=FoodOrder}/{controller=CustomerOrder}/{action=MenuIndex}"
                );
                routes.MapRoute(
                   name: "default",
                   template: "{area=FoodOrder}/{controller=CustomerOrder}/{action=MenuIndex}"
                );
            });

            app.UseKendo(env);
        }
    }
}
