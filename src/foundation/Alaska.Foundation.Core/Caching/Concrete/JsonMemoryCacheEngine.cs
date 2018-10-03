using Alaska.Foundation.Core.Caching.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alaska.Foundation.Core.Caching.Interfaces;
using Alaska.Foundation.Core.Caching.Model;
using Microsoft.Extensions.Caching.Memory;
using Alaska.Foundation.Core.Collections;

namespace Alaska.Foundation.Core.Caching.Concrete
{
    public class JsonMemoryCacheEngine : CacheEngineBase
    {
        private SafeList<string> _keys = new SafeList<string>();
        private MemoryCache _cache;

        public JsonMemoryCacheEngine(MemoryCacheOptions options)
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
            var item = _cache.Get(key);
            return item == null ? 
                null : 
                new CacheItem<T>((ICacheItem<T>)item);
        }

        public override void Set<T>(ICacheItem<T> item)
        {
            var serializedItem = new SerializedCacheItem<T>(item);
            _cache.Set(item.Key, serializedItem, DateTimeOffset.FromFileTime(item.ExpirationTime.ToFileTime()));
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
