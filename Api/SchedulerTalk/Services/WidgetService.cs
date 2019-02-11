using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulerTalk.Hubs;
using SchedulerTalk.Jobs;
using SchedulerTalk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerTalk.Services
{
    public class WidgetService
    {
        private readonly ILogger<WidgetService> _logger;
        private DatabaseContext _context;
        private IHubContext<WidgetHub> _hubContext;

        public WidgetService(ILogger<WidgetService> logger, DatabaseContext context, IHubContext<WidgetHub> hubContext)
        {
            _logger = logger;
            _context = context;
            _hubContext = hubContext;
        }


        public async Task<ActionResult<List<Widget>>> GetListAsync()
        {
            var list = await _context.Widgets.OrderByDescending(o => o.DateCreated).ToListAsync();
            return list;

        }

        public async Task<Widget> GetAsync(int id)
        {
            var item = await _context.Widgets.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                throw new NotFoundException("Failed to load the specified Widget.");
            }

            return item;
        }

        public async Task<Widget> CreateAsync(Widget item)
        {
            await _context.Widgets.AddAsync(item);

            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("Create", item);

            var jobId = BackgroundJob.Schedule<ProcessWidgetJob>(x => x.Execute(item.Id), TimeSpan.FromSeconds(30));

            return item;
        }

        public async Task<Widget> UpdateAsync(Widget item)
        {
            var widget = await this.GetAsync(item.Id);

            widget.Name = item.Name;
            widget.Processing = item.Processing;

            // Item is already updated...
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("Update", item);

            var jobId = BackgroundJob.Schedule<ProcessWidgetJob>(x => x.Execute(item.Id), TimeSpan.FromSeconds(10));

            return widget;
        }

        public async Task DeleteAsync(int id)
        {
            var widget = await this.GetAsync(id);
            _context.Widgets.Remove(widget);
            await _context.SaveChangesAsync();
        }
    }
}
