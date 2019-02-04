using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchedulerTalk.ViewModels;

namespace SchedulerTalk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetsController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Widget>> Get()
        {
            var list = new List<Widget>()
            {
                new Widget() { Id = 1, Name = "Foggle", Processing = false, DateCreated = DateTime.Now.AddDays(-5) },
                new Widget() { Id = 2, Name = "Woggle", Processing = false, DateCreated = DateTime.Now.AddDays(-4) },
                new Widget() { Id = 3, Name = "Sniffl", Processing = false, DateCreated = DateTime.Now.AddDays(-3) },
                new Widget() { Id = 4, Name = "Groblr", Processing = true, DateCreated = DateTime.Now.AddDays(-2) },
                new Widget() { Id = 5, Name = "Chiggl", Processing = true, DateCreated = DateTime.Now.AddDays(-1) },
            };

            return list;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Widget> Get(int id)
        {
            return new Widget() { Id = 0, Name = "Foggle", DateCreated = DateTime.Now.AddDays(-5) };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Widget value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Widget value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
