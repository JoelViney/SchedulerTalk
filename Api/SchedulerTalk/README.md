﻿

## Packages

Microsoft.AspNetCore.App (PreInstalled)
Microsoft.NETCore.App (PreInstalled)

Install-Package Swashbuckle.AspNetCore

Install-Package Microsoft.Extensions.Logging

Install-Package NLog
Install-Package NLog.Web.AspNetCore




## Hangfire Notes

Install-Package HangFire 
Install-Package Hangfire.MemoryStorage.Core
Install-Package HangFire.Console
or
Install-Package HangFire.SqlServer -Version 1.6.22

	Startup.

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            //=================================================================
            // HANGFIRE 
            //
            services.AddHangfire(c => c.UseMemoryStorage());
			
            services.AddMvc();
			
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHangfireDashboard();
            app.UseHangfireServer();
			
            app.UseMvc();