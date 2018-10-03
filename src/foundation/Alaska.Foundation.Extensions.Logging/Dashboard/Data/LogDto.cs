using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Extensions.Logging.Dashboard.Data
{
    public class LogDto
    {
        [JsonProperty("logId")]
        public string Id { get; set; }

        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; }

        [JsonProperty("timeStamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("exception")]
        public ExceptionDto Exception { get; set; }
    }
}
