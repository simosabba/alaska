using Alaska.Foundation.Core.Caching.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alaska.Foundation.Core.Caching.Abstractions
{
    public abstract class CacheEngineBase : ICacheEngine
    {
        public abstract ICacheItem Get(string key);

        public abstract ICacheItem<T> Get<T>(string key);

        public abstract void Set<T>(ICacheItem<T> item);

        public abstract void Remove(string key);

        public abstract void Clear();

        public abstract IEnumerable<string> Keys { get; }
    }
}
