using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Core.Collections
{
    public abstract class BinaryTree<TValue>
    {
        private BinaryTreeNode<TValue> _root;

        public BinaryTree(TValue root)
        {
            _root = new BinaryTreeNode<TValue>(root, null, 0);
        }

        public BinaryTreeNode<TValue> Root => _root;
    }

    public class BinaryTreeNode<TValue>
    {
        private int _depth;
        private TValue _value;
        private BinaryTreeNode<TValue> _parent;
        private BinaryTreeNode<TValue> _left;
        private BinaryTreeNode<TValue> _right;

        public BinaryTreeNode(TValue value, BinaryTreeNode<TValue> parent, int depth)
        {
            _depth = depth;
            _value = value;
            _parent = parent;
        }

        public void SetValue(TValue value)
        {
            _value = value;
        }

        public BinaryTreeNode<TValue> SetLeft(TValue value)
        {
            return _left = value == null ? null : new BinaryTreeNode<TValue>(value, this, _depth + 1);
        }

        public BinaryTreeNode<TValue> SetRight(TValue value)
        {
            return _right = value == null ? null : new BinaryTreeNode<TValue>(value, this, _depth + 1);
        }

        public int Depth => _depth;
        public TValue Value => _value;
        public BinaryTreeNode<TValue> Left => _left;
        public BinaryTreeNode<TValue> Right => _right;
        public BinaryTreeNode<TValue> Parent => _parent;

        public bool IsLeaf => Left == null && Right == null;
    }
}
