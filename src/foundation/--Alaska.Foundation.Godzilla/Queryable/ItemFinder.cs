using Alaska.Foundation.Core.Collections;
using Alaska.Foundation.Godzilla.Abstractions;
using Alaska.Foundation.Godzilla.Queryable.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable
{
    internal class ItemFinder : ExpressionVisitor
    {
        private Expression expression;

        private bool _isEvaluated = false;
        private QueryTree _queryTree = null;
        private BinaryTreeNode<QueryNode> _lastOperandNode = null;
        private UnaryModifier? _lastUnaryModifier = null;

        private ItemQueryBuilder _queryBuilder = new ItemQueryBuilder();

        public ItemFinder(Expression exp)
        {
            this.expression = exp;
        }

        public QueryTree QueryTree
        {
            get
            {
                if (!_isEvaluated)
                {
                    this.Visit(this.expression);
                    _isEvaluated = true;
                    if (_queryTree == null)
                        throw new InvalidOperationException("No conditions specified");
                }
                return _queryTree;
            }
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.IsNonGenericMethod<bool>(typeof(IQueryableItem<>), "Matches"))
            {
                if (IsInsideNotModifier())
                    throw new InvalidOperationException("Unsupported not operation for method Matches");

                var expression = m.GetUnaryExpressionOperand();
                var entityType = m.GetMethodExpressionGenericType();
                var entityFilterType = typeof(EntityFilter<>).MakeGenericType(entityType);
                var matchesFilter = (QueryFilter)Activator.CreateInstance(entityFilterType, expression);
                AppendQueryFilter(matchesFilter);
                return m;
            }

            if (m.IsGenericMethod<bool>(typeof(IQueryableItem), "Is"))
            {
                var entityType = m.GetGenericMethodArgument();
                AppendQueryFilter(new EntityTypeFilter(entityType, IsInsideNotModifier()));
                return m;
            }

            if (m.IsGenericMethod<bool>(typeof(IQueryableItem), "HasRelationship") ||
                m.IsGenericMethod<bool>(typeof(IQueryableItem<>), "HasRelationship"))
            {
                if (IsInsideNotModifier())
                    throw new InvalidOperationException("Unsupported not operation for method HasRelationship");

                var relationType = m.GetGenericMethodArgument();
                AppendQueryFilter(new RelationshipFilter(relationType));
                return m;
            }

            if (m.IsGenericMethod<bool>(typeof(IQueryableItem), "HasInboundRelationship") ||
                m.IsGenericMethod<bool>(typeof(IQueryableItem<>), "HasInboundRelationship"))
            {
                if (IsInsideNotModifier())
                    throw new InvalidOperationException("Unsupported not operation for method HasInboundRelationship");

                var relationType = m.GetGenericMethodArgument();
                AppendQueryFilter(new RelationshipToFilter(relationType));
                return m;
            }

            if (m.IsGenericMethod<bool>(typeof(IQueryableItem), "HasOutboundRelationship") ||
                m.IsGenericMethod<bool>(typeof(IQueryableItem<>), "HasOutboundRelationship"))
            {
                if (IsInsideNotModifier())
                    throw new InvalidOperationException("Unsupported not operation for method HasOutboundRelationship");

                var relationType = m.GetGenericMethodArgument();
                AppendQueryFilter(new RelationshipFromFilter(relationType));
                return m;
            }

            throw new InvalidOperationException($"Unknown method {m.ToString()}");
        }

        protected override Expression VisitBinary(BinaryExpression be)
        {
            if (IsQueryOperand(be))
            {
                AppendQueryOperand(new QueryOperand(be.NodeType));
                return base.VisitBinary(be);
            }

            throw new NotImplementedException($"{be.NodeType} node type not implemented");
            //return base.VisitBinary(be);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.IsNotExpression())
            {
                _lastUnaryModifier = UnaryModifier.Not;
                return base.VisitUnary(node);
            }

            throw new InvalidOperationException($"Unknown unary expresson {node.ToString()}");
        }

        private bool IsInsideNotModifier()
        {
            return IsInside(UnaryModifier.Not);
        }

        private bool IsInside(UnaryModifier modifier)
        {
            return _lastUnaryModifier.HasValue && _lastUnaryModifier.Value == modifier;
        }

        private bool IsQueryOperand(BinaryExpression be)
        {
            switch (be.NodeType)
            {
                case ExpressionType.Equal:
                    return false;
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                    return true;
                default:
                    throw new NotImplementedException($"Operand {be.NodeType} not implemented");
            }
        }

        private void AppendQueryOperand(QueryOperand operand)
        {
            _lastUnaryModifier = null;

            if (_queryTree == null)
            {
                _queryTree = new QueryTree(operand);
                _lastOperandNode = _queryTree.Root;
                return;
            }

            if (_lastOperandNode.Left == null)
            {
                _lastOperandNode = _lastOperandNode.SetLeft(operand);
                return;
            }

            if (_lastOperandNode.Right == null)
            {
                _lastOperandNode = _lastOperandNode.SetRight(operand);
                return;
            }
        }

        private void AppendQueryFilter(QueryFilter filter)
        {
            _lastUnaryModifier = null;

            if (_queryTree == null)
            {
                _queryTree = new QueryTree(filter);
                return;
            }

            if (_lastOperandNode.Left == null)
            {
                _lastOperandNode.SetLeft(filter);
                return;
            }

            if (_lastOperandNode.Right == null)
            {
                _lastOperandNode.SetRight(filter);
                _lastOperandNode = _lastOperandNode.Parent;
                return;
            }

            throw new InvalidOperationException($"Corrupted query tree");
        }
    }
}
