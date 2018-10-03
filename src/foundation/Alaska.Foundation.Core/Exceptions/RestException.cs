using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Exceptions
{
    public class RestException : Exception
    {
        public RestException()
        {
        }

        public RestException(string message) : base(message)
        {
        }

        public RestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
