using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleSearchAlbums
{
    public class LibraryController
    {
        readonly ILibraryRequest CashLibrary;
        readonly string FilePathCash = @"..\..\albums.xml";

        public LibraryController(string url, CssSelector selector)
        {
            var webLibrary = CreateWebLibrary(url, selector);
            CashLibrary = new CashXmlLibrary(webLibrary, new XmlAlbumParser(FilePathCash));
        }

        private ILibraryRequest CreateWebLibrary(string url, CssSelector selector)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("url");
            return new WebLibrary(url.Trim(), new HtmlAlbumParser(selector));
        }

        public IEnumerable<string> OutputResult(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                throw new ArgumentNullException("search");

            IEnumerable<IAlbum> albums = null;
            string message = "";

            try
            {
                albums = CashLibrary.GetAlbums(search);
                message = CashLibrary.GetMessage();
                if (albums.Count() == 0) message += " Альбомы не найдены.";
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return ResultEnumerable(message, albums);
        }
        private IEnumerable<string> ResultEnumerable(string message, IEnumerable<IAlbum> albums)
        {
            if (!string.IsNullOrWhiteSpace(message))
                yield return message;
            
            if (albums != null)
                foreach (var album in albums)
                {
                    yield return album.ToString();
                }
        }
    }
}
