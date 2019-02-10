using Hangfire;
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
    public class CreateWidgetJob
    {
        private static Random _random = new Random();

        private readonly ILogger<CreateWidgetJob> _logger;
        private readonly WidgetService _service;

        public CreateWidgetJob(ILogger<CreateWidgetJob> logger, WidgetService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task Execute()
        {
            var prefixes = new string[] { "Fugg", "Dog", "Cheek", "Moo", "Bob", "Choo", "Zee", "Wagh", "Chomp" };
            var suffixes = new string[] { "ed", "er", "ington", "ssssss", "oes", "choo", "lah", "gh", "-alot" };

            var name = $"{prefixes[_random.Next(prefixes.Length)]}{suffixes[_random.Next(suffixes.Length)]}";

            var item = new Widget()
            {
                Name = name,
                Processing = true,
                DateCreated = DateTime.Now
            };

            _logger.LogDebug("Creating widget {0}...", item.Name);

            var widget = await _service.CreateAsync(item);
        }
    }
}
