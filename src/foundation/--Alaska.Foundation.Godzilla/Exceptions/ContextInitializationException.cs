using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class ContextInitializationException : CollectionException
    {
        public ContextInitializationException(string message)
            : base(message)
        { }

        public ContextInitializationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
