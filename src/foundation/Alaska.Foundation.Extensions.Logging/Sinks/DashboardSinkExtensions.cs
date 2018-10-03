using Alaska.Foundation.Extensions.Logging.Sinks;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog
{
    public static class DashboardSinkExtensions
    {
        public static LoggerConfiguration DashboardSink(
                  this LoggerSinkConfiguration loggerConfiguration,
                  Action<DashboardSinkOptions> setupAction = null)
        {
            var options = new DashboardSinkOptions();
            setupAction?.Invoke(options);

            return loggerConfiguration.Sink(new DashboardSink(options));
        }
    }
}
