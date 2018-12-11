using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Exceptions
{
    public class HierarchyItemUpdateException : Exception
    {
        internal HierarchyItemUpdateException(string message) : base(message) { }
        internal HierarchyItemUpdateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
