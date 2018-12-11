using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Attributes;
using Alaska.Foundation.Godzilla.Entities.Common;
using Newtonsoft.Json;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Entities.Templates
{
    [Template(GodzillaCore.Templates.Template)]
    public class Template : IEntity, ITemplate
    {
        private JsonSchema4 _schema = null;

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string EntityName { get; set; }

        [JsonProperty("typeInfo")]
        public DataTypeInfo TypeInfo { get; set; }

        [JsonProperty("defaultIcon")]
        public IconInfo DefaultIcon { get; set; }

        [JsonProperty("userIcon")]
        public IconInfo UserIcon { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("typeSchema")]
        public string TypeSchema
        {
            get => _schema?.ToJson();
            set => _schema = string.IsNullOrEmpty(value) ? null : ParseJsonSchema(value);
        }

        public JsonSchema4 GetSchema()
        {
            return _schema;
        }

        public IconInfo GetIcon()
        {
            return UserIcon ?? DefaultIcon;
        }

        public void SetSchema(JsonSchema4 schema)
        {
            _schema = schema;
        }

        private JsonSchema4 ParseJsonSchema(string schema)
        {
            var jsonSchema = JsonSchema4.FromJsonAsync(schema);
            jsonSchema.Wait();
            return jsonSchema.Result;
        }
    }

    public class IconInfo
    {
        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
