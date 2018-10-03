using Alaska.Foundation.Core.Caching.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Caching.Model
{
    internal class CacheEntryInfo: ICacheItemInfo
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("expiration")]
        public TimeSpan Expiration { get; set; }

        [JsonProperty("expirationTime")]
        public DateTime ExpirationTime { get; set; }

        [JsonProperty("expirationTimeStr")]
        public string ExpirationTimeStr { get { return ExpirationTime.ToString("yyyy/MM/dd HH:mm:ss"); } }
    }
}
