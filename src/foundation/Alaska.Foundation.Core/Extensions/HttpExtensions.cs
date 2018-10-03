using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Alaska.Foundation.Core.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var raw = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(raw);
        }
        
        public static void CopyHttpHeaders(this HttpClient client, Dictionary<string, object> headers)
        {
            foreach (var key in headers.Keys)
            {
                client.DefaultRequestHeaders.Add(key, headers[key].ToString());
            }
        }

        public static void CopyHttpHeaders(this HttpClient client, NameValueCollection headers)
        {
            foreach (var key in headers.AllKeys)
            {
                client.DefaultRequestHeaders.Add(key, headers[key]);
            }
        }
    }
}
