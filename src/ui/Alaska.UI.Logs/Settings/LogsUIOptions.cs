using Alaska.Foundation.Web.Middleware;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Alaska.UI.Logs.Settings
{
    public class LogsUIOptions : IAngularUIOptions
    {
        public string RoutePrefix { get; set; } = "alaska/logs";
        
        public string ManifestResourceBasePath { get; set; } = "Alaska.UI.Logs.angular.dist.alaska_logs";

        public Assembly ManifestResourceAssembly { get; set; } = typeof(LogsUIOptions).GetTypeInfo().Assembly;

        public List<string> Endpoints { get; set; } = new List<string>();

        public string DocumentTitle { get; set; } = "Alaska Logs UI";

        public string HeadContent { get; set; } = "";

        public JObject ConfigObject { get; } = JObject.FromObject(new
        {
            urls = new object[] { },
            validatorUrl = JValue.CreateNull()
        });

        public JObject OAuthConfigObject { get; } = JObject.FromObject(new
        {
        });
    }
}
