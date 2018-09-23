using System.Collections.Generic;

namespace ConsoleSearchAlbums
{
    public interface IStateRequest
    {
        bool IsSucceed { get; }
        IEnumerable<IAlbum> GetAlbums(string artist);
        string GetMessage();
    }
}