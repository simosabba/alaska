using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class InvalidEntityTypeException : Exception
    {
        internal InvalidEntityTypeException(string message) : base(message) { }
        internal InvalidEntityTypeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
