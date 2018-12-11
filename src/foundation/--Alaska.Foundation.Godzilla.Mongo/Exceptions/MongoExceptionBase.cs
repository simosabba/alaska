using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Mongo.Exceptions
{
    public abstract class MongoExceptionBase : Exception
    {
        internal MongoExceptionBase() : base() { }
        internal MongoExceptionBase(string message) : base(message) { }
        internal MongoExceptionBase(string message, Exception innerException) : base(message, innerException) { }
    }
}
