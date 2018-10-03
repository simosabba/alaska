using Alaska.Feature.Contents.Abstractions;
using Alaska.Feature.Contents.Models;
using Alaska.Foundation.Core.Messaging.Email;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Feature.Contents
{
    public class ContentService : IContentService
    {
        #region Init

        private readonly IContentManager _contentManager;
        private readonly IContentCache _contentsCache;

        public ContentService(IContentManager contentManager, IContentCache contentsCache)
        {
            _contentManager = contentManager ?? throw new ArgumentException(nameof(IContentManager));
            _contentsCache = contentsCache ?? throw new ArgumentException(nameof(IContentCache));
        }

        #endregion

        #region Contents

        public Site GetSite(string route, CultureInfo culture)
        {
            var resolvedCulture = ResolveCulture(culture);
            return _contentManager.GetSite(route, resolvedCulture);
        }

        public Page GetPage(string id, CultureInfo culture)
        {
            var resolvedCulture = ResolveCulture(culture);
            return _contentManager.GetPage(id, resolvedCulture);
        }

        public Component GetComponent(string id, CultureInfo culture)
        {
            var resolvedCulture = ResolveCulture(culture);
            return _contentManager.GetComponent(id, resolvedCulture);
        }

        public Item GetItem(string id, CultureInfo culture)
        {
            var resolvedCulture = ResolveCulture(culture);
            return _contentManager.GetItem(id, resolvedCulture);
        }

        public Label GetLabel(string labelId, CultureInfo culture)
        {
            var resolvedCulture = ResolveCulture(culture);
            return _contentsCache.RetreiveLabel(resolvedCulture, labelId, () => _contentManager.GetLabel(labelId, resolvedCulture));
        }
        
        public LabelPage GetLabelPage(string pageId, CultureInfo culture)
        {
            var resolvedCulture = ResolveCulture(culture);
            return _contentsCache.RetreiveLabelPage(resolvedCulture, pageId, () => _contentManager.GetLabelPage(pageId, resolvedCulture));
        }

        public EmailTemplate GetEmailTemplate(string templateId)
        {
            return GetEmailTemplate(templateId, GetCurrentCulture());
        }

        public EmailTemplate GetEmailTemplate(string templateId, CultureInfo culture)
        {
            var resolvedCulture = ResolveCulture(culture);
            return _contentManager.GetEmailTemplate(templateId, resolvedCulture);
        }

        private CultureInfo GetCurrentCulture()
        {
            return System.Globalization.CultureInfo.CurrentCulture;
        }

        #endregion

        #region Languages

        private CultureInfo ResolveCulture(CultureInfo culture)
        {
            var installedCultures = _contentsCache.RetreiveInstalledCultures(
                () => _contentManager.GetInstalledLanguages()
                );

            if (installedCultures.Contains(culture))
                return culture;

            if (!culture.Name.Equals("en-US", StringComparison.InvariantCultureIgnoreCase))
                return ResolveCulture(new CultureInfo("en-US"));

            return installedCultures.First();
        }

        #endregion
    }
}
