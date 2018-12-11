using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class CollectionItemUpdateException : Exception
    {
        internal CollectionItemUpdateException(string message) : base(message) { }
        internal CollectionItemUpdateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
