using Alaska.Foundation.Core.Caching.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Core.Caching
{
    public class CacheOptions : ICacheOptions
    {
        public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(30);

        public bool IsDisabled => false;

        public bool CacheNullItems => false;

        public string ConnectionString => throw new NotImplementedException();
    }
}
