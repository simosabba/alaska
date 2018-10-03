using Alaska.Feature.Contents.Models;
using Alaska.Foundation.Core.Messaging.Email;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Feature.Contents.Abstractions
{
    public enum ContentResolutionMode { Exact, LeafFirst, RootFirst }

    public interface IContentManager
    {
        Site GetSite(string route, CultureInfo culture);
        Page GetPage(string id, CultureInfo culture);
        Page GetPage(string id, CultureInfo culture, ContentResolutionMode resolutionMode);
        Component GetComponent(string id, CultureInfo culture);
        Item GetItem(string id, CultureInfo culture);
        Item GetItem(string id, CultureInfo culture, ContentResolutionMode resolutionMode);
        string GetLabelPageId(string labelId);
        Label GetLabel(string labelId, CultureInfo culture);
        LabelPage GetLabelPageFromLabel(string labelId, CultureInfo culture);
        LabelPage GetLabelPage(string pageId, CultureInfo culture);
        EmailTemplate GetEmailTemplate(string id, CultureInfo culture);
        IEnumerable<CultureInfo> GetInstalledLanguages();
    }
}
