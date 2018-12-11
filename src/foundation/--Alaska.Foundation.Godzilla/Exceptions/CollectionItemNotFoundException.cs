using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class CollectionItemNotFoundException : Exception
    {
        internal CollectionItemNotFoundException(string message) : base(message) { }
        internal CollectionItemNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
