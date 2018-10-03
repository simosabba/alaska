using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TResponse> GetJsonAsync<TResponse>(this HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Error response {url} -> {response.StatusCode}");
            return await response.Content.ReadAsJsonAsync<TResponse>();
        }
    }
}
