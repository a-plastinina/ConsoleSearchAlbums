using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ConsoleSearchAlbums
{
    public class LibraryCash : ILibraryCash
    {
        private readonly string FilePathCash;
        public XDocument XmlDocument { get; private set; }

        public LibraryCash(string filePathCash)
        {
            if (string.IsNullOrWhiteSpace(filePathCash))
                throw new ArgumentNullException("filePathCash");
            
            this.FilePathCash = filePathCash;
        }

        public void Write(IEnumerable<IAlbum> albums)
        {
            bool isNewDocument = !File.Exists(FilePathCash);

            if (isNewDocument) CreateDocument();
            else OpenDocument();

            var root = XmlDocument.Root;
            foreach (var album in albums)
            {
                if (isNewDocument) AddAlbum(root, album);
                else if (!ExistAlbum(root, album)) AddAlbum(root, album);
                else continue;
            }
            XmlDocument.Save(FilePathCash);
        }

        public bool ExistAlbum(XElement root, IAlbum album)
        {
            return root.Elements("album").Any(
                a => album.Equals(a.Attribute("artist").Value, a.Attribute("name").Value));
        }

        public void AddAlbum(XElement root, IAlbum album)
        {
            root.Add(new XElement("album",
                new XAttribute("artist", album.Artist),
                new XAttribute("name", album.Name)));
        }

        private void CreateDocument()
        {
            XmlDocument = new XDocument(new XElement("albums-result"));
        }

        private void OpenDocument()
        {
            using (var stream = new StreamReader(FilePathCash, Encoding.GetEncoding(1251)))
            {
                XmlDocument = XDocument.Load(stream);
                stream.Close();
            }
        }

        public IEnumerable<IAlbum> Read(string nameArtist)
        {
            try
            {
                OpenDocument();
                var regexArtist = new Regex($@"\b{nameArtist}\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                return from a in XmlDocument.Root.Elements("album")
                       where regexArtist.IsMatch(a.Attribute("artist").Value)
                       select new Album()
                       {
                           Artist = a.Attribute("artist").Value,
                           Name = a.Attribute("name").Value
                       };
            }
            catch (FileNotFoundException)
            { }
            return new Album[] { };
        }
    }
}