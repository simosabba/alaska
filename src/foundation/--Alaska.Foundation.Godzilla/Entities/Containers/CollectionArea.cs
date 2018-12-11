using Alaska.Foundation.Godzilla.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Entities.Containers
{
    [Template(GodzillaCore.Templates.CollectionArea)]
    public class CollectionArea : Area
    {
        public List<string> Entities { get; set; }
    }
}
