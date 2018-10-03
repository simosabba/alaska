using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Collections
{
    public class LookupMap<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _directDict = new Dictionary<TKey, TValue>();
        private Dictionary<TValue, TKey> _reverseDict = new Dictionary<TValue, TKey>();

        public void Add(TKey key, TValue value)
        {
            if (_directDict.ContainsKey(key))
                throw new InvalidOperationException($"Direct key {key} already exists");
            if (_reverseDict.ContainsKey(value))
                throw new InvalidOperationException($"Reverse key {value} already exists");

            _directDict.Add(key, value);
            _reverseDict.Add(value, key);
        }

        public void Set(TKey key, TValue value)
        {
            _directDict[key] = value;
            _reverseDict[value] = key;
        }

        public void Remove(TKey key)
        {
            if (!ContainsKey(key))
                return;
            _reverseDict.Remove(GetValue(key));
            _directDict.Remove(key);
        }

        public void RemoveValue(TValue value)
        {
            if (!ContainsValue(value))
                return;
            _directDict.Remove(GetKey(value));
            _reverseDict.Remove(value);
        }

        public void Clear()
        {
            _directDict.Clear();
            _reverseDict.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return _directDict.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _reverseDict.ContainsKey(value);
        }

        public TValue this[TKey key]
        {
            get { return GetValue(key); }
            set { Set(key, value); }
        }

        public IEnumerable<TKey> Keys => _directDict.Keys;
        public IEnumerable<TValue> Values => _directDict.Values;

        public TValue GetValue(TKey key)
        {
            return _directDict[key];
        }

        public TKey GetKey(TValue value)
        {
            return _reverseDict[value];
        }
    }
}
