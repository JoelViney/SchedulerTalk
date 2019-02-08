using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerTalk.Models
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Widget> Widgets { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

    }
}
