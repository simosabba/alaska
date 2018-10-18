using Alaska.Foundation.Godzilla.Collections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable.Filters
{
    internal class HierarchyFilter : QueryFilter
    {
        public HierarchyFilter(Expression<Func<HierarchyEntry, bool>> expression)
        {
            Expression = expression;
        }

        public Expression<Func<HierarchyEntry, bool>> Expression { get; private set; }

        public override IEnumerable<Guid> Execute(EntityContext context)
        {
            return context.Hierarchy.GetEntitiesId(Expression);
        }

        public override string Representation => Expression.ToString();
    }
}
