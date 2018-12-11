using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable.Filters
{
    internal class EntityFilter<TEntity> : QueryFilter
        where TEntity : IEntity
    {
        public EntityFilter(Expression<Func<TEntity, bool>> expression)
        {
            Expression = expression;
        }

        public Expression<Func<TEntity, bool>> Expression { get; private set; }

        public override IEnumerable<Guid> Execute(EntityContext context)
        {
            return context.AsQueryable<TEntity>()
                .Where(Expression)
                .Select(x => x.Id)
                .ToList();
        }

        public override string Representation => Expression.ToString();
    }
}
