using Alaska.Foundation.Core.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable.Filters
{
    internal class QueryTree : BinaryTree<QueryNode>
    {
        public QueryTree(QueryNode root) 
            : base(root)
        {
        }

        public override string ToString()
        {
            return PrintNode(Root);
        }

        public string PrintNode(BinaryTreeNode<QueryNode> node)
        {
            var tab = new string(' ', node.Depth);
            var left = node.Left == null ? string.Empty : PrintNode(node.Left);
            var right = node.Right == null ? string.Empty : PrintNode(node.Right);
            return $"{tab}(\n{tab}{left}\n{tab}{node.Value.Representation}\n{tab}{right}\n{tab})\n";
        }
    }
}
