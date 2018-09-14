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
        private string CssSelectorArtist;
        private string CssSelectorMessage = "p.message";

        public AlbumParser()
            :this(".album-result-heading", 
                  ".album-result-artist")
        { }
        private AlbumParser(string cssSelectorAlbum, string cssSelectorArtist)
        {
            //".album-result-inner"
            //".album-result-heading a"
            CssSelectorAlbum = cssSelectorAlbum;
            CssSelectorArtist = cssSelectorArtist;
        }

        public IEnumerable<string> Search(string response)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(response);

            var artistResult = ExtractResult(document, ".album-result-inner");

            if (artistResult.Count() > 0)
                return artistResult;
            else
                return ExtractResult2(document, CssSelectorMessage);
        }

        private IEnumerable<string> ExtractResult(IHtmlDocument document, string cssSelector)
        {
            var artistResult = document.QuerySelectorAll(cssSelector);
            foreach (var element in artistResult)
            {
                yield return string.Format("{0} - {1}",
                    GetTextContent(element.QuerySelector(CssSelectorArtist).TextContent),
                    GetTextContent(element.QuerySelector(CssSelectorAlbum).TextContent));
            }
        }

        private static string GetTextContent(string text)
        {
            return new Regex(@"\s{2,}|\n*").Replace(text, "");
        }

        private IEnumerable<string> ExtractResult2(IHtmlDocument document, string cssSelector)
        {
            var artistResult = document.QuerySelectorAll(cssSelector);
            foreach (var element in artistResult)
            {
                yield return element.InnerHtml;
            }
        }
    }
}
