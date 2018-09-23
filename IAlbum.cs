namespace ConsoleSearchAlbums
{
    public interface IAlbum
    {
        string Artist { get; }
        string Name { get; }
        bool Equals(string artist, string name);
    }
}