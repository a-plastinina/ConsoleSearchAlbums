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
        private readonly string CssSelectorArtist;
        private IHtmlDocument Document;

        public HtmlAlbumParser(string cssSelectorArtist, string cssSelectorAlbum)
        {
            if (string.IsNullOrWhiteSpace(cssSelectorAlbum))
                throw new ArgumentNullException("cssSelectorAlbum");
            if (string.IsNullOrWhiteSpace(cssSelectorArtist))
                throw new ArgumentNullException("cssSelectorArtist");

            CssSelectorAlbum = cssSelectorAlbum;
            CssSelectorArtist = cssSelectorArtist;            
        }

        public IEnumerable<IAlbum> GetAlbums()
        {
            if (Document != null)
            {
                var artistNameElement = Document.QuerySelector(CssSelectorArtist);
                var artistResult = Document.QuerySelectorAll(CssSelectorAlbum);
                foreach (var element in artistResult)
                {
                    yield return new Album()
                    {
                        Artist = GetTextContent(artistNameElement.TextContent),
                        Name = GetTextContent(element.TextContent)
                    };
                }
            }
        }

        public void CreateDocument(string source)
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
            return string.Empty;
        }
    }
}
