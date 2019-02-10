using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SchedulerTalk.Hubs;
using SchedulerTalk.Models;
using SchedulerTalk.Services;

namespace SchedulerTalk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetsController : ControllerBase
    {
        private readonly WidgetService _service;

        public WidgetsController(WidgetService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Widget>>> Get()
        {
            return await _service.GetListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Widget>> Get(int id)
        {
            return await _service.GetAsync(id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<Widget> Post([FromBody] Widget item)
        {
            return await _service.CreateAsync(item);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Widget>> Put(int id, [FromBody] Widget item)
        {
            return await _service.UpdateAsync(item);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task Delete(int id)
        {
            await _service.DeleteAsync(id);
        }
    }
}
