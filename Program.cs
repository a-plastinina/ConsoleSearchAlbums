using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ConsoleSearchAlbums
{
    class Program
    {
        static void Main(string[] args)
        {
            string search = string.Empty;

            // Адрес ресурса, к которому выполняется запрос
            string url = "https://www.last.fm/ru/search/albums"; ;
            Console.WriteLine("Добро пожаловать на last.fm.\n");
            do
            {
                Console.WriteLine("Введите имя исполнителя для поиска альбомов:");
                search = Console.ReadLine();

                Console.WriteLine($"\nРезультат поиска \"{search}\":\n");

                var controller = new LibraryController(url, search);
                var results = controller.OutputResult(".album-result-heading", "p.message");

                foreach (var item in results)
                {
                    Console.WriteLine(item);
                }

                Console.Write("\n\nДля продолжения нажмите любую клавишу.\nДля завершения работы, нажмите 0: ");
            } while (Console.ReadLine() != "0");
        }
    }
}
