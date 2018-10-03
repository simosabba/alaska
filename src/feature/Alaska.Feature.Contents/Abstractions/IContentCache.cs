using Alaska.Feature.Contents.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alaska.Feature.Contents.Abstractions
{
    public interface IContentCache
    {
        Label RetreiveLabel(CultureInfo culture, string labelId, Func<Label> initializer);
        LabelPage RetreiveLabelPage(CultureInfo culture, string pageId, Func<LabelPage> initializer);
        IEnumerable<CultureInfo> RetreiveInstalledCultures(Func<IEnumerable<CultureInfo>> initializer);
    }
}
