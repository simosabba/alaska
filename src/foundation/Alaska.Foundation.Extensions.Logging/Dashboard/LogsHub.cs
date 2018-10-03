using Alaska.Foundation.Core.Extensions;
using Alaska.Foundation.Core.Logging;
using Alaska.Foundation.Extensions.Logging.Dashboard.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Extensions.Logging.Dashboard
{
    public class LogsHub : Hub
    {
        private LogsRepository _logsRepository;

        public LogsHub(LogsRepository logsRepository)
        {
            _logsRepository = logsRepository;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                await base.OnConnectedAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection error");
                Console.WriteLine(e.ToDetailedString());
            }
        }

        public async Task GetFirstLogsPage(int pageSize)
        {
            try
            {
                var logs = _logsRepository.GetFirstLogsPage(pageSize);
                await Clients.Caller.SendAsync("receiveFirstLogsPage", logs);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending first logs page");
                Console.WriteLine(e.ToDetailedString());
            }
        }

        public async Task GetLogsPage(string lastLogId, int pageSize)
        {
            try
            {
                var logs = _logsRepository.GetLogsPage(lastLogId, pageSize);
                await Clients.Caller.SendAsync("receiveLogsPage", logs);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending logs page");
                Console.WriteLine(e.ToDetailedString());
            }
        }
    }
}
