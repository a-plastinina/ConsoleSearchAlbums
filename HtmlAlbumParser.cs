using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleSearchAlbums
{
    public class HtmlAlbumParser
    {
        private readonly string CssSelectorAlbum;
        private readonly string CssSelectorMessage;
        private IHtmlDocument Document;

        public readonly bool IsSucceed;
        public string SearchResultMessage { get; private set; }

        public HtmlAlbumParser(ILibraryWeb library, string cssSelectorAlbum, string cssSelectorMessageNotFound)
        {
            if (string.IsNullOrWhiteSpace(cssSelectorAlbum))
                throw new ArgumentNullException("cssSelectorAlbum");

            CssSelectorAlbum = cssSelectorAlbum;
            CssSelectorMessage = cssSelectorMessageNotFound;

            var parser = new HtmlParser();
            if (library.IsSucceed) Document = parser.Parse(library.HtmlResponse);
            IsSucceed = Document != null;
        }

        public IEnumerable<IAlbum> GetAlbums()
        {
            if (!IsSucceed)
            {
                yield return null;
            }
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

        public string ShowMessage()
        {
            if (!IsSucceed)
                return "Неверный формат данных: IHtmlDocument.";
            if (string.IsNullOrWhiteSpace(CssSelectorMessage))
                return string.Empty;

            var result = Document.QuerySelector(CssSelectorMessage);
            return result == null ? string.Empty : result.InnerHtml;
        }
    }
}
