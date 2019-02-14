using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerTalk.Jobs
{
    public class JobLogger
    {
        private readonly ILogger _logger;

        public PerformContext HangfireContext { get; set; }

        public JobLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);

            string output = String.Format(message, args);
            HangfireContext.WriteLine(output);
        }
    }
}
