using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, LogLevel level, string message, params object[] args)
        {
            switch(level)
            {
                case LogLevel.Critical:
                    logger.LogCritical(message, args);
                    break;
                case LogLevel.Debug:
                    logger.LogDebug(message, args);
                    break;
                case LogLevel.Error:
                    logger.LogError(message, args);
                    break;
                case LogLevel.Information:
                    logger.LogInformation(message, args);
                    break;
                case LogLevel.Warning:
                    logger.LogWarning(message, args);
                    break;
                case LogLevel.None:
                    break;
                default:
                case LogLevel.Trace:
                    logger.LogTrace(message, args);
                    break;
            }
        }
    }
}
