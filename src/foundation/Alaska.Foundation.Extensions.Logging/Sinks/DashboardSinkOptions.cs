using Alaska.Foundation.Extensions.Logging.Dashboard;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Alaska.Foundation.Extensions.Logging.Sinks
{
    public class DashboardSinkOptions
    {
        public string ApplicationId { get; set; } = GetDefaultApplicationId();
        public IFormatProvider FormatProvider { get; set; }
        public bool Disabled { get; set; } = false;
        public LogsOptions BufferOptions { get; set; } = new LogsOptions();

        private static string GetDefaultApplicationId()
        {
            return Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}
