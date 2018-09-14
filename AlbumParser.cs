using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using AngleSharp.Dom;
using System.Text.RegularExpressions;

namespace ConsoleSearchAlbums
{
    public class AlbumParser
    {
        private string CssSelectorAlbum;
        private string CssSelectorMessage = "p.message";

        public AlbumParser()
            :this(".album-result-heading")
        { }
        private AlbumParser(string cssSelectorAlbum)
        {
            CssSelectorAlbum = cssSelectorAlbum;
        }

        public IEnumerable<string> Search(string response)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(response);

            var artistResult = ExtractResult(document, CssSelectorAlbum);

            if (artistResult.Count() > 0)
                return artistResult;
            else
                return ExtractResultMessage(document, CssSelectorMessage);
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
