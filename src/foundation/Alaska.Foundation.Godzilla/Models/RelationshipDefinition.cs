using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Models
{
    internal class RelationshipDefinition : IRelationshipDefinition
    {
        public Guid Id { get; set; }
        public RelationDirection Direction { get; set; }
    }
}
