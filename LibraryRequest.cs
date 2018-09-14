using System.Net;
using System.Text;

namespace ConsoleSearchAlbums
{
    public class LibraryRequest
    {
        public LibraryRequest()
        { }
        public string Get(string url, string search)
        {
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                if (!string.IsNullOrWhiteSpace(search))
                {
                    webClient.QueryString.Add("q", search.Replace(" ", "+"));
                }
                // Выполняем запрос по адресу и получаем ответ в виде строки
                return webClient.DownloadString(url);
            }
        }
    }
}
