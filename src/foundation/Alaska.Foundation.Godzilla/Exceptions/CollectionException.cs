using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class CollectionException : Exception
    {
        public CollectionException()
            : base()
        { }

        public CollectionException(string message)
            : base()
        { }

        public CollectionException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
