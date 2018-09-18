namespace ConsoleSearchAlbums
{
    public class Album : object, IAlbum
    {
        public string Artist { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Artist} - {Name}";
        }
    }
}