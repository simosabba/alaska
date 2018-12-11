using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class PathAlreadyExistsException : Exception
    {
        internal PathAlreadyExistsException(string message) : base(message) { }
        internal PathAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
