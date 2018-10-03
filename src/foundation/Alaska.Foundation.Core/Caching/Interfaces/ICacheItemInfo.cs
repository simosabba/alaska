using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Caching.Interfaces
{
    public interface ICacheItemInfo
    {
        string Key { get; }
        TimeSpan Expiration { get; }
        DateTime ExpirationTime { get; }
    }
}
