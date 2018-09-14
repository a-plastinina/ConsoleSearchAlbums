using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace ConsoleSearchAlbums
{
    class Program
    {
        static void Main(string[] args)
        {
            string search, response = string.Empty;
            IEnumerable<string> albums = null;

            // Адрес ресурса, к которому выполняется запрос
            string url = "https://www.last.fm/ru/search/albums"; ;
            Console.WriteLine("Добро пожаловать на last.fm.\n");
            do
            {
                Console.WriteLine("Введите имя исполнителя для поиска альбомов:");
                search = Console.ReadLine();

                Console.WriteLine("Выполняется поиск...\n");
                response = Get(url, search);

                var parser = new AlbumParser();
                albums = parser.Search(response);

                Console.WriteLine($"Результат поиска \"{search}\":\n");
                foreach (var item in albums)
                {
                    Console.WriteLine(item);
                }

                Console.Write("\nДля продолжения нажмите любую клавишу. Чтобы завершить работу, нажмите 0: ");
            } while (Console.ReadLine() != "0");
        }

        private static string Get(string url, string search)
        {
            // Создаём объект WebClient
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                if (!string.IsNullOrWhiteSpace(search))
                {
                    webClient.QueryString.Add("q", search.Replace(" ","+"));
                    webClient.QueryString.Add("format", "json");
                }
                // Выполняем запрос по адресу и получаем ответ в виде строки
                return webClient.DownloadString(url);
            }
        }
        
    }
}
