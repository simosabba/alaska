using Alaska.Foundation.Web.Middleware;
using Alaska.UI.Cache.Settings;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alaska.UI.Cache.Extensions
{
    public class CacheUIIndexMiddleware : AngularUIPluginMiddleware
    {
        //private readonly RequestDelegate _next;
        //private readonly CacheUIOptions _options;

        public CacheUIIndexMiddleware(RequestDelegate next, CacheUIOptions options)
            : base(next, options)
        {
            //_next = next;
            //_options = options;
        }

        //public async Task Invoke(HttpContext httpContext)
        //{
        //    var httpMethod = httpContext.Request.Method;
        //    var path = httpContext.Request.Path.Value;

        //    // If the RoutePrefix is requested (with or without trailing slash), redirect to index URL
        //    if (Regex.IsMatch(path, $"^/{_options.RoutePrefix}/?$"))
        //    {
        //        await RespondWithIndexHtml(httpContext.Response);
        //        return;
        //    }

        //    if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{_options.RoutePrefix}/?"))
        //    {
        //        var relativeContentPath = path.Substring(_options.RoutePrefix.Length + 1);
        //        await RespondWithEmbeddedContent(httpContext.Response, relativeContentPath);
        //        return;
        //    }

        //    await _next(httpContext);
        //    return;
        //}

        //private void RespondWithRedirect(HttpResponse response, string redirectPath)
        //{
        //    response.StatusCode = 301;
        //    response.Headers["Location"] = redirectPath;
        //}

        //private async Task RespondWithEmbeddedContent(HttpResponse response, string relativeContentPath)
        //{
        //    response.StatusCode = 200;
        //    response.ContentType = "text/html";

        //    var content = GetEmbeddedResource(relativeContentPath);
        //    await response.WriteAsync(content, Encoding.UTF8);
        //}

        //private async Task RespondWithIndexHtml(HttpResponse response)
        //{
        //    response.StatusCode = 200;
        //    response.ContentType = "text/html";

        //    // Inject parameters before writing to response
        //    var content = GetEmbeddedResource("index.html");
        //    var htmlBuilder = new StringBuilder(content);
        //    foreach (var entry in GetIndexParameters())
        //    {
        //        htmlBuilder.Replace(entry.Key, entry.Value);
        //    }

        //    await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
        //}

        //private string GetEmbeddedResource(string relativeResourcePath)
        //{
        //    var manifestResourcePath = $"{_options.ManifestResourceBasePath}.{relativeResourcePath.TrimStart('/').Replace("-", "_").Replace("/", ".")}";

        //    using (var stream = _options.ManifestResourceAssembly.GetManifestResourceStream(manifestResourcePath))
        //    using (var reader = new StreamReader(stream))
        //    {
        //        var sb = new StringBuilder(reader.ReadToEnd());
        //        return sb.ToString();
        //    }
        //}

        //private IDictionary<string, string> GetIndexParameters()
        //{
        //    return new Dictionary<string, string>()
        //    {
        //        { "%(DocumentTitle)", _options.DocumentTitle },
        //        { "%(HeadContent)", _options.HeadContent },
        //        { "%(ConfigObject)", SerializeToJson(_options.ConfigObject) },
        //        { "%(OAuthConfigObject)", SerializeToJson(_options.OAuthConfigObject) }
        //    };
        //}

        //private string SerializeToJson(JObject jObject)
        //{
        //    return JsonConvert.SerializeObject(jObject, new JsonSerializerSettings
        //    {
        //        NullValueHandling = NullValueHandling.Include,
        //        Formatting = Formatting.None
        //    });
        //}
    }
}
