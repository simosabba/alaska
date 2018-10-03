using Alaska.Feature.Contents.Models;
using Alaska.Foundation.Core.Messaging.Email;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alaska.Feature.Contents.Abstractions
{
    public interface IContentService
    {
        Site GetSite(string route, CultureInfo culture);
        Page GetPage(string id, CultureInfo culture);
        Component GetComponent(string id, CultureInfo culture);
        Item GetItem(string id, CultureInfo culture);
        Label GetLabel(string labelId, CultureInfo culture);
        LabelPage GetLabelPage(string pageId, CultureInfo culture);
        EmailTemplate GetEmailTemplate(string templateId);
        EmailTemplate GetEmailTemplate(string templateId, CultureInfo culture);
    }
}
