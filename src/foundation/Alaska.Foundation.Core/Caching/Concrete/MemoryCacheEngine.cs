using Alaska.Foundation.Core.Caching.Abstractions;
using Alaska.Foundation.Core.Caching.Interfaces;
using Alaska.Foundation.Core.Collections;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Caching.Concrete
{
    internal class MemoryCacheEngine : CacheEngineBase
    {
        private SafeList<string> _keys = new SafeList<string>();
        private MemoryCache _cache;

        public MemoryCacheEngine(MemoryCacheOptions options)
        {
            _cache = new MemoryCache(options);
        }

        public override ICacheItem Get(string key)
        {
            var item = _cache.Get(key);
            return item == null ? null : (ICacheItem)item;
        }

        public override ICacheItem<T> Get<T>(string key)
        {
            object item;
            _cache.TryGetValue(key, out item);
            return item == null ? null : (ICacheItem<T>)item;
        }

        public override void Set<T>(ICacheItem<T> item)
        {
            _cache.Set(item.Key, item, DateTimeOffset.FromFileTime(item.ExpirationTime.ToFileTime()));
            _keys.Add(item.Key);
        }

        public override void Remove(string key)
        {
            _cache.Remove(key);
            _keys.Remove(key);
        }

        public override void Clear()
        {
            Keys.ToList().ForEach(x => Remove(x));
        }

        public override IEnumerable<string> Keys =>
            _keys.Values.ToList();
    }
}
