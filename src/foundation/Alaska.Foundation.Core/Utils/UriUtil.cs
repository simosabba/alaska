using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Alaska.Foundation.Core.Utils
{
    public static class UriUtil
    {
        public static string AppendQueryString(string url, string key, string value)
        {
            var separator = url.Contains("?") ? "&" : "?";
            return $"{url}{separator}{key}={value}";
        }

        public static string RemoveUriSegments(string uri, int segments)
        {
            return uri.TrimEnd('/').Split('/').SkipLast(segments).JoinString("/");
        }

        public static int GetUriDepth(string uri)
        {
            return uri.Split('/').Count();
        }

        public static string TakeUriSegments(string uri, int segments)
        {
            return uri.Split('/').Take(segments).JoinString("/");
        }

        public static string GetParentPath(string uri)
        {
            return string.Join("/", uri.TrimEnd('/').Split('/').SkipLast(1));
        }

        public static Uri CombineUri(Uri uriBase, params string[] relativeParts)
        {
            return CombineUri(uriBase.ToString(), relativeParts);
        }

        public static Uri CombineUri(string uriBase, params string[] relativeParts)
        {
            return CreateUri(string.Format("{0}/{1}", uriBase.TrimEnd('/'), string.Join("/", relativeParts.Select(x => x.Trim('/')))));
        }

        public static Uri CombineWithQueryString(Uri baseUri, NameValueCollection parameters)
        {
            return CreateUri(string.Format(baseUri.ToString().TrimEnd('/'), ConvertToQueryString(parameters)));
        }

        public static string ConvertToQueryString(NameValueCollection parameters)
        {
            var param = new List<string>();
            foreach (var key in parameters.AllKeys)
            {
                var values = parameters.GetValues(key);
                if (values == null)
                    continue;
                param.AddRange(values.Select(value => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))));
            }
            return "?" + string.Join("&", param);
        }

        private static Uri CreateUri(string uri)
        {
            return new Uri(uri, UriKind.RelativeOrAbsolute);
        }
    }
}
