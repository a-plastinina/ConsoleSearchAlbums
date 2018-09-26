using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ConsoleSearchAlbums
{
    public class CashLibrary : IStateRequest
    {
        string PathXmlFile;
        XDocument XmlDocument;

        public CashLibrary(string filePathCash)
        {
            PathXmlFile = filePathCash;
        }

        public bool IsSucceed { get; private set; }

        public IEnumerable<IAlbum> GetAlbums(string artist)
        {
            OpenDocument();

            if (!IsSucceed) return null;

            var regexArtist = new Regex($@"\b{artist}\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return from a in XmlDocument.Root.Elements("album")
                   where regexArtist.IsMatch(a.Attribute("artist").Value)
                   select new Album()
                   {
                       Artist = a.Attribute("artist").Value,
                       Name = a.Attribute("name").Value
                   };
        }

        public string GetMessage()
        {
            return "Чтение данных из кэша.";
        }

        public void Write(IEnumerable<IAlbum> albums)
        {
            if (albums.Count() == 0) return;

            bool isNewDocument = !File.Exists(PathXmlFile);

            if (isNewDocument) CreateDocument();
            else OpenDocument();

            var root = XmlDocument.Root;
            foreach (var album in albums)
            {
                if (isNewDocument) AddAlbum(root, album);
                else if (!ExistAlbum(root, album)) AddAlbum(root, album);
                else continue;
            }
            XmlDocument.Save(PathXmlFile);
        }

        public void CreateDocument()
        {
            XmlDocument = new XDocument(new XElement("albums-result"));
        }

        public void OpenDocument()
        {
            IsSucceed = File.Exists(PathXmlFile);
            try
            {
                using (var stream = new StreamReader(PathXmlFile, Encoding.GetEncoding(1251)))
                {
                    XmlDocument = XDocument.Load(stream);
                    stream.Close();
                }
            }
            catch (Exception)
            {
                IsSucceed = false;
            }
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

    }
}