using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class AngularAppMiddlewareExtensions
    {
        public static IApplicationBuilder UseAngularAppMiddleware(this IApplicationBuilder app, Action<AngularAppOptions> setupAction = null)
        {
            var options = new AngularAppOptions();
            setupAction?.Invoke(options);

            return app.Use(async (context, next) =>
            {
                await next();

                // If there's no available file and the request doesn't contain an extension, we're probably trying to access a page.
                // Rewrite request to use app root
                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !options.PathsToExclude.Any(x => context.Request.Path.Value.StartsWith(x, StringComparison.InvariantCultureIgnoreCase))
                    )
                {
                    await RespondWithFileContent(context.Response, options.AppIndexFile, options.InlineSettings);
                }
            });
        }

        private static async Task RespondWithFileContent(HttpResponse response, string filePath, object inlineSettings)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html";

            var content = File.ReadAllText(filePath);
            if (inlineSettings == null)
            {
                await response.WriteAsync(content, Encoding.UTF8);
                return;
            }

            var htmlDocument = CreateHtmlDocument(content, inlineSettings);
            await response.WriteAsync(htmlDocument, Encoding.UTF8);
        }

        private static string CreateHtmlDocument(string originalDocument, object inlineSettings)
        {
            try
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(originalDocument);
                var inlineSettingsNode = htmlDocument.CreateElement("script");
                inlineSettingsNode.InnerHtml = $"var settings = {JsonConvert.SerializeObject(inlineSettings)};";

                var body = htmlDocument.DocumentNode.SelectSingleNode("//body");
                var currentBodyHtml = body.OuterHtml;
                body.PrependChild(inlineSettingsNode);

                return htmlDocument.Text.Replace(currentBodyHtml, body.OuterHtml);
            }
            catch (Exception e)
            {
                return originalDocument;
            }
        }
    }

    public class AngularAppOptions
    {
        public string AppIndexFile { get; set; } = "wwwroot/app/index.html";

        public string BaseUrl { get; set; } = "/";

        public List<string> PathsToExclude { get; set; } = new List<string> { "/api", "/alaska", "swagger" };

        public object InlineSettings { get; set; }
    }
}
