using Alaska.Foundation.Core.Utils;
using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable
{
    internal class ItemQueryContext
    {
        internal object Execute(Expression expression, bool isEnumerable, EntityContext context)
        {
            return ExecutePrivate<IQueryableItem>(expression, isEnumerable, x => Find(context, x));
        }

        internal object Execute(Expression expression, bool isEnumerable, EntityContext context, Type entityType)
        {
            var executeGenericMethod = ReflectionUtil.GetGenericMethod(this.GetType(), BindingFlags.Instance | BindingFlags.NonPublic, "Execute");
            var executeMethod = executeGenericMethod.MakeGenericMethod(entityType);
            return executeMethod.Invoke(this, new object[] { expression, isEnumerable, context });
        }

        internal object Execute<TEntity>(Expression expression, bool isEnumerable, EntityContext context)
            where TEntity : IEntity
        {
            return ExecutePrivate(expression, isEnumerable, x => Find<TEntity>(context, x));
        }

        private IEnumerable<IQueryableItem> Find(EntityContext context, ItemFinder finder)
        {
            var queryTree = finder.QueryTree;
            var executor = new ItemQueryExecutor(context);
            var items = executor.FindItems(queryTree);
            return items;
        }

        private IEnumerable<IQueryableItem<TEntity>> Find<TEntity>(EntityContext context, ItemFinder finder)
            where TEntity : IEntity
        {
            var queryTree = finder.QueryTree;
            var executor = new ItemQueryExecutor(context);
            var items = executor.FindItems<TEntity>(queryTree);
            return items;
        }

        internal object ExecutePrivate<TItem>(Expression expression, bool isEnumerable, Func<ItemFinder, IEnumerable<TItem>> findExpression)
        {
            // The expression must represent a query over the data source. 
            if (!IsQueryOverDataSource(expression))
                throw new InvalidProgramException("No query over the data source was specified.");

            // Find the call to Where() and get the lambda expression predicate.
            var whereFinder = new InnermostWhereFinder();
            var whereExpression = whereFinder.GetInnermostWhere(expression);
            var lambdaExpression = (LambdaExpression)((UnaryExpression)(whereExpression.Arguments[1])).Operand;

            // Send the lambda expression through the partial evaluator.
            lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

            // Get the place name(s) to query the Web service with.
            var lf = new ItemFinder(lambdaExpression.Body);
            var items = findExpression(lf);

            // Copy the IEnumerable places to an IQueryable.
            var queryableItems = items.AsQueryable();

            return queryableItems;

            //// Copy the expression tree that was passed in, changing only the first 
            //// argument of the innermost MethodCallExpression.
            //var treeCopier = new ExpressionTreeModifier<TItem>(queryableItems);
            //var newExpressionTree = treeCopier.Visit(expression);

            //// This step creates an IQueryable that executes by replacing Queryable methods with Enumerable methods. 
            //if (isEnumerable)
            //    return queryableItems.Provider.CreateQuery(newExpressionTree);
            //else
            //    return queryableItems.Provider.Execute(newExpressionTree);
        }

        private bool IsQueryOverDataSource(Expression expression)
        {
            // If expression represents an unqueried IQueryable data source instance, 
            // expression is of type ConstantExpression, not MethodCallExpression. 
            return (expression is MethodCallExpression);
        }
    }
}
