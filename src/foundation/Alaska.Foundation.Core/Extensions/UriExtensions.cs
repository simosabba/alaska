using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Alaska.Foundation.Core.Extensions
{
    public static class UriExtensions
    {
        public static Uri GetHostUrl(this Uri value)
        {
            return value.Port == 80 || value.Port == 443 ?
                new Uri(string.Format("{0}://{1}", value.Scheme, value.Host), UriKind.Absolute) :
                new Uri(string.Format("{0}://{1}:{2}", value.Scheme, value.Host, value.Port), UriKind.Absolute);
        }

        public static Uri Combine(this Uri uri, string relarivePath)
        {
            return new Uri(string.Format("{0}/{1}",
                uri.GetHostUrl().ToString().TrimEnd('/'),
                relarivePath.TrimStart('/')));
        }

        public static NameValueCollection GetQueryString(this Uri uri)
        {
            return HttpUtility.ParseQueryString(uri.Query);
        }

        public static string GetQueryStringValue(this Uri uri, string key)
        {
            return uri.GetQueryString()[key];
        }
    }
}
