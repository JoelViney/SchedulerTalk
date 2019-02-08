using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerTalk.Models
{
    public class Widget
    {
        public int Id { get; set; }

        [MaxLength(254)]
        public string Name { get; set; }

        public bool Processing { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
