using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Entities.Common
{
    public class DataTypeInfo
    {
        private Type _type;

        [JsonProperty("assemblyQualifiedName")]
        public string AssemblyQualifiedName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isAbstract")]
        public bool IsAbstract { get; set; }

        [JsonIgnore]
        public Type Type
        {
            get
            {
                if (_type == null)
                    _type = Type.GetType(AssemblyQualifiedName);
                return _type;
            }
        }
    }
}
