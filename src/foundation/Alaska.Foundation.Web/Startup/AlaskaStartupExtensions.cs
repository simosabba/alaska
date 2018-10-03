using Alaska.Foundation.Core.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Web.Startup
{
    public static class AlaskaStartupExtensions
    {
        public static IApplicationBuilder UseAlaska(this IApplicationBuilder app)
        {
            LoggerInitializer.RegisterFactory(app.ApplicationServices.GetRequiredService<ILoggerFactory>());

            return app;
        }
    }
}
