using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Alaska.Feature.Contents.Models
{
    public class ContentBase : ItemBase
    {
        [JsonProperty("template")]
        public ContentTemplate Template { get; set; }
        
        //[JsonProperty("rendering")]
        //public Rendering Rendering { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; }
    }
}
