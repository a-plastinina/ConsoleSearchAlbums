using System;
using System.Net;
using System.Text;

namespace ConsoleSearchAlbums
{
    public class LibraryRequest
    {
        string Url;
        public LibraryRequest(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("url");
            Url = url;
        }
        public string Get(string search)
        {
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                if (!string.IsNullOrWhiteSpace(search))
                {
                    webClient.QueryString.Add("q", search.Replace(" ", "+"));
                }
                // Выполняем запрос по адресу и получаем ответ в виде строки
                return webClient.DownloadString(Url);
            }
        }
    }
}
