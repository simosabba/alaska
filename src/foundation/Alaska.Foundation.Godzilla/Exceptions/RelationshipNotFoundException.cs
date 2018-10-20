using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class RelationshipNotFoundException : Exception
    {
        internal RelationshipNotFoundException(string message) : base(message) { }
        internal RelationshipNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
