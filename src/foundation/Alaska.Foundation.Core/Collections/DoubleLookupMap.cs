using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Collections
{
    public class DoubleLookupMap<TKey1, TKey2, TValue>
    {
        private LookupMap<TKey1, TValue> _key1Map = new LookupMap<TKey1, TValue>();
        private LookupMap<TKey2, TValue> _key2Map = new LookupMap<TKey2, TValue>();
        private LookupMap<TKey1, TKey2> _keysMap = new LookupMap<TKey1, TKey2>();

        public void Add(TKey1 key1, TKey2 key2, TValue value)
        {
            _key1Map.Add(key1, value);
            _key2Map.Add(key2, value);
            _keysMap.Add(key1, key2);
        }

        public void RemoveFromKey1(TKey1 key)
        {
            var key2 = _keysMap.GetValue(key);
            _key1Map.Remove(key);
            _key2Map.Remove(key2);
            _keysMap.Remove(key);
        }

        public void RemoveFromKey2(TKey2 key2)
        {
            var key1 = _keysMap.GetKey(key2);
            _key1Map.Remove(key1);
            _key2Map.Remove(key2);
            _keysMap.Remove(key1);
        }

        public void SetFromKey1(TKey1 key, TValue value)
        {
            var key2 = _keysMap.GetValue(key);
            _key1Map.Set(key, value);
            _key2Map.Set(key2, value);
        }

        public void SetFromKey2(TKey2 key, TValue value)
        {
            var key1= _keysMap.GetKey(key);
            _key1Map.Set(key1, value);
            _key2Map.Set(key, value);
        }

        public void Clear()
        {
            _key1Map.Clear();
            _key2Map.Clear();
            _keysMap.Clear();
        }

        public bool ContainsKey1(TKey1 key)
        {
            return _key1Map.ContainsKey(key);
        }

        public bool ContainsKey2(TKey2 key)
        {
            return _key2Map.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _key1Map.ContainsValue(value);
        }

        public IEnumerable<TKey1> Keys1 => _key1Map.Keys;
        public IEnumerable<TKey2> Keys2 => _key2Map.Keys;
        public IEnumerable<TValue> Values => _key1Map.Values;

        public TValue GetValueFromKey1(TKey1 key)
        {
            return _key1Map.GetValue(key);
        }

        public TValue GetValueFromKey2(TKey2 key)
        {
            return _key2Map.GetValue(key);
        }

        public TKey1 GetKey1FromValue(TValue value)
        {
            return _key1Map.GetKey(value);
        }

        public TKey2 GetKey2FromValue(TValue value)
        {
            return _key2Map.GetKey(value);
        }

        public TKey1 GetKey1FromKey2(TKey2 key)
        {
            return _keysMap.GetKey(key);
        }

        public TKey2 GetKey2FromKey1(TKey1 key)
        {
            return _keysMap.GetValue(key);
        }
    }
}
