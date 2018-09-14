using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;

namespace ConsoleSearchAlbums
{
    public class AlbumParser
    {
        private string CssSelectorAlbum;
        private string CssSelectorMessage = "p.message";

        public AlbumParser():this(".album-result-heading a")
        { }
        private AlbumParser(string cssSelector)
        {
            CssSelectorAlbum = cssSelector;
        }

        public IEnumerable<string> Search(string response)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(response);

            var artistResult = ExtractResult(document, CssSelectorAlbum);

            if (artistResult.Count() > 0)
                return artistResult;
            else
                return ExtractResult(document, CssSelectorMessage);
        }

        private IEnumerable<string> ExtractResult(IHtmlDocument document, string cssSelector)
        {
            var artistResult = document.QuerySelectorAll(cssSelector);
            foreach (var element in artistResult)
            {
                yield return element.InnerHtml;
            }
        }
    }
}
