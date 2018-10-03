using Alaska.Foundation.Extensions.Logging.Dashboard;
using Alaska.Foundation.Extensions.Logging.Sinks.Data;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Alaska.Foundation.Extensions.Logging.Sinks
{
    public class DashboardSink : ILogEventSink
    {
        private readonly DashboardSinkOptions _options;
        private readonly DashboardSinkLogsConverter _converter;
        
        private LogsRepository LogsRepository => LoggingServices.ServiceProvider.GetService<LogsRepository>();

        public DashboardSink(DashboardSinkOptions options)
        {
            _options = options;
            _converter = new DashboardSinkLogsConverter(options);
        }

        public void Emit(LogEvent logEvent)
        {
            var log = _converter.ConvertToLogDto(logEvent);
            LogsRepository.WriteAsync(log).ConfigureAwait(false);
        }
    }
}
