using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Feature.Contents.Models
{
    public class Rendering
    {
        [JsonProperty("html")]
        public string Html { get; set; }

        [JsonProperty("js")]
        public string Js { get; set; }

        [JsonProperty("css")]
        public string Css { get; set; }
    }
}
