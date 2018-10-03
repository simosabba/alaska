using System;
using System.Collections.Generic;

namespace Alaska.Foundation.Core.Messaging.Email.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(IEmail email);
    }
}
