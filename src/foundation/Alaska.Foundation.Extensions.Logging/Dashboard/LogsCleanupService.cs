using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Alaska.Foundation.Extensions.Logging.Dashboard
{
    internal class LogsCleanupService
    {
        private readonly LogsOptions _options;
        private readonly LogsRepository _logsRepository;
        private readonly Timer _cleanupTimer;

        public LogsCleanupService(
            LogsOptions options,
            LogsRepository logsRepository)
        {
            _cleanupTimer = new Timer(
                x => _logsRepository.CleanupLogs(),
                null,
                _options.CleanupInterval,
                _options.CleanupInterval);
        }
    }
}
