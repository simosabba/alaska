using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Feature.Contents.Models
{
    public class SeoData
    {
        [JsonProperty("metaTitle")]
        public string MetaTitle { get; set; }

        [JsonProperty("metaDescription")]
        public string MetaDescription { get; set; }

        [JsonProperty("canonicalUrl")]
        public string CanonicalUrl { get; set; }

        [JsonProperty("noIndex")]
        public bool NoIndex { get; set; }

        [JsonProperty("noFollow")]
        public bool NoFollow { get; set; }
    }
}
