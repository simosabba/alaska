using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class HierarchyItemNotFoundException : Exception
    {
        internal HierarchyItemNotFoundException(string message) : base(message) { }
        internal HierarchyItemNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
