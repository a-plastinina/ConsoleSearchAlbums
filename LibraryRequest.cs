using System;
using System.Net;
using System.Text;

namespace ConsoleSearchAlbums
{
    public class LibraryWeb : ILibraryWeb
    {
        readonly string Url;
        static WebClient client;

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

        public LibraryWeb(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");
            Url = url;

            CreateWebClient();
        }

        public bool IsSucceed { get; private set; }

        public string HtmlResponse { get; private set; }

        public void Read(string search)
        {
            try
            { 
                client.QueryString.Add("q", search.Replace(" ", "+"));
                HtmlResponse = client.DownloadString(Url);
                IsSucceed = true;
            }
            catch (Exception)
            {
                IsSucceed = false;
            }
        }
    }
}
