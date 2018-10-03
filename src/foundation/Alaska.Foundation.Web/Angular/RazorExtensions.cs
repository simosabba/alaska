using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class RazorExtensions
    {
        public static HtmlString RenderAppIndexHead(this IHtmlHelper helper, string appIndexFile)
        {
            var doc = LoadHtmlAppFile(appIndexFile);
            if (doc == null)
                return HtmlString.Empty;

            var headNode = doc.DocumentNode.SelectSingleNode("//head");
            return headNode == null ?
                HtmlString.Empty :
                new HtmlString(headNode.InnerHtml);
        }

        public static HtmlString RenderAppIndexBody(this IHtmlHelper helper, string appIndexFile)
        {
            var doc = LoadHtmlAppFile(appIndexFile);
            if (doc == null)
                return HtmlString.Empty;

            var bodyNode = doc.DocumentNode.SelectSingleNode("//body");
            return bodyNode == null ?
                new HtmlString(doc.DocumentNode.InnerHtml) :
                new HtmlString(bodyNode.InnerHtml);
        }

        private static HtmlDocument LoadHtmlAppFile(string path)
        {
            var fileContent = File.ReadAllText(path);
            var doc = new HtmlDocument();
            doc.LoadHtml(fileContent);
            return doc;
        }
    }
}
