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
        private readonly ILogger<ProcessWidgetJob> _logger;
        private readonly WidgetService _service;

        public ProcessWidgetJob(ILogger<ProcessWidgetJob> logger, WidgetService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task Execute(PerformContext context, int id)
        {
            context.WriteLine("Starting job.");
            _logger.LogDebug("Starting job.");

            var widget = await _service.GetAsync(id);

            for (int i = 0; i < 10; i++)
            {
                context.WriteLine("Processing widget {0}...", widget.Name);
                _logger.LogDebug("Processing widget {0}...", widget.Name);
                Thread.Sleep(1000);
            }

            widget.Processing = false;

            await _service.UpdateAsync(widget);

            context.WriteLine("Job done.");
            context.WriteLine("Job done.");
            context.WriteLine("Job done.");
            _logger.LogDebug("Job done.");
            Thread.Sleep(1000);
        }

        public async Task Execute(int id)
        {
            _logger.LogDebug("Starting job.");

            var widget = await _service.GetAsync(id);

            for (int i = 0; i < 10; i++)
            {
                _logger.LogDebug("Processing widget {0}...", widget.Name);
                Thread.Sleep(1000);
            }

            widget.Processing = false;

            await _service.UpdateAsync(widget);
        }
    }
}
