using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable
{
    //source: https://msdn.microsoft.com/en-us/library/bb546158.aspx

    public class EntityContextQueryableData<TData> : IQueryable<TData>, IOrderedQueryable<TData>
    {
        public EntityContextQueryableData(EntityContext context)
        {
            Provider = new EntityContextQueryProvider(context);
            Expression = Expression.Constant(this);
        }

        public EntityContextQueryableData(EntityContextQueryProvider provider, Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (!typeof(IQueryable<TData>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }

            Provider = provider ?? throw new ArgumentNullException("provider");
            Expression = expression;
        }

        public Type ElementType => typeof(TData);

        public IQueryProvider Provider { get; private set; }
        public Expression Expression { get; private set; }

        public IEnumerator<TData> GetEnumerator()
        {
            return (Provider.Execute<IEnumerable<TData>>(Expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (Provider.Execute<System.Collections.IEnumerable>(Expression)).GetEnumerator();
        }
    }
}
