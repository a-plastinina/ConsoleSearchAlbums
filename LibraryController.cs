using System;
using System.Collections.Generic;

namespace ConsoleSearchAlbums
{
    public class LibraryController
    {
        readonly CssSelector Selector;
        readonly string Url;

        public LibraryController(string url, CssSelector selector)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("url");
            Url = url.Trim();

            Selector = selector ?? throw new ArgumentNullException("selector");
        }

        public IEnumerable<string> OutputResult(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                throw new ArgumentNullException("search");

            IEnumerable<IAlbum> albums = null;
            string message = "";
            string filePathCash = @"..\..\albums.xml";

            try
            {
                var context = new LibraryContext(Url, Selector);
                albums = context.Get(search);
                message = context.GetMessage();

                if (context.IsSucceed)
                {
                    var cashLibrary = new CashLibrary(filePathCash);
                    cashLibrary.Write(albums);
                }
                else
                {
                    context.ChangeRequest(new CashLibrary(filePathCash));
                    albums = context.Get(search);
                    message += context.GetMessage();
                }
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
