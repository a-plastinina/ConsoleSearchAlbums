using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleSearchAlbums
{
    public class AlbumParser
    {
        private readonly string CssSelectorAlbum;
        private readonly string CssSelectorMessage;
        private IHtmlDocument Document;

        public AlbumParser(string cssSelectorAlbum, string cssSelectorMessageNotFound)
        {
            if (string.IsNullOrWhiteSpace(cssSelectorAlbum))
                throw new ArgumentNullException("cssSelectorAlbum");
            CssSelectorAlbum = cssSelectorAlbum;
            CssSelectorMessage = cssSelectorMessageNotFound;
        }

        public IEnumerable<IAlbum> Search(string response)
        {
            var parser = new HtmlParser();
            Document = parser.Parse(response);
            return ExtractResult();
        }

        private IEnumerable<IAlbum> ExtractResult()
        {
            if (Document == null) throw new NullReferenceException("document");

            var artistResult = Document.QuerySelectorAll(CssSelectorAlbum);
            foreach (var element in artistResult)
            {
                yield return new Album()
                {
                    Artist = GetTextContent(element.NextElementSibling.TextContent),
                    Name = GetTextContent(element.TextContent)
                };
            }
        }

        private static string GetTextContent(string text)
        {
            return new Regex(@"\s{2,}|\n*").Replace(text, "");
        }

        public string SearchResultMessage()
        {
            if (Document == null) throw new NullReferenceException("document");
            var result = Document.QuerySelector(CssSelectorMessage);
            return result == null ? "" : result.InnerHtml;
        }
    }
}
