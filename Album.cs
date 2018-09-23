namespace ConsoleSearchAlbums
{
    public class Album : object, IAlbum
    {
        public string Artist { get; set; }
        public string Name { get; set; }

        public bool Equals(string artist, string name)
        {
            return string.Equals(Artist, artist, System.StringComparison.CurrentCultureIgnoreCase)
                && string.Equals(Name, name, System.StringComparison.CurrentCultureIgnoreCase);
        }
                
        public override string ToString()
        {
            return $"{Artist} - {Name}";
        }
    }
}