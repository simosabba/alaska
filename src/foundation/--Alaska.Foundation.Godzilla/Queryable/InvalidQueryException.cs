using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Godzilla.Queryable
{
    public class InvalidQueryException : System.Exception
    {
        private string message;

        internal InvalidQueryException(string message)
        {
            this.message = message + " ";
        }

        public override string Message
        {
            get
            {
                return "The client query is invalid: " + message;
            }
        }
    }
}
