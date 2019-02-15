using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerTalk.Jobs;
using SchedulerTalk.Services;

namespace SchedulerTalk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly WidgetService _service;

        public JobsController(WidgetService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> Post(string jobName)
        {
            if (jobName.ToLower() == "create")
            {
                var jobId = BackgroundJob.Enqueue<CreateWidgetJob>(x => x.Execute(null));
                return Ok();
            }
            else if (jobName.ToLower() == "process")
            {
                var result = await _service.ProcessOneAsync();
                return Ok(result);
            }

            return NotFound();
        }

    }
}