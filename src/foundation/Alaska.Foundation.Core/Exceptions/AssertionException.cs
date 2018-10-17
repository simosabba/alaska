using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Core.Exeptions
{
    public class AssertionException : Exception
    {
        public AssertionException(string message)
            : base(message)
        { }

        public AssertionException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
