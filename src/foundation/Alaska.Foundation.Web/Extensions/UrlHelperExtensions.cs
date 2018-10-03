using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string BuildRelativeUrl(this IUrlHelper helper, string path, object parameters)
        {
            return helper.BuildRelativeUrl(path, ConvertToDictionary(parameters));
        }
        
        public static string BuildRelativeUrl(this IUrlHelper helper, string path, IDictionary<string, object> parameters)
        {
            return BuildUrl(helper, path, parameters, string.Empty);
        }

        public static string BuildRelativeUrl(this IUrlHelper helper, string path, object parameters, HttpContext context)
        {
            return helper.BuildAbsoluteUrl(path, ConvertToDictionary(parameters), context);
        }

        public static string BuildAbsoluteUrl(this IUrlHelper helper, string path, IDictionary<string, object> parameters, HttpContext context)
        {
            return BuildUrl(helper, path, parameters, context.Request.Host.Value);
        }

        public static string BuildUrl(this IUrlHelper helper, string path, IDictionary<string, object> parameters, string host)
        {
            var queryString = parameters == null ? string.Empty : "?" + string.Join("&", parameters.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value?.ToString())}"));
            return $"{host.TrimEnd('/')}/{path.TrimEnd('?').TrimStart('/')}{queryString}";
        }

        public static string BuildUrl(this IUrlHelper helper, string path, IDictionary<string, object> parameters)
        {
            var queryString = parameters == null ? string.Empty : "?" + string.Join("&", parameters.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value?.ToString())}"));
            return $"{path.TrimEnd('?')}{queryString}";
        }

        public static Dictionary<string, object> ConvertToDictionary(object parameters)
        {
            var dict = new Dictionary<string, object>();
            if (parameters != null)
            {
                var p = parameters.GetType().GetProperties();
                foreach (var property in p)
                    dict.Add(property.Name, property.GetValue(parameters, null));
            }
            return dict;
        }
    }
}
