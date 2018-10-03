using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Feature.Contents.Models
{
    public class LabelPage
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("culture")]
        public Culture Culture { get; set; }

        [JsonProperty("labels")]
        public IEnumerable<Label> Labels { get; set; }
    }
}
