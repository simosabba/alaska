using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        internal DuplicateEntityException(string message) : base(message) { }
        internal DuplicateEntityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
