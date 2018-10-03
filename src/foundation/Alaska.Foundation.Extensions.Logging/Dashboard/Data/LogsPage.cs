using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Extensions.Logging.Dashboard.Data
{
    public class LogsPage
    {
        [JsonProperty("data")]
        public List<LogDto> Data { get; set; } = new List<LogDto>();
        
        [JsonProperty("nextPageStartId")]
        public string NextPageStartId { get; set; }
    }
}
