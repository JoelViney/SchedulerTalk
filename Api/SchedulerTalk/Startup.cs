using Hangfire;
using Hangfire.Console;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog.Web;
using SchedulerTalk.Hubs;
using SchedulerTalk.Jobs;
using SchedulerTalk.Models;
using SchedulerTalk.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace SchedulerTalk
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;

        /// <summary></summary>
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            this._configuration = configuration;
            this._environment = environment;

            //=================================================================
            // LOGGER
            //  https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-2
            // NLog: Setup the logger first to catch all errors
            //
            var logFactory = NLogBuilder.ConfigureNLog("nlog.config");
            logFactory.KeepVariablesOnReload = true;
            logFactory.Configuration.Reload();
            var logger = logFactory.GetCurrentClassLogger();
            logger.Debug("Main Initialised...");
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            //=================================================================
            // In Memory Database
            //
            services.AddDbContext<DatabaseContext>(opt => opt.UseInMemoryDatabase("SchedulerTalk"));


            //=================================================================
            // SWAGGER
            // options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ssZ";
            // options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            // Add Swagger
            // Set the comments path for the Swagger JSON and UI.
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var swaggerXml = Path.Combine(basePath, "Swagger.xml");
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Version = "v1", Title = "Scheduler Talk API", });
                x.IncludeXmlComments(swaggerXml);
                x.CustomSchemaIds(t => t.FullName);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            //=================================================================
            // SIGNALR
            //
            services.AddSignalR();

            //=================================================================
            // DEPENDENCY INJECTION
            //
            services.AddScoped<WidgetService>();

            //=================================================================
            // CORS
            // "I allow cross domain calls from the domains I specify"
            // https://weblog.west-wind.com/posts/2016/Sep/26/ASPNET-Core-and-CORS-Gotchas
            //
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:8080", "http://localhost:49699")
                    .AllowCredentials());
            });

            //=================================================================
            // Hangfire
            //
            services.AddHangfire(config =>
                {
                    config.UseMemoryStorage();
                    config.UseConsole();
                }
            );

            //=================================================================
            // CONFIGURATION OPTIONS
            //
            services.AddMvc(opt =>
            {
                if (this._environment.IsProduction() && this._configuration["DisableSSL"] != "true")
                {
                    opt.Filters.Add(new RequireHttpsAttribute());
                }
            }).AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Bug in core 2.2 >>> https://github.com/aspnet/AspNetCore/issues/4398
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Version 1.0");
                c.RoutePrefix = string.Empty;
            });

            app.UseHangfireDashboard();
            app.UseHangfireServer();


            app.UseSignalR(routes =>
            {
                routes.MapHub<WidgetHub>("/widgethub");
            });

            // app.UseHttpsRedirection();
            app.UseMvc();

            StartJobs();

            // Seed the database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DatabaseContext>();
                AddTestData(context);
            }
        }

        private static void StartJobs()
        {
            RecurringJob.AddOrUpdate<CreateWidgetJob>("Create Widgets Job", x => x.Execute(null), Cron.Minutely());
        }

        private static void AddTestData(DatabaseContext context)
        {
            /*
            var list = new List<Widget>()
            {
                new Widget() { Name = "Foggle", Processing = false, DateCreated = DateTime.Now.AddDays(-5) },
                new Widget() { Name = "Woggle", Processing = false, DateCreated = DateTime.Now.AddDays(-4) },
                new Widget() { Name = "Sniffl", Processing = false, DateCreated = DateTime.Now.AddDays(-3) },
                new Widget() { Name = "Groblr", Processing = true, DateCreated = DateTime.Now.AddDays(-2) },
                new Widget() { Name = "Chiggl", Processing = true, DateCreated = DateTime.Now.AddDays(-1) },
            };

            context.Widgets.AddRange(list);

            context.SaveChanges();
            */
        }
    }
}
