using System.Collections.Generic;

namespace ConsoleSearchAlbums
{
    public interface IAlbumParser
    {
        void CreateDocument(string source);
        IEnumerable<IAlbum> GetAlbums();
        string GetMessage();
    }
}
