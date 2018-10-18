using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        internal ItemNotFoundException(string message) : base(message) { }
        internal ItemNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
