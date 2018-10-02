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

            // селектор для поиска в ответе
            var cssSelector = new CssSelector()
            {
                ArtistElement = ".header-title a",
                AlbumElement = ".album-grid-item-title a"
            };

            Console.WriteLine("Добро пожаловать на LAST.FM\n");
            var controller = new LibraryController(url, cssSelector);

            do
            {
                try
                {
                    search = GetArtistNameForSearch();

                    Console.WriteLine($"\nРезультат поиска \"{search}\":\n");

                    var results = controller.OutputResult(search);
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
            } while (IsFinish());

        }

        private static bool IsFinish()
        {
            return Console.ReadLine() != "0";
        }

        private static string GetArtistNameForSearch()
        {
            string search=string.Empty;
            do
            {
                Console.Write("Введите имя исполнителя для поиска альбомов: ");
                search = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(search));
            return search;
        }
    }
}
