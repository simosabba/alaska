using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Feature.Contents.Models
{
    public abstract class ItemBase
    {
        [JsonProperty("info")]
        public ItemInfo Info { get; set; }
    }
}
