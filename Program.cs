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
            bool isValidSearchLength = false;
            IEnumerable<string> albums = null;

            Console.OutputEncoding = Encoding.GetEncoding(1251);

            // Адрес ресурса, к которому выполняется запрос
            string url = "https://www.last.fm/ru/search/albums"; ;
            Console.WriteLine("\nДобро пожаловать на last.fm.\n");
            do
            {
                do
                {
                    Console.WriteLine("Введите имя исполнителя для поиска альбомов:");
                    search = Console.ReadLine();
                    if (!(isValidSearchLength = search.Length >= 3))
                        Console.WriteLine("Слишком короткое имя, поиск начинается с трех символов.");
                } while (!isValidSearchLength);

                Console.WriteLine("Выполняется поиск...");

                response = Get(url, search);

                var parser = new AlbumParser();
                albums = parser.Search(response);

                Console.WriteLine($"Результат поиска \"{search}\":");
                foreach (var item in albums)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine();

                Console.Write("Для продолжения нажмите любую клавишу. Чтобы завершить работу, нажмите 0: ");
            } while (Console.ReadLine() != "0");
        }

        private static string Get(string url, string search)
        {
            // Создаём объект WebClient
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.Unicode;
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
