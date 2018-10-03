using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Feature.Contents.Models
{
    public class Page : ContentBase
    {
        [JsonProperty("seo")]
        public SeoData Seo { get; set; }
    }
}
