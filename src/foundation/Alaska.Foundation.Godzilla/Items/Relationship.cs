using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Entries;
using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Items
{
    public sealed class Relationship : RelationshipBase, IRelationship
    {
        internal Relationship(
            EntityContext context,
            RelationshipEntryBase relationship)
            : base(context, relationship)
        { }

        public object Data => _relationship.GetData();
    }

    public sealed class Relationship<TRelationship> : RelationshipBase, IRelationship<TRelationship>
    {
        internal Relationship(
            EntityContext context,
            RelationshipEntry<TRelationship> relationship)
            : base(context, relationship)
        { }

        public TRelationship Data => (TRelationship)_relationship.GetData();
    }
}
