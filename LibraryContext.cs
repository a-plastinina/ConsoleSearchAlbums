using System.Collections.Generic;
using System.Linq;

namespace ConsoleSearchAlbums
{
    public class LibraryContext
    {
        IStateRequest State;

        public LibraryContext(string url, CssSelector cssSelector)
        {
            State = new WebLibrary(url, new HtmlAlbumParser(cssSelector));
        }

        public void ChangeRequest(IStateRequest state)
        {
            State = state;
        }

        public IEnumerable<IAlbum> Get(string artist)
        {
            var albums = State.GetAlbums(artist);
            CountAlbums = albums != null ? albums.Count() : 0;
            return albums;
        }

        private int CountAlbums = 0;
        public bool IsSucceed { get { return State.IsSucceed; } }

        public string GetMessage()
        {
            var result = State.GetMessage();
            if (IsSucceed && CountAlbums == 0) result += "\nАльбомы не найдены";
            return result;
        }
    }
}
