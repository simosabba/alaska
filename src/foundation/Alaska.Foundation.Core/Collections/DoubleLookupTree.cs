using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Collections
{
    public abstract class DoubleLookupTree<TValue, TKey1, TKey2> : Tree<TValue>
    {
        private DoubleLookupMap<TKey1, TKey2, TreeNode<TValue>> _map = new DoubleLookupMap<TKey1, TKey2, TreeNode<TValue>>();

        public DoubleLookupTree(TValue root)
            : base(root)
        {
            OnAdd += OnItemAdd;
            OnSet += OnItemSet;
            OnRemove += OnItemRemove;
        }

        protected abstract TKey1 GetKey1(TValue value);
        protected abstract TKey2 GetKey2(TValue value);

        private void OnItemAdd(TreeNode<TValue> node)
        {
            _map.Add(GetKey1(node.Value), GetKey2(node.Value), node);
        }

        private void OnItemRemove(TreeNode<TValue> node)
        {
            _map.RemoveFromKey1(GetKey1(node.Value));
        }

        private void OnItemSet(TreeNode<TValue> node, TValue oldValue, TValue newValue)
        {
            _map.RemoveFromKey1(GetKey1(oldValue));
            _map.Add(GetKey1(node.Value), GetKey2(node.Value), node);
        }

        public TreeNode<TValue> GetNodeFromKey1(TKey1 key1)
        {
            return _map.GetValueFromKey1(key1);
        }

        public TreeNode<TValue> GetNodeFromKey2(TKey2 key2)
        {
            return _map.GetValueFromKey2(key2);
        }
    }
}
