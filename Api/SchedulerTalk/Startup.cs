using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;

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
            // https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-2
            // NLog: Setup the logger first to catch all errors
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
                    .AllowCredentials());
            });

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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Version 1.0");
                c.RoutePrefix = string.Empty;
            });

            // app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
