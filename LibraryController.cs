using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSearchAlbums
{
    public class LibraryController
    {
        string Url;
        string Search;

        public LibraryController(string url, string search)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("url");
            if (string.IsNullOrWhiteSpace(search))
                throw new ArgumentNullException("search");

            Url = url.Trim();
            Search = search;
        }

        public IEnumerable<string> OutputResult(string cssSelectorResult, string cssSelectorNotFound)
        {
            var lib = new LibraryRequest();
            var response = lib.Get(Url, Search);

            var parser = new AlbumParser(cssSelectorResult, cssSelectorNotFound);
            return parser.Search(response);
        }
    }
}
