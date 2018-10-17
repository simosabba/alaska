using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public interface IEntityRelationship
    {
        Guid Id { get; }

        Guid SourceEntityId { get; }

        Guid TargetEntityId { get; }

        Guid RelationshipId { get; }

        DateTime CreationTime { get; }

        DateTime UpdateTime { get; }

        object GetData();
    }

    public interface IEntityRelationshipDetails
    {
        IEntityRelationship Relation { get; }

        IItem SourceEntity { get; }

        IItem TargetEntity { get; }
    }
}
