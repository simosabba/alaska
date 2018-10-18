using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Mongo.Exceptions
{
    public class MongoDerivedTypeResolutionException : MongoExceptionBase
    {
        public MongoDerivedTypeResolutionException(string message) : base(message) { }
    }
}
