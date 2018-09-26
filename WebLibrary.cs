using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ConsoleSearchAlbums
{
    internal class WebLibrary : IStateRequest
    {
        readonly string Url;
        readonly IAlbumParser Parser;

        static WebClient client;

        public bool IsSucceed { get; private set; }

        private void CreateWebClient()
        {
            if (client == null)
            {
                client = new WebClient()
                {
                    Encoding = Encoding.UTF8
                };
            }
        }

        public WebLibrary(string url, IAlbumParser parser)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");
            Url = url;

            Parser = parser ?? throw new ArgumentNullException("parser");

            CreateWebClient();
        }

        public IEnumerable<IAlbum> GetAlbums(string artist)
        {
            try
            {
                var response = client.DownloadString(string.Format(Url, artist.Replace(" ", "+")));
                IsSucceed = true;

                Parser.CreateDocument(response);
                return Parser.GetAlbums();
            }
            catch (Exception)
            {
                IsSucceed = false;
                return null;
            }
        }
        public string GetMessage()
        {
            return IsSucceed ? Parser.GetMessage() : "Нет подключения к Интернет. ";
        }
    }
}