using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulerTalk.Hubs;
using SchedulerTalk.Models;
using SchedulerTalk.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task Execute(int id)
        {
            var widget = await _service.GetAsync(id);

            _logger.LogDebug("Processing widget {0}...", widget.Name);

            widget.Processing = false;

            await _service.UpdateAsync(widget);
        }
    }
}
