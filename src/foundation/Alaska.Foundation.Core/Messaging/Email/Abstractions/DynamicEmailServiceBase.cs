using Alaska.Foundation.Core.Messaging.Email.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Alaska.Foundation.Core.Messaging.Email.Abstractions
{
    public abstract class DynamicEmailServiceBase : IDynamicEmailService
    {
        public IEmail CreateEmailInstance(Guid templateId)
        {
            return CreateEmailInstance(templateId.ToString());
        }

        public IEmail CreateEmailInstance(Guid templateId, EmailPlaceholders placeholders)
        {
            return CreateEmailInstance(templateId.ToString(), placeholders);
        }

        public IEmail CreateEmailInstance(Guid templateId, CultureInfo culture)
        {
            return CreateEmailInstance(templateId.ToString(), culture);
        }

        public IEmail CreateEmailInstance(Guid templateId, CultureInfo culture, EmailPlaceholders placeholders)
        {
            return CreateEmailInstance(templateId.ToString(), culture, placeholders);
        }

        public IEmail CreateEmailInstance(string template)
        {
            return CreateEmailInstance(template, (EmailPlaceholders)null);
        }

        public IEmail CreateEmailInstance(string template, EmailPlaceholders placeholders)
        {
            var emailTemplate = GetEmailTemplate(template);
            return BuildEmail(emailTemplate, placeholders);
        }

        public IEmail CreateEmailInstance(string template, CultureInfo culture)
        {
            return CreateEmailInstance(template, culture, null);
        }

        public virtual IEmail CreateEmailInstance(string template, CultureInfo culture, EmailPlaceholders placeholders)
        {
            var emailTemplate = GetEmailTemplate(template, culture);
            return BuildEmail(emailTemplate, placeholders);
        }

        protected abstract EmailTemplate GetEmailTemplate(string template, CultureInfo culture);
        protected abstract EmailTemplate GetEmailTemplate(string template);

        protected IEmail BuildEmail(EmailTemplate template, EmailPlaceholders placeholders)
        {
            return new Email
            {
                From = template.From,
                To = template.To.ToList(),
                Cc = template.Cc.ToList(),
                Bcc = template.Bcc.ToList(),
                Subject = ReplacePlaceholders(template.Subject, placeholders),
                Body = ReplacePlaceholders(template.Body, placeholders)
            };
        }

        protected string ReplacePlaceholders(string field, EmailPlaceholders placeholders)
        {
            if (placeholders == null)
                return field;

            foreach (var placeholder in placeholders)
                field = ReplacePlaceholder(field, placeholder.Key, placeholder.Value);
            return field;
        }

        protected virtual string ReplacePlaceholder(string field, string placeholder, object value)
        {
            return Regex.Replace(field, placeholder, value?.ToString() ?? string.Empty, RegexOptions.IgnoreCase);
        }
    }
}
