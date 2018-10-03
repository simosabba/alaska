using Alaska.Feature.Contents.Abstractions;
using Alaska.Feature.Contents.Models;
using Alaska.Foundation.Core.Caching;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Feature.Contents.Cache
{
    public class CmsContentsCache : CacheInstance, IContentCache
    {
        #region Init

        public CmsContentsCache()
        { }

        private static readonly TimeSpan DefaultExpiration = TimeSpan.FromHours(1);

        #endregion

        #region Keys

        private const string LabelKeyPrefix = "single_label";
        private const string LabelPageKeyPrefix = "label_page";
        private const string InstalledCulturesKey = "installed_cultures";

        #endregion
        
        #region Label

        public Label RetreiveLabel(CultureInfo culture, string labelId, Func<Label> initializer)
        {
            var key = BuildLabelKey(culture, labelId);
            return Retreive(key, initializer, DefaultExpiration);
        }

        private string BuildLabelKey(CultureInfo culture, string labelId)
        {
            return $"{LabelKeyPrefix}_{culture.Name}_{labelId}";
        }

        #endregion
        
        #region Label Page

        public LabelPage RetreiveLabelPage(CultureInfo culture, string pageId, Func<LabelPage> initializer)
        {
            var key = BuildLabelPageKey(culture, pageId);
            return Retreive(key, initializer, DefaultExpiration);
        }

        private string BuildLabelPageKey(CultureInfo culture, string pageId)
        {
            return $"{LabelPageKeyPrefix}_{culture.Name}_{pageId}";
        }

        #endregion

        #region Cultures

        public IEnumerable<CultureInfo> RetreiveInstalledCultures(Func<IEnumerable<CultureInfo>> initializer)
        {
            return Retreive(InstalledCulturesKey, initializer, DefaultExpiration);
        }

        #endregion
    }
}
