using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToDetailedString(this Exception e)
        {
            return string.Format("Type: {0}\nMessage: {1}\nStackTrace: {2}{3}",
                e.GetType().FullName,
                e.Message,
                e.StackTrace,
                e.InnerException == null ? string.Empty : string.Format("\n\nInnerException:\n{0}", e.InnerException.ToDetailedString())
                );
        }
    }
}
