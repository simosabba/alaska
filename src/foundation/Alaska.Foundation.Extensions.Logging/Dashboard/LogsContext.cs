using Alaska.Foundation.Extensions.Logging.Dashboard.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Extensions.Logging.Dashboard
{
    public class LogsContext : DbContext
    {
        public LogsContext()
        { }

        //public LogsContext(DbContextOptions<LogsContext> options)
        //    : base(options)
        //{ }

        public DbSet<LogDto> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("alaska-logs");
        }
    }
}
