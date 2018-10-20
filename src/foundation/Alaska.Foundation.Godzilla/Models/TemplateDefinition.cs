using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Entities.Common;
using Alaska.Foundation.Godzilla.Entities.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Models
{
    internal class TemplateDefinition : ITemplate
    {
        public string EntityName { get; set; }

        public DataTypeInfo TypeInfo { get; set; }

        public IconInfo DefaultIcon { get; set; }

        public IconInfo UserIcon { get; set; }

        public bool IsActive { get; set; }

        public string TypeSchema { get; set; }
    }
}
