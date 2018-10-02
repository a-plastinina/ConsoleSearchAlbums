using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleSearchAlbums
{
    public class HtmlAlbumParser
    {
        readonly CssSelector SelectorElement;
        private IHtmlDocument Document;

        public HtmlAlbumParser(CssSelector cssSelector)
        {
            if (cssSelector == null)
                throw new ArgumentNullException("cssSelector");
            if (string.IsNullOrWhiteSpace(cssSelector.AlbumElement))
                throw new ArgumentNullException("cssSelector.AlbumElement");
            if (string.IsNullOrWhiteSpace(cssSelector.ArtistElement))
                throw new ArgumentNullException("cssSelectorArtist");

            SelectorElement = cssSelector;
        }

        public IEnumerable<IAlbum> GetAlbums()
        {
            if (Document != null)
            {
                var artistNameElement = Document.QuerySelector(SelectorElement.ArtistElement);
                var albumElements = Document.QuerySelectorAll(SelectorElement.AlbumElement);
                                    
                foreach (var element in albumElements)
                {
                    yield return new Album()
                    {
                        Artist = GetValue(artistNameElement.TextContent),
                        Name = GetValue(element.TextContent)
                    };
                }
            }
        }

        public void CreateDocument(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new NullReferenceException("source");

            var parser = new HtmlParser();
            Document = parser.Parse(source);
        }

        private static string GetValue(string text)
        {
            return new Regex(@"\s{2,}|\n*").Replace(text, "").Trim();
        }

        public string GetMessage()
        {
            if (Document == null)
                return "Неверный формат данных: IHtmlDocument.";
            return string.Empty;
        }
    }
}
