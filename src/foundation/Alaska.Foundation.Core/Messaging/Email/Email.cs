using Alaska.Foundation.Core.Messaging.Email.Interfaces;
using System;
using System.Collections.Generic;

namespace Alaska.Foundation.Core.Messaging.Email
{
    public class Email : IEmail
    {
        public Email()
        {
            To = new List<string>();
            Cc = new List<string>();
            Bcc = new List<string>();
        }

        public string From { get; set; }
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        IEnumerable<string> IEmail.To => To;
        IEnumerable<string> IEmail.Cc => Cc;
        IEnumerable<string> IEmail.Bcc => Bcc;
    }
}
