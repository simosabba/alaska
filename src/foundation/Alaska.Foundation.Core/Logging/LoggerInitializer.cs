using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Core.Logging
{
    public static class LoggerInitializer
    {
        public static void RegisterFactory(ILoggerFactory loggerFactory)
        {
            Logger.RegisterFactory(loggerFactory);
        }
    }
}
