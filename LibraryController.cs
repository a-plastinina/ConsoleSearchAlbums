using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleSearchAlbums
{
    public class LibraryController
    {
        readonly string Url;
        readonly string Search;

        public LibraryController(string url, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                throw new ArgumentNullException("search");

            Url = url.Trim();
            Search = search;
        }

        public IEnumerable<string> OutputResult(string cssSelectorResult, string cssSelectorNotFound)
        {
            IEnumerable<IAlbum> albums = null;
            string message = "";
            string filePathCash = @"..\..\albums.xml";

            try
            {
                var context = new LibraryContext(Url, cssSelectorResult, cssSelectorNotFound);
                albums = context.Get(Search);
                message = context.GetMessage();

                if (context.IsSucceed)
                {
                    var cashLibrary = new CashLibrary(filePathCash);
                    cashLibrary.Write(albums);
                }
                else
                {
                    context.ChangeRequest(new CashLibrary(filePathCash));
                    albums = context.Get(Search);
                    message = context.GetMessage();
                }                
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return Output(message, albums);
        }
        private IEnumerable<string> Output(string message, IEnumerable<IAlbum> albums)
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
