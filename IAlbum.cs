using System.Xml.Linq;

namespace ConsoleSearchAlbums
{
    public interface IAlbum
    {
        string Artist { get; }
        string Name { get; }
        bool Equals(string artist, string name);
    }

    public interface ILibraryRequest
    {
        bool IsSucceed { get; }
        void Read(string search);
    }

    public interface ILibraryWeb : ILibraryRequest
    {
        string HtmlResponse { get; }
    }

    public interface ILibraryCash : ILibraryRequest
    {
        XDocument XmlDocument { get; }
    }
}