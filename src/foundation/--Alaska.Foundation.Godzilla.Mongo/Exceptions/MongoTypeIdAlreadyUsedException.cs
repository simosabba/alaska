using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Mongo.Exceptions
{
    public class MongoTypeIdAlreadyUsedException : MongoExceptionBase
    {
        internal MongoTypeIdAlreadyUsedException() : base() { }
        internal MongoTypeIdAlreadyUsedException(string message) : base(message) { }
        internal MongoTypeIdAlreadyUsedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
