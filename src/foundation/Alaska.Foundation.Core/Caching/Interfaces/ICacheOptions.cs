using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Caching.Interfaces
{
    public interface ICacheOptions
    {
        TimeSpan DefaultExpiration { get; }
        bool IsDisabled { get; }
        bool CacheNullItems { get; }
        string ConnectionString { get; }
    }
}
