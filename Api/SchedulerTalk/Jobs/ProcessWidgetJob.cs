using Hangfire.Console;
using Hangfire.Server;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulerTalk.Hubs;
using SchedulerTalk.Models;
using SchedulerTalk.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchedulerTalk.Jobs
{
    public class ProcessWidgetJob
    {
        private readonly JobLogger _logger;
        private readonly WidgetService _service;

        public ProcessWidgetJob(ILogger<ProcessWidgetJob> logger, WidgetService service)
        {
            _logger = new JobLogger(logger);
            _service = service;
        }

        public async Task Execute(PerformContext context, int id)
        {
            _logger.LogDebug("Starting job.");

            var widget = await _service.GetAsync(id);

            _logger.LogDebug("Processing Widget #{0} {1}...", widget.Id, widget.Name);

            for (int i = 0; i < 10; i++)
            {
                _logger.LogDebug("Doing Stuff...");
                Thread.Sleep(1000);
            }

            widget.Processing = false;

            await _service.UpdateAsync(widget);

            _logger.LogDebug("Processed Widget #{0} {1}.", widget.Id, widget.Name);

            context.WriteLine("");
        }

        public async Task Execute(int id)
        {
            _logger.LogDebug("Starting job.");

            var widget = await _service.GetAsync(id);

            _logger.LogDebug("Processing Widget #{0} {1}...", widget.Id, widget.Name);

            for (int i = 0; i < 10; i++)
            {
                _logger.LogDebug("Doing Stuff...");
                Thread.Sleep(1000);
            }

            widget.Processing = false;

            await _service.UpdateAsync(widget);

            _logger.LogDebug("Processed Widget #{0} {1}.", widget.Id, widget.Name);
        }
    }
}
