using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ConsoleSearchAlbums
{
    public class CashXmlLibrary : ILibraryRequest
    {
        XmlAlbumParser XmlParser;
        ILibraryRequest DefaultLibrary;

        public CashXmlLibrary(ILibraryRequest defaultLibrary, XmlAlbumParser xmlParser)
        {
            XmlParser = xmlParser;
            DefaultLibrary = defaultLibrary;
        }

        public bool IsSucceed { get; private set; }

        public IEnumerable<IAlbum> GetAlbums(string artist)
        {
            var result = DefaultLibrary.GetAlbums(artist);
            if (DefaultLibrary.IsSucceed)
            {
                XmlParser.Write(result);
                return result;
            }
            else
            {
                return XmlParser.GetAlbums(artist);
            }
        }

        public string GetMessage()
        {
            return DefaultLibrary.IsSucceed ? DefaultLibrary.GetMessage() : "Чтение данных из кэша.";
        }
    }
}