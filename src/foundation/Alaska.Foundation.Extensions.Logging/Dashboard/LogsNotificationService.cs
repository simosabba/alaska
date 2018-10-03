using Alaska.Foundation.Core.Extensions;
using Alaska.Foundation.Extensions.Logging.Dashboard.Data;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Extensions.Logging.Dashboard
{
    public class LogsNotificationService
    {
        private IHubContext<LogsHub> _logsHub;

        public LogsNotificationService(IHubContext<LogsHub> logsHub)
        {
            _logsHub = logsHub;
        }

        public async Task SendLogAsync(LogDto log)
        {
            try
            {
                await _logsHub.Clients.All.SendAsync("receiveNewLog", log);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending log {(log != null ? JsonConvert.SerializeObject(log) : "null")}");
                Console.WriteLine(e.ToDetailedString());
            }
            
        }
    }
}
