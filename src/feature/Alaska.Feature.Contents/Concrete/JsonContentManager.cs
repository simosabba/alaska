using Alaska.Feature.Contents.Abstractions;
using Alaska.Feature.Contents.Concrete.Models;
using Alaska.Feature.Contents.Models;
using Alaska.Foundation.Core.Messaging.Email;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Alaska.Feature.Contents.Concrete
{
    internal class JsonContentManagerPaths
    {
        public const string RootFolder = "contents";
        public const string LanguagesFolder = "languages";
        public const string SitesFolder = "sites";
        public const string PagesFolder = "pages";
        public const string ComponentsFolder = "components";
        public const string DictionaryFolder = "dictionary";
    }

    public class JsonContentManager : ContentManagerBase
    {
        private readonly string _rootPath;

        public JsonContentManager(JsonContentServiceOptions options)
        {
            _rootPath = Path.Combine(options.ContentsRoot, JsonContentManagerPaths.RootFolder);
        }

        public override Component GetComponent(string id, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override EmailTemplate GetEmailTemplate(string id, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<CultureInfo> GetInstalledLanguages()
        {
            var languages = GetFileContent<List<LanguageInfo>>(JsonContentManagerPaths.LanguagesFolder, "languages.json");
            return languages
                .Select(x => CultureInfo.GetCultureInfo(x.Name))
                .ToList();
        }

        public override Item GetItem(string id, CultureInfo culture, ContentResolutionMode resolutionMode)
        {
            throw new NotImplementedException();
        }

        public override Label GetLabel(string labelId, CultureInfo culture)
        {
            var pageId = labelId.Split('/')[0];
            var labelKey = labelId.Split('/')[1];
            var page = GetLabelPage(pageId, culture);
            if (page == null)
                return null;

            return page.Labels.FirstOrDefault(x => x.Key.Equals(labelKey, StringComparison.InvariantCultureIgnoreCase));
        }

        public override LabelPage GetLabelPage(string pageId, CultureInfo culture)
        {
            var labels = GetFileContent<List<Label>>(culture, JsonContentManagerPaths.DictionaryFolder, pageId);
            if (labels == null)
                return null;

            return new LabelPage
            {
                Id = pageId,
                Culture = new Culture(culture),
                Labels = labels,
            };
        }

        public override string GetLabelPageId(string labelId)
        {
            throw new NotImplementedException();
        }

        public override Page GetPage(string id, CultureInfo culture, ContentResolutionMode resolutionMode)
        {
            throw new NotImplementedException();
        }

        public override Site GetSite(string route, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private T GetFileContent<T>(string category, string relativePath)
        {
            var path = GetPath(category, relativePath);
            if (!File.Exists(path))
                return default(T);

            var content = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(content);
        }

        private T GetFileContent<T>(CultureInfo culture, string category, string relativePath)
        {
            var path = GetPath(culture, category, relativePath);
            if (!File.Exists(path))
                return default(T);

            var content = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(content);
        }
        
        private string GetPath(CultureInfo culture, string category, string relativePath)
        {
            var folder = GetPath(category, relativePath);
            return Path.Combine(folder, $"{culture.Name.ToLower()}.json");
        }

        private string GetPath(string category, string relativePath)
        {
            var segments = Path.Combine(relativePath.Split('/'));
            return Path.Combine(_rootPath, category, segments);
        }
    }
}
