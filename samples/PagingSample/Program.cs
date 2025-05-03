using System.Text;
using TableTower.Core.Builder;
using TableTower.Core.Paging;
using TableTower.Core.Themes;
using TableTower.Data;

namespace PagingSample;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        IPager<User> pager = new DefaultPager<User>(InMemoryDatabase.Users, pageSize: 5);

        var consolePager = new ConsolePager<User>(pager, (data) =>
        {
            var builder = new TableBuilder(opt => { opt.Title = "Users Page"; opt.WrapData = false; })
                .WithData(data)
                .WithTheme(new RoundedTheme());

            return builder.Build();
        });

        consolePager.Run();
    }
}