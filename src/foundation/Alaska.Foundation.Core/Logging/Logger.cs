using Alaska.Foundation.Core.Logging.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Logging
{
    public static class Logger
    {
        private const int DefaultEventId = 0;
        private static readonly ILogger EmptyLogger = new EmptyLogger();

        private static ILoggerFactory _LoggerFactory;

        internal static void RegisterFactory(ILoggerFactory loggerFactory)
        {
            _LoggerFactory = loggerFactory;
        }

        public static ILogger Current => GetLogger(CallerType);

        private static ILogger GetLogger(Type loggerType)
        {
            if (_LoggerFactory != null)
            {
                var logger = _LoggerFactory.CreateLogger(loggerType);
                return logger;
            }

            return EmptyLogger;
        }

        private static Type CallerType => new StackTrace()
            .GetFrame(2)
            .GetMethod().DeclaringType;
    }
}
