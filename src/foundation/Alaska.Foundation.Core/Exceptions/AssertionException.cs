using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Core.Exeptions
{
    public class AssertionException : Exception
    {
        internal AssertionException(string message)
            : base(message)
        { }

        internal AssertionException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
