using System.Reflection;
using System.Text;
using TableTower.Core.Builder;
using TableTower.Core.Enums;
using TableTower.Core.Rendering;
using TableTower.Core.Themes;
using TableTower.Data;

namespace BasicSample;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        string dir = AppDomain.CurrentDomain.BaseDirectory;
        Assembly assembly = Assembly.LoadFrom(Path.Combine(dir, "TableTower.Core.dll"));

        var themes = assembly
            .GetExportedTypes()
            .Where(t => !t.IsAbstract &&
                        !t.IsInterface &&
                        typeof(ITheme).IsAssignableFrom(t));

        foreach (Type themeType in themes.Reverse())
        {
            object? instance = Activator.CreateInstance(themeType);
            if (instance != null)
            {
                RunExample(themeType.Name, (ITheme)instance, false);
            }
        }
    }

    private static void RunExample(string title, ITheme theme, bool wrapData)
    {
        //var builder = new TableBuilder()
        //    .WithTitle(title)
        //    .WithColumns("ID", "Name", "Occupation", "Country")
        //    .WithColumn("Description", HorizontalAlignment.Right, 40)
        //    .SetTheme(theme)
        //    .WrapData(wrapData);

        //foreach (User user in InMemoryDatabase.Users)
        //{
        //    builder.AddRow(user.ID, user.Name, user.Occupation, user.Country, user.Description);
        //}

        var builder = new TableBuilder()
            .WithTitle(title)
            .WithColumns("ID", "Name", "Occupation", "Country")
            .WithColumn("Description", HorizontalAlignment.Right, 40)
            .WithData(InMemoryDatabase.Users, true) // you can change it false and remove adding columns manually
            .SetTheme(theme)
            .WrapData(wrapData);

        var table = builder.Build();
        new ConsoleRenderer().Print(table);

        Console.WriteLine("\n\n");
    }
}