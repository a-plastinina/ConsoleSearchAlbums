﻿using System;
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
            bool isNewDocument = !File.Exists(FilePathCash);
            var xmlDocument = OpenDocument(isNewDocument);
            var root = xmlDocument.Root;

            foreach (var album in albums)
            {
                if (isNewDocument)
                    AddAlbum(root, album);
                else if (!ExistAlbum(root, album))
                    AddAlbum(root, album);
                else continue;
            }
            xmlDocument.Save(FilePathCash);
        }

        private bool ExistAlbum(XElement root, IAlbum album)
        {
            return root.Elements("albums").Any(
                a => string.Compare(a.Attribute("artist").Value, album.Artist, true) == 0
                    && string.Compare(a.Attribute("name").Value, album.Name, true) == 0);
        }

        private static void AddAlbum(XElement root, IAlbum album)
        {
            root.Add(new XElement("album",
                new XAttribute("artist", album.Artist),
                new XAttribute("name", album.Name)));
        }

        private XDocument OpenDocument(bool isNewDocument)
        {
            return isNewDocument
                    ? new XDocument(new XElement("albums-result"))
                    : XDocument.Load(FilePathCash);
        }

        public IEnumerable<IAlbum> Read(string nameArtist)
        {
            var xmlDocument = XDocument.Load(FilePathCash);

            var searchResult = from a in xmlDocument.Root.Elements("album")
                               where string.Compare(a.Attribute("artist").Value, nameArtist.Trim(), true) == 0
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