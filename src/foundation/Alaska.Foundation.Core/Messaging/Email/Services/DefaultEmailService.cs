using Alaska.Foundation.Core.Messaging.Email.Abstractions;
using Alaska.Foundation.Core.Messaging.Email.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Alaska.Foundation.Core.Messaging.Email.Services
{
    internal class DefaultEmailService : IEmailService
    {
        public void SendEmail(IEmail email)
        {
            var mail = new MailMessage();
            if (!string.IsNullOrWhiteSpace(email.From))
                mail.From = new MailAddress(email.From);
            email.To.ToList().ForEach(x => mail.To.Add(x));
            email.Cc.ToList().ForEach(x => mail.To.Add(x));
            email.Bcc.ToList().ForEach(x => mail.To.Add(x));
            mail.Subject = email.Subject;
            mail.Body = email.Body;
            mail.IsBodyHtml = true;

            var client = new SmtpClient();
            client.Send(mail);
        }
    }
}
