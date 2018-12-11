using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Models
{
    internal class EntityRelationshipData : IEntityRelationshipDetails
    {
        public IEntityRelationship Relation { get; set; }

        public IItem SourceEntity { get; set; }

        public IItem TargetEntity { get; set; }
    }
}
