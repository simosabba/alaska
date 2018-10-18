using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class InvalidItemStateException : Exception
    {
        internal InvalidItemStateException(string message) : base(message) { }
        internal InvalidItemStateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
