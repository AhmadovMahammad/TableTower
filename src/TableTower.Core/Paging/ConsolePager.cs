using TableTower.Core.Models;
using TableTower.Core.Rendering;

namespace TableTower.Core.Paging;
public sealed class ConsolePager<T> where T : class, new()
{
    private readonly IPager<T> _pager;
    private readonly Func<IEnumerable<T>, Table> _tableGenerator;

    public ConsolePager(IPager<T> pager, Func<IEnumerable<T>, Table> tableGenerator)
    {
        _pager = pager;
        _tableGenerator = tableGenerator;
    }

    public void Run()
    {
        ConsoleKey key = ConsoleKey.Spacebar;

        while (key != ConsoleKey.None)
        {
            IEnumerable<T> items = _pager.GetPageData(_pager.CurrentPage);
            Table table = _tableGenerator(items);

            Console.Clear();
            new ConsoleRenderer().Print(table);

            Console.WriteLine($"Page {_pager.CurrentPage} / {_pager.TotalPages}");
            Console.WriteLine("n - next, p - previous, q - quit");

            key = Console.ReadKey(true).Key;
            int currentPage = _pager.CurrentPage;

            switch (key)
            {
                case ConsoleKey.N:
                    {
                        if (_pager.CurrentPage < _pager.TotalPages)
                        {
                            _pager.GetPageData(currentPage + 1);
                        }

                        break;
                    }

                case ConsoleKey.P:
                    {
                        if (_pager.CurrentPage > 1)
                        {
                            _pager.GetPageData(currentPage - 1);
                        }

                        break;
                    }

                case ConsoleKey.Q:
                    {
                        key = ConsoleKey.None;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }
}
