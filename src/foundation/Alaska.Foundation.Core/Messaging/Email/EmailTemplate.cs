using Alaska.Foundation.Core.Messaging.Email.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Core.Messaging.Email
{
    public class EmailTemplate : IEmail
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public List<string> To { get; set; }

        [JsonProperty("cc")]
        public List<string> Cc { get; set; }

        [JsonProperty("bcc")]
        public List<string> Bcc { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        IEnumerable<string> IEmail.To => To;
        IEnumerable<string> IEmail.Cc => Cc;
        IEnumerable<string> IEmail.Bcc => Bcc;
    }
}
