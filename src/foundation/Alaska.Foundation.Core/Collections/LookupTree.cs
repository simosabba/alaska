using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Collections
{
    public abstract class LookupTree<TValue, TKey>: Tree<TValue>
    {
        private LookupMap<TKey, TreeNode<TValue>> _map = new LookupMap<TKey, TreeNode<TValue>>();

        public LookupTree(TValue root)
            : base(root)
        {
            OnAdd += OnItemAdd;
            OnSet += OnItemSet;
            OnRemove += OnItemRemove;
        }

        protected abstract TKey GetKey(TValue value);

        private void OnItemAdd(TreeNode<TValue> node)
        {
            _map.Add(GetKey(node.Value), node);
        }
        
        private void OnItemRemove(TreeNode<TValue> node)
        {
            _map.Remove(GetKey(node.Value));
        }

        private void OnItemSet(TreeNode<TValue> node, TValue oldValue, TValue newValue)
        {
            _map.ContainsKey(GetKey(oldValue));
            _map.Add(GetKey(node.Value), node);
        }
    }
}
