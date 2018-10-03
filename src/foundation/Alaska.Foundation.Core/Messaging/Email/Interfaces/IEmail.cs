using System;
using System.Collections.Generic;
using System.Text;

namespace Alaska.Foundation.Core.Messaging.Email.Interfaces
{
    public interface IEmail
    {
        string From { get; }
        IEnumerable<string> To { get; }
        IEnumerable<string> Cc { get; }
        IEnumerable<string> Bcc { get; }
        string Subject { get; }
        string Body { get; }
    }
}
