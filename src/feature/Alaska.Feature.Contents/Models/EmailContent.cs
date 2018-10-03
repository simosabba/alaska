using Alaska.Foundation.Core.Messaging.Email;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Feature.Contents.Models
{
    public class EmailContent : ItemBase
    {
        [JsonProperty("template")]
        public EmailTemplate Template { get; set; }
    }
}
