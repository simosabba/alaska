using Alaska.Foundation.Core.Collections;
using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Items;
using Alaska.Foundation.Godzilla.Queryable.Filters;
using Alaska.Foundation.Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable
{
    internal class ItemQueryExecutor
    {
        #region Init

        private EntityContext _context;

        public ItemQueryExecutor(EntityContext context)
        {
            _context = context;
        }

        #endregion

        #region Find

        public IEnumerable<IQueryableItem<TEntity>> FindItems<TEntity>(QueryTree queryTree)
            where TEntity : IEntity
        {
            var itemsId = ExecuteQuery(queryTree.Root);
            return _context.GetItems<TEntity>(itemsId)
                .Cast<Item<TEntity>>();
        }

        public IEnumerable<IQueryableItem> FindItems(QueryTree queryTree)
        {
            var itemsId = ExecuteQuery(queryTree.Root);
            return _context.GetItems(itemsId)
                .Cast<Item>();
        }

        #endregion

        #region Run Query

        private IEnumerable<Guid> ExecuteQuery(BinaryTreeNode<QueryNode> queryNode)
        {
            if (queryNode.Value is QueryFilter)
            {
                var filter = queryNode.Value as QueryFilter;
                return filter.Execute(_context);
            }

            if (queryNode.Value is QueryOperand)
            {
                var leftResults = ExecuteQuery(queryNode.Left);
                var rightResults = ExecuteQuery(queryNode.Right);
                return AggregateResults(leftResults, rightResults, ((QueryOperand)queryNode.Value).Operand);
            }

            throw new NotImplementedException($"Invalid query node type {queryNode.Value.GetType().FullName}");
        }

        private IEnumerable<Guid> AggregateResults(IEnumerable<Guid> left, IEnumerable<Guid> right, ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.AndAlso:
                    return left.Intersect(right).ToList();
                case ExpressionType.OrElse:
                    return left.Union(right).Distinct().ToList();
                default:
                    throw new InvalidOperationException($"Invalid expression type {expressionType}");
            }
        }

        #endregion
    }
}
