using Alaska.Foundation.Web.Middleware;
using Alaska.UI.Logs.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.UI.Logs.Extensions
{
    public class LogsUIIndexMiddleware : AngularUIPluginMiddleware
    {
        public LogsUIIndexMiddleware(RequestDelegate next, LogsUIOptions options)
            : base(next, options)
        { }

    }
}
