using Alaska.Foundation.Core.Logging;
using Alaska.Foundation.Extensions.Logging.Dashboard.Data;
using Alaska.Foundation.Extensions.Logging.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alaska.Foundation.Extensions.Logging.Dashboard
{
    public enum LogsSortOrder { Asc, Desc }

    public class LogsRepository
    {
        private readonly LogsOptions _options;
        private readonly LogsNotificationService _notificationService;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public LogsRepository(
            LogsOptions options,
            LogsNotificationService notificationService)
        {
            _options = options;
            _notificationService = notificationService;
        }
        
        public async Task WriteAsync(LogDto log)
        {
            try
            {
                using (var logsContext = NewLogsContext())
                {
                    await logsContext.Logs.AddAsync(log);
                    await logsContext.SaveChangesAsync();
                }

                await _notificationService.SendLogAsync(log);
            }
            catch (Exception e)
            {
                throw new LogsRepositoryException("Error writing log", e);
            }
        }
        public LogsPage GetFirstLogsPage(int pageSize)
        {
            return GetLogsPage(null, pageSize);
        }
        
        public LogsPage GetLogsPage(string lastId, int pageSize)
        {
            try
            {
                using (var logsContext = NewLogsContext())
                {
                    DateTimeOffset? to = null;
                    if (!string.IsNullOrEmpty(lastId))
                    {
                        var lastLog = logsContext.Logs
                            .FirstOrDefault(x => x.Id == lastId);
                        if (lastLog == null)
                            return new LogsPage();
                        to = lastLog.Timestamp;
                    }
                    
                    var logsQuery = logsContext.Logs.AsQueryable();
                    if (to.HasValue)
                        logsQuery = logsQuery.Where(x => x.Timestamp <= to);

                    var results = logsQuery
                        .OrderByDescending(x => x.Timestamp)
                        .Take(pageSize + 1)
                        .ToList();

                    if (results.Count < pageSize)
                        return new LogsPage
                        {
                            Data = results
                        };

                    return new LogsPage
                    {
                        Data = results.Take(pageSize).ToList(),
                        NextPageStartId = results.Last().Id,
                    };
                }
            }
            catch (Exception e)
            {
                throw new LogsRepositoryException($"Error searching log page {lastId ?? string.Empty} - size {pageSize}", e);
            }
        }

        public void CleanupLogs()
        {
            try
            {
                using (var logsContext = NewLogsContext())
                {
                    var minAge = DateTime.UtcNow.Add(-_options.Retention);

                    var expiredLogs = logsContext.Logs
                        .Where(x => x.Timestamp <= minAge)
                        .ToList();
                    logsContext.Logs.RemoveRange(expiredLogs);
                    logsContext.SaveChanges();

                    var recordsToDelete = logsContext.Logs.Count() - _options.MaxSize;
                    if (recordsToDelete > 0)
                    {
                        var exceedingLogs = logsContext.Logs
                            .OrderBy(x => x.Timestamp)
                            .Take(recordsToDelete)
                            .ToList();
                        logsContext.Logs.RemoveRange(exceedingLogs);
                        logsContext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Current.LogError(e, "Error deleting logs");
            }
        }

        private LogsContext NewLogsContext()
        {
            return new LogsContext();
        }
    }
}
