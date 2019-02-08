using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchedulerTalk.Models;

namespace SchedulerTalk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetsController : ControllerBase
    {
        private DatabaseContext _context;

        public WidgetsController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Widget>> Get()
        {
            var list = _context.Widgets.ToList();
            return list;
        }

        [HttpGet("{id}")]
        public ActionResult<Widget> Get(int id)
        {
            var item = _context.Widgets.First(x => x.Id == id);
            return item;
        }

        [HttpPost]
        public void Post([FromBody] Widget value)
        {
            _context.Widgets.Add(value);
            _context.SaveChanges();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Widget value)
        {
            var widget = _context.Widgets.First(x => x.Id == id);

            widget.Name = value.Name;
            widget.Processing = value.Processing;

            _context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var widget = _context.Widgets.First(x => x.Id == id);
            _context.Widgets.Remove(widget);
            _context.SaveChanges();
        }
    }
}
