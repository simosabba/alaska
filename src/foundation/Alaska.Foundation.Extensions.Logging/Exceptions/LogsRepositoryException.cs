using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alaska.Foundation.Extensions.Logging.Exceptions
{
    public class LogsRepositoryException : Exception
    {
        public LogsRepositoryException()
        {
        }

        public LogsRepositoryException(string message) : base(message)
        {
        }

        public LogsRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LogsRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
