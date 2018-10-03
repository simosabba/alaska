using Alaska.Feature.Contents.Abstractions;
using Alaska.Foundation.Core.Messaging.Email;
using Alaska.Foundation.Core.Messaging.Email.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alaska.Feature.Contents
{
    public class DynamicEmailService : DynamicEmailServiceBase
    {
        #region Init

        private readonly IContentService _contentsService;

        public DynamicEmailService(IContentService contentsService)
        {
            _contentsService = contentsService ?? throw new ArgumentException(nameof(IContentService));
        }

        #endregion

        protected override EmailTemplate GetEmailTemplate(string template, CultureInfo culture)
        {
            return _contentsService.GetEmailTemplate(template, culture);
        }

        protected override EmailTemplate GetEmailTemplate(string template)
        {
            return _contentsService.GetEmailTemplate(template);
        }
    }
}
