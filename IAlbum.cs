using System.Collections.Generic;

namespace ConsoleSearchAlbums
{
    public interface IAlbum
    {
        string Artist { get; }
        string Name { get; }
    }

    public interface ILibraryCash
    {
        void Write(IEnumerable<IAlbum> list);
        IEnumerable<IAlbum> Read(string artist);
    }
}