using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Abstractions
{
    public enum RelationDirection { Monodirectional, Bidirectional }

    public interface IRelationshipDefinition
    {
        Guid Id { get; }
        RelationDirection Direction { get; }
    }
}
