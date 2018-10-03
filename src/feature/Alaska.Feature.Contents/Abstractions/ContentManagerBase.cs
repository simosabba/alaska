using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Alaska.Foundation.Core.Messaging.Email;
using Alaska.Feature.Contents.Models;

namespace Alaska.Feature.Contents.Abstractions
{
    public abstract class ContentManagerBase : IContentManager
    {
        private ContentResolutionMode DefaultContentResolutionMode => ContentResolutionMode.RootFirst;

        public abstract string GetLabelPageId(string labelId);

        public abstract Label GetLabel(string labelId, CultureInfo culture);

        public virtual LabelPage GetLabelPageFromLabel(string labelId, CultureInfo culture)
        {
            var pageId = GetLabelPageId(labelId);
            return string.IsNullOrEmpty(pageId) ? null : GetLabelPage(pageId, culture);
        }

        public abstract LabelPage GetLabelPage(string pageId, CultureInfo culture);

        public virtual Page GetPage(string id, CultureInfo culture)
        {
            return GetPage(id, culture, DefaultContentResolutionMode);
        }

        public abstract Page GetPage(string id, CultureInfo culture, ContentResolutionMode resolutionMode);

        public abstract Component GetComponent(string id, CultureInfo culture);

        public abstract Site GetSite(string route, CultureInfo culture);

        public Item GetItem(string id, CultureInfo culture)
        {
            return GetItem(id, culture, DefaultContentResolutionMode);
        }

        public abstract Item GetItem(string id, CultureInfo culture, ContentResolutionMode resolutionMode);

        public abstract EmailTemplate GetEmailTemplate(string id, CultureInfo culture);
        public abstract IEnumerable<CultureInfo> GetInstalledLanguages();
    }
}
