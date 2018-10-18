using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Mongo.Exceptions
{
    public class MongoConfigurationException : MongoExceptionBase
    {
        internal MongoConfigurationException() : base() { }
        internal MongoConfigurationException(string message) : base(message) { }
        internal MongoConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
