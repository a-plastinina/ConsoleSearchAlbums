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

            // сайт выполняет поиск и среди наименований альбомов
            // среди артистов выдает результат Rianna => Rihanna 
            var webLib = new LibraryWeb(Url);
            webLib.Read(Search);

            if (webLib.IsSucceed)
            {
                var parser = new HtmlAlbumParser(webLib, cssSelectorResult, cssSelectorNotFound);

                if (parser.IsSucceed)
                {
                    albums = parser.GetAlbums();

                    if (albums != null && albums.Count() != 0)
                    {
                        var cash = new LibraryCash(filePathCash);
                        cash.Write(albums);
                    }
                }
                message = parser.ShowMessage();
            }
            else
            {
                message = "Заданный узел не отвечает. Чтение данных из кэша.\n";

                var cash = new LibraryCash(filePathCash);
                albums = cash.Read(Search);

                if (albums.Count() == 0)
                    message += "\nАльбомы не найдены";
            }

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
