using System.Collections.Generic;

namespace ConsoleSearchAlbums
{
    public interface ILibraryRequest
    {
        bool IsSucceed { get; }
        IEnumerable<IAlbum> GetAlbums(string artist);
        string GetMessage();
    }
}