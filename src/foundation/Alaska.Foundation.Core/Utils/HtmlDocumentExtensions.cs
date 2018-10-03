//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HtmlAgilityPack
//{
//    public static class HtmlDocumentExtensions
//    {
//        public static void ReplaceAttributeValue(this HtmlDocument document, string nodesXpathSelector, string attributeName, Func<string, string> valueFactory)
//        {
//            var nodes = document.DocumentNode.SelectNodes(nodesXpathSelector);
//            if (nodes == null)
//                return;

//            foreach (var node in nodes)
//                node.ReplaceAttributeValue(attributeName, valueFactory);
//        }

//        public static void ReplaceAttributeValue(this HtmlNode node, string attributeName, Func<string, string> valueFactory)
//        {
//            var nodeValue = node.GetAttributeValue(attributeName, string.Empty);
//            node.SetAttributeValue(attributeName, valueFactory(nodeValue));
//        }
//    }
//}
