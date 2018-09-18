using System.Collections.Generic;
using System.Xml.Linq;

namespace ConsoleSearchAlbums
{
    public interface IAlbum
    {
        string Artist { get; }
        string Name { get; }
    }

    public interface IResponse
    {
        IEnumerable<IAlbum> Search(string artist);
        string MessageResultSearch();
    }

    public interface ILibraryCashXml
    {
        XDocument XmlDocument { get; }
        void Write(IEnumerable<IAlbum> list);
        IEnumerable<IAlbum> Read(string artist);
    }
}