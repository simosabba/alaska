using Alaska.Foundation.Godzilla.Attributes;
using Alaska.Foundation.Godzilla.Entries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Entities.Templates
{
    [Relationship(GodzillaCore.Relationships.DerivedFrom, RelationDirection.Monodirectional)]
    public class DerivedFrom
    {
    }
}
