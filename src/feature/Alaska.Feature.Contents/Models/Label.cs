﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Feature.Contents.Models
{
    public class Label
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
