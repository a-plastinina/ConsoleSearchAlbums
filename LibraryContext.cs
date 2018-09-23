using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSearchAlbums
{
    public class LibraryContext
    {
        IStateRequest State;

        public LibraryContext(string url, string cssSelectorResult, string cssSelectorNotFound)
        {
            State = new WebLibrary(url, new HtmlAlbumParser(cssSelectorResult, cssSelectorNotFound));
        }

        public void ChangeRequest(IStateRequest state)
        {
            State = state;
        }

        public IEnumerable<IAlbum> Get(string artist)
        {
            var albums = State.GetAlbums(artist);
            return albums;
        }

        public bool IsSucceed { get { return State.IsSucceed; } }

        public string GetMessage()
        {
            return State.GetMessage();
        }
    }
}
