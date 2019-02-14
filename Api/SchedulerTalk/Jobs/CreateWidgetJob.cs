using Hangfire;
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
    public class CreateWidgetJob
    {
        private static Random _random = new Random();

        private readonly JobLogger _logger;
        private readonly WidgetService _service;

        public CreateWidgetJob(ILogger<CreateWidgetJob> logger, WidgetService service)
        {
            _logger = new JobLogger(logger);
            _service = service;
        }

        public async Task Execute(PerformContext context)
        {
            _logger.HangfireContext = context;

            _logger.LogDebug("Starting job...");

            var prefixes = new string[] { "Fugg", "Dog", "Cheek", "Moo", "Bob", "Choo", "Zee", "Wagh", "Chomp" };
            var suffixes = new string[] { "ed", "er", "ington", "ssssss", "oes", "choo", "lah", "gh", "-alot" };

            var name = $"{prefixes[_random.Next(prefixes.Length)]}{suffixes[_random.Next(suffixes.Length)]}";


            var item = new Widget()
            {
                Name = name,
                Processing = true,
                DateCreated = DateTime.Now
            };

            for (int i = 0; i < 10; i++)
            {
                _logger.LogDebug("Creating widget {0}...", item.Name);
                Thread.Sleep(1000);
            }

            var widget = await _service.CreateAsync(item);

            context.WriteLine("Job done 3");
            context.WriteLine("Job done 2");
            _logger.LogDebug("Job done 1");
            context.WriteLine("");
        }
    }
}
