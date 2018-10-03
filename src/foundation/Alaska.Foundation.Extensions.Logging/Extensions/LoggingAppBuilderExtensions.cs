using Alaska.Foundation.Extensions.Logging.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class LoggingAppBuilderExtensions
    {
        public static IApplicationBuilder UseLogsDashboard(this IApplicationBuilder app)
        {
            return app.UseSignalR(routes =>
            {
                routes.MapHub<LogsHub>("/hub/logs");
            });
        }
    }
}
