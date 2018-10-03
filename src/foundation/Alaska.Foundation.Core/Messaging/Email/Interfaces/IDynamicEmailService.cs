using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Messaging.Email.Interfaces
{
    public interface IDynamicEmailService
    {
        IEmail CreateEmailInstance(Guid templateId);
        IEmail CreateEmailInstance(Guid templateId, EmailPlaceholders placeholders);
        IEmail CreateEmailInstance(Guid templateId, CultureInfo culture);
        IEmail CreateEmailInstance(Guid templateId, CultureInfo culture, EmailPlaceholders placeholders);

        IEmail CreateEmailInstance(string template);
        IEmail CreateEmailInstance(string template, EmailPlaceholders placeholders);
        IEmail CreateEmailInstance(string template, CultureInfo culture);
        IEmail CreateEmailInstance(string template, CultureInfo culture, EmailPlaceholders placeholders);
    }
}
