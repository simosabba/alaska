using Alaska.Foundation.Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Entries
{
    public enum RelationDirection { Monodirectional, Bidirectional }

    public abstract class RelationshipEntryBase :
        IDatabaseCollectionElement,
        IEntityRelationship
    {
        public RelationshipEntryBase()
        { }

        public Guid Id { get; set; }

        public Guid SourceEntityId { get; set; }

        public Guid TargetEntityId { get; set; }

        public Guid RelationshipId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public abstract object GetData();
    }

    public sealed class RelationshipEntry<T> : RelationshipEntryBase
    {
        public T Data { get; set; }

        public override object GetData()
        {
            return Data;
        }
    }
}
