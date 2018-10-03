using Alaska.UI.Logs.Extensions;
using Alaska.UI.Logs.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
    public static class LogsUIApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAlaskaLogsUI(this IApplicationBuilder app, Action<LogsUIOptions> setupAction = null)
        {
            var options = new LogsUIOptions();
            setupAction?.Invoke(options);

            LogsUIOptionsRepository.Options = options;

            app.UseMiddleware<LogsUIIndexMiddleware>(options);

            return app;
        }
    }
}
