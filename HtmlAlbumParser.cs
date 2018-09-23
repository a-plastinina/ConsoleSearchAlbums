using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleSearchAlbums
{
    public class HtmlAlbumParser: IAlbumParser
    {
        private readonly string CssSelectorAlbum;
        private readonly string CssSelectorMessage;
        private IHtmlDocument Document;

        public HtmlAlbumParser(string cssSelectorAlbum, string cssSelectorMessageNotFound)
        {
            if (string.IsNullOrWhiteSpace(cssSelectorAlbum))
                throw new ArgumentNullException("cssSelectorAlbum");

            CssSelectorAlbum = cssSelectorAlbum;
            CssSelectorMessage = cssSelectorMessageNotFound;            
        }

        public IEnumerable<IAlbum> GetAlbums(string source)
        {
            CreateDocument(source);

            if (Document != null)
            {
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
        }

        private void CreateDocument(string source)
        {
            var parser = new HtmlParser();
            Document = parser.Parse(source);
        }

        private static string GetTextContent(string text)
        {
            return new Regex(@"\s{2,}|\n*").Replace(text, "");
        }

        public string GetMessage()
        {
            if (Document == null)
                return "Неверный формат данных: IHtmlDocument.";
            if (string.IsNullOrWhiteSpace(CssSelectorMessage))
                return string.Empty;

            var result = Document.QuerySelector(CssSelectorMessage);
            return result == null ? string.Empty : result.InnerHtml;
        }
    }
}
