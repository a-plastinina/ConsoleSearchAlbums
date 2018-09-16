using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ConsoleSearchAlbums
{
    internal class LibraryCash: ILibraryCash
    {
        private string FilePathCash;

        public LibraryCash(string filePathCash)
        {
            if (string.IsNullOrWhiteSpace(filePathCash))
                throw new ArgumentNullException("filePathCash");
            
            this.FilePathCash = filePathCash;
        }

        public void Write(IEnumerable<IAlbum> albums)
        {
            var xmlDocument = new XDocument(new XElement("albums-result"));
            var root = xmlDocument.Root;

            foreach (var album in albums)
            {
                root.Add(new XElement("album", 
                    new XAttribute("artist", album.Artist),
                    new XAttribute("name", album.Name)));
            }
            xmlDocument.Save(FilePathCash);
        }

        public IEnumerable<IAlbum> Read(string nameArtist)
        {
            var xmlDocument = XDocument.Load(FilePathCash);

            var searchResult = from a in xmlDocument.Root.Elements("album")
                               where a.Attribute("artist").Value.ToLower() == nameArtist.Trim().ToLower()
                               select new Album()
                               {
                                   Artist = a.Attribute("artist").Value,
                                   Name = a.Attribute("name").Value
                               };

            foreach (var item in searchResult)
                yield return item;
        }
    }
}