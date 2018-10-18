using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Mongo.Exceptions
{
    public class MongoFilterCompositionException : MongoExceptionBase
    {
        public MongoFilterCompositionException(string message) : base(message) { }
        public MongoFilterCompositionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
