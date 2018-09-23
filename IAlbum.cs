using System.Collections.Generic;
using System.Xml.Linq;

namespace ConsoleSearchAlbums
{
    public interface IAlbum
    {
        string Artist { get; }
        string Name { get; }
        bool Equals(string artist, string name);
    }

    public interface IAlbumParser
    {
        IEnumerable<IAlbum> GetAlbums(string source);
        string GetMessage();
    }
}