using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Extensions.Logging.Dashboard
{
    public class LogsOptions
    {
        public TimeSpan CleanupInterval { get; set; } = TimeSpan.FromMinutes(1);
        public TimeSpan Retention { get; set; } = TimeSpan.FromMinutes(30);
        public int MaxSize { get; set; } = 100000;
    }
}
