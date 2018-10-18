using Alaska.Foundation.Godzilla.Collections;
using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable.Filters
{
    public enum UnaryModifier { Not }

    internal abstract class QueryFilter : QueryNode
    {
        public abstract IEnumerable<Guid> Execute(EntityContext context);
    }
}
