using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Extensions.Logging.Dashboard.Data
{
    [Owned]
    public class ExceptionDto
    {
        [JsonProperty("stackTrace")]
        public string StackTrace { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("innerException")]
        public ExceptionDto InnerException { get; set; }
    }
}
