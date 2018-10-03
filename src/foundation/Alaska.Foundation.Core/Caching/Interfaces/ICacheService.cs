using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Caching.Interfaces
{
    public interface ICacheService
    {
        ICacheInstance GetCache(string cacheId);
        TCacheInstance GetCache<TCacheInstance>() where TCacheInstance : ICacheInstance;
        IEnumerable<ICacheInstance> GetAllCaches();
    }
}
