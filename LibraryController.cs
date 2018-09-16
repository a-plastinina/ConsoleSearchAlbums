using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleSearchAlbums
{
    public class LibraryController
    {
        string Url;
        string Search;

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
            try
            {
                var parser = new AlbumParser(cssSelectorResult, cssSelectorNotFound);
                var response = new LibraryRequest(Url).Get(Search);

                albums = parser.Search(response);
                message = parser.SearchResultMessage();

                var cash = new LibraryCash("albums.xml");
                cash.Write(albums);
            }
            catch (System.Net.WebException)
            {
                message = "Чтение данных из кэша";
                var cash = new LibraryCash("albums.xml");
                albums = cash.Read(Search);

                if (albums.Count() == 0)
                    message += "\nАльбомы не найдены";
            }

            if (!string.IsNullOrWhiteSpace(message))
                yield return message;

            foreach (var album in albums)
            {
                yield return $"{album.Artist} - {album.Name}";
            }
        }
    }
}
