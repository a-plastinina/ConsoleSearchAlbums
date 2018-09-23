using System;

namespace ConsoleSearchAlbums
{
    class Program
    {
        static void Main(string[] args)
        {
            string search = string.Empty;

            // Адрес ресурса, к которому выполняется запрос
            string url = "https://www.last.fm/ru/music/{0}/+albums";
            Console.WriteLine("Добро пожаловать на LAST.FM\n");
            do
            {
                try
                {
                    do
                    {
                        Console.Write("Введите имя исполнителя для поиска альбомов: ");
                        search = Console.ReadLine();
                    }
                    while (string.IsNullOrWhiteSpace(search));

                    Console.WriteLine($"\nРезультат поиска \"{search}\":\n");

                    var controller = new LibraryController(url, search);
                    var results = controller.OutputResult(".header-title a", ".album-grid-item-title a");

                    foreach (var item in results)
                    {
                        Console.WriteLine(item);
                    }

                    Console.Write("\nДля завершения работы, нажмите 0.\nДля продолжения нажмите Enter: ");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            } while (Console.ReadLine() != "0");

        }

    }
}
