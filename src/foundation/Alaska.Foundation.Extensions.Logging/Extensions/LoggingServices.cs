using Alaska.Foundation.Extensions.Logging.Dashboard;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LoggingServices
    {
        internal static ServiceProvider ServiceProvider { get; private set; }
        
        public static ISignalRServerBuilder AddLoggingExtensions(this IServiceCollection services, Action<LogsOptions> initializer = null)
        {
            var options = new LogsOptions();
            initializer?.Invoke(options);

            services
                .AddMvcCore()
                .AddApplicationPart(typeof(LogsHub).Assembly);

            var s = services
                .AddSingleton(options)
                .AddSingleton<LogsCleanupService>()
                .AddTransient<LogsNotificationService>()
                .AddTransient<LogsRepository>()
                .AddSignalR();

            ServiceProvider = s.Services.BuildServiceProvider();
            return s;
        }
    }
}
