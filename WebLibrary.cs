﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ConsoleSearchAlbums
{
    internal class WebLibrary : ILibraryRequest
    {
        readonly string Url;
        readonly HtmlAlbumParser Parser;

        public bool IsSucceed { get; private set; }
        
        public WebLibrary(string url, HtmlAlbumParser parser)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");
            Url = url;

            Parser = parser ?? throw new ArgumentNullException("parser");
        }

        public IEnumerable<IAlbum> GetAlbums(string artist)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;

                    var response = client.DownloadString(string.Format(Url, artist.Replace(" ", "+")));
                    IsSucceed = true;

                    Parser.CreateDocument(response);
                    return Parser.GetAlbums();
                }
            }
            catch (Exception e)
            {
                IsSucceed = false;
                return null;
            }
        }
        public string GetMessage()
        {
            return IsSucceed ? Parser.GetMessage() : "Заданный узел не доступен. ";
        }
    }
}