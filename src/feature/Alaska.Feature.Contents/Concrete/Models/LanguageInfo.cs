using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Feature.Contents.Concrete.Models
{
    internal class LanguageInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
