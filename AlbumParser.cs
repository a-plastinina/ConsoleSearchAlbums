using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleSearchAlbums
{
    public class AlbumParser
    {
        private string CssSelectorAlbum;
        private string CssSelectorMessage;

        public AlbumParser(string cssSelectorAlbum, string cssSelectorMessageNotFound)
        {
            if (string.IsNullOrWhiteSpace(cssSelectorAlbum))
                throw new ArgumentNullException("cssSelectorAlbum");
            CssSelectorAlbum = cssSelectorAlbum;
            CssSelectorMessage = cssSelectorMessageNotFound;
        }

        public IEnumerable<string> Search(string response)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(response);
            var artistResult = ExtractResult(document, CssSelectorAlbum);

            if (artistResult.Count() > 0)
                return artistResult;
            else if (!string.IsNullOrWhiteSpace(CssSelectorMessage))
                return ExtractResultMessage(document, CssSelectorMessage);
            else return new string[] { };
        }

        private IEnumerable<string> ExtractResult(IHtmlDocument document, string cssSelector)
        {
            var artistResult = document.QuerySelectorAll(cssSelector);
            foreach (var element in artistResult)
            {
                yield return string.Format("{0} - {1}",
                    GetTextContent(element.NextElementSibling.TextContent),
                    GetTextContent(element.TextContent));
            }
        }

        private static string GetTextContent(string text)
        {
            return new Regex(@"\s{2,}|\n*").Replace(text, "");
        }

        private IEnumerable<string> ExtractResultMessage(IHtmlDocument document, string cssSelector)
        {
            var artistResult = document.QuerySelectorAll(cssSelector);
            foreach (var element in artistResult)
            {
                yield return element.InnerHtml;
            }
        }
    }
}
