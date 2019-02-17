using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//using Hangfire.Console;
//using Hangfire.MemoryStorage;
//using Hangfire.Server;

namespace SchedulerTalkPrac
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddHangfire(config =>
            //{
            //    config.UseMemoryStorage();
            //    config.UseConsole();
            //});
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            //app.UseHangfireDashboard();
            //app.UseHangfireServer();

            app.UseHttpsRedirection();
            app.UseMvc();


            //RecurringJob.AddOrUpdate<CreateWidgetJob>("Create Widgets Job", x => x.Execute(null), Cron.Minutely());
        }
    }

    //public class CreateWidgetJob
    //{
    //    public void Execute(PerformContext context)
    //    {
    //        for (int i = 1; i <= 10; i++)
    //        {
    //            context.WriteProgressBar(i * 10);
    //            System.Threading.Thread.Sleep(1000);
    //            context.WriteLine($"Doing stuff {i}");
    //        }
    //    }
    //}
}
