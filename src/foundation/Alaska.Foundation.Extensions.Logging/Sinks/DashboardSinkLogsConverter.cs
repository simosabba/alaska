using Alaska.Foundation.Extensions.Logging.Dashboard.Data;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Extensions.Logging.Sinks.Data
{
    internal class DashboardSinkLogsConverter
    {
        private DashboardSinkOptions _options;

        public DashboardSinkLogsConverter(DashboardSinkOptions options)
        {
            _options = options;
        }

        public LogDto ConvertToLogDto(LogEvent logEvent)
        {
            return new LogDto
            {
                ApplicationId = _options.ApplicationId,
                Timestamp = logEvent.Timestamp,
                Level = logEvent.Level.ToString(),
                Message = logEvent.RenderMessage(_options.FormatProvider),
                Exception = ConvertToExceptionDto(logEvent.Exception),
            };
        }

        public ExceptionDto ConvertToExceptionDto(Exception exception)
        {
            if (exception == null)
                return null;

            return new ExceptionDto
            {
                Message = exception.Message,
                Source = exception.Source,
                StackTrace = exception.StackTrace,
                InnerException = ConvertToExceptionDto(exception.InnerException),
            };
        }
    }
}
