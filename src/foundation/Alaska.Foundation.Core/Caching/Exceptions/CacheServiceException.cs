using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Caching.Exceptions
{
    public class CacheServiceException : CacheException
    {
        public CacheServiceException()
        {
        }

        public CacheServiceException(string message) : base(message)
        {
        }

        public CacheServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
