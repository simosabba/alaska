using Alaska.Foundation.Core.Utils;
using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable
{
    public class EntityContextQueryProvider : IQueryProvider
    {
        private ItemQueryContext _queryContext = new ItemQueryContext();
        private EntityContext _context;

        public EntityContextQueryProvider(EntityContext context)
        {
            _context = context;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(EntityContextQueryableData<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        // Queryable's collection-returning standard query operators call this method. 
        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return new EntityContextQueryableData<TResult>(this, expression);
        }

        public object Execute(Expression expression)
        {
            return _queryContext.Execute(expression, false, _context);
        }

        // Queryable's "single value" standard query operators call this method.
        // It is also called from QueryableTerraServerData.GetEnumerator(). 
        public TResult Execute<TResult>(Expression expression)
        {
            if (typeof(TResult) == typeof(IQueryableItem))
                return (TResult)_queryContext.Execute(expression, false, _context);

            if (typeof(TResult) == typeof(IEnumerable<IQueryableItem>))
                return (TResult)_queryContext.Execute(expression, true, _context);

            var isEnumerable = (typeof(TResult).Name == "IEnumerable`1");
            if (isEnumerable)
            {
                var entityType = typeof(TResult).GenericTypeArguments.FirstOrDefault().GenericTypeArguments.FirstOrDefault();
                if (entityType != null)
                    return (TResult)_queryContext.Execute(expression, true, _context, entityType);
            }
            else
            {
                var entityType = typeof(TResult).GenericTypeArguments.FirstOrDefault();
                if (entityType != null)
                    return (TResult)_queryContext.Execute(expression, false, _context, entityType);
            }

            throw new InvalidOperationException($"Invalid result type {typeof(TResult).FullName}");
        }
    }
}
