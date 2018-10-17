using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Entities.Containers
{
    [Template(GodzillaCore.Templates.ContainerBase)]
    public abstract class ContainerBase : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
