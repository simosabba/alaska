using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Collections
{
    public class Tree<T>
    {
        public delegate void OnAddEventHandler(TreeNode<T> node);
        public delegate void OnSetEventHandler(TreeNode<T> node, T oldValue, T newValue);
        public delegate void OnRemoveEventHandler(TreeNode<T> node);
        
        private TreeNode<T> _root;
        
        public Tree(T root)
        {
            _root = new TreeNode<T>(this, root, null);
        }

        public TreeNode<T> Root => _root;

        public event OnAddEventHandler OnAdd;
        public event OnSetEventHandler OnSet;
        public event OnRemoveEventHandler OnRemove;

        internal void InvokeOnAdd(TreeNode<T> node)
        {
            if (OnAdd != null)
                OnAdd(node);
        }

        internal void InvokeOnSet(TreeNode<T> node, T oldValue, T newValue)
        {
            if (OnSet != null)
                OnSet(node, oldValue, newValue);
        }

        internal void InvokeOnRemove(TreeNode<T> node)
        {
            if (OnRemove != null)
                OnRemove(node);
        }
    }

    public class TreeNode<T>
    {
        private Tree<T> _tree;
        private T _value;
        private TreeNode<T> _parent;
        private TreeNodeCollection<T> _children;

        internal TreeNode(Tree<T> tree, T value, TreeNode<T> parent)
        {
            _tree = tree;
            _value = value;
            _parent = parent;
            _children = new TreeNodeCollection<T>(tree, this);
        }

        public T Value
        {
            get { return _value; }
            set
            {
                var oldValue = _value;
                _value = value;
                _tree.InvokeOnSet(this, oldValue, value);
            }
        }

        public TreeNode<T> Parent { get { return _parent; } }
        public TreeNodeCollection<T> Children { get { return _children; } }
    }

    public class TreeNodeCollection<T> : IEnumerable<TreeNode<T>>
    {
        private Tree<T> _tree;
        private TreeNode<T> _node;
        private List<TreeNode<T>> _children = new List<TreeNode<T>>();

        internal TreeNodeCollection(Tree<T> tree, TreeNode<T> node)
        {
            _tree = tree;
            _node = node;
        }

        public void AddRange(IEnumerable<T> values)
        {
            values.ToList().ForEach(Add);
        }

        public void Add(T value)
        {
            var node = new TreeNode<T>(_tree, value, _node);
            _children.Add(node);
            _tree.InvokeOnAdd(node);
        }

        public void Remove(T node)
        {
            var nodesToRemove = _children.Where(x => x.Value.Equals(node));
            foreach (var nodeToRemove in nodesToRemove)
            {
                _children.Remove(nodeToRemove);
                _tree.InvokeOnRemove(nodeToRemove);
            }
        }

        public void Clear()
        {
            var nodes = _children.ToList();
            _children.Clear();
            nodes.ForEach(x => _tree.InvokeOnRemove(x));
        }

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _children.GetEnumerator();
        }
    }
}
