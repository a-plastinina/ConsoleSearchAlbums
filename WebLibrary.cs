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
                client = new WebClient
                {
                    Encoding = Encoding.UTF8
                };
            }
            client.QueryString.Clear();
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
                client.QueryString.Add("q", artist.Replace(" ", "+"));
                var response = client.DownloadString(Url);
                IsSucceed = true;

                return Parser.GetAlbums(response);
            }
            catch (Exception)
            {
                IsSucceed = false;
                return null;
            }
        }
        public string GetMessage()
        {
            return IsSucceed ? Parser.GetMessage() : string.Empty;
        }
    }
}