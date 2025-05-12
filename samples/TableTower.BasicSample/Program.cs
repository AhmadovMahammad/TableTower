using System.Collections;
using System.Reflection;
using TableTower.Core.Builder;
using TableTower.Core.Enums;
using TableTower.Core.Rendering;
using TableTower.Core.Themes;
using TableTower.Data;

namespace TableTower.BasicSample;
internal class Program
{
    private static void Main(string[] args)
    {
        string dir = AppDomain.CurrentDomain.BaseDirectory;
        Assembly assembly = Assembly.LoadFrom(Path.Combine(dir, "TableTower.Core.dll"));

        var themeTypes = assembly
            .GetExportedTypes()
            .Where(t => !t.IsAbstract &&
                        !t.IsInterface &&
                        typeof(ITheme).IsAssignableFrom(t));

        foreach (Type type in themeTypes.Where(t => t.IsAssignableFrom(typeof(RoundedTheme))))
        {
            object? instance = Activator.CreateInstance(type);
            if (instance != null)
            {
                ITheme theme = (ITheme)instance;

                //ShowPrimitiveDataExample(theme);
                ShowUserObjectsExample(theme);
                ShowCustomColumnDefinitionExample(theme);
                ShowFormattedRowsExample(theme);
                ShowNonListData(theme);
            }
        }
    }

    private static void ShowPrimitiveDataExample(ITheme theme)
    {
        var method = typeof(TableBuilder)
            .GetMethods()
            .First(m => m.Name == "WithDataCollection" && m.IsGenericMethod && m.GetParameters().Length >= 1);

        var listData = new List<IEnumerable>
        {
            InMemoryDatabase.StringData,
            InMemoryDatabase.DateData,
            InMemoryDatabase.PriceData,
            InMemoryDatabase.BooleanData,
            InMemoryDatabase.IntegerData,
        };

        foreach (var data in listData)
        {
            Type itemType = data.GetType().GetGenericArguments()[0];

            var builder = new TableBuilder(opt =>
            {
                opt.ShowRowLines = true;
                opt.Title = "Primitive Data Type";
            }).WithTheme(theme);

            MethodInfo genericMethod = method.MakeGenericMethod(itemType);
            genericMethod.Invoke(builder, [data, false]);

            Print(builder);
        }
    }

    private static void ShowUserObjectsExample(ITheme theme)
    {
        TableBuilder builder = new TableBuilder(opt =>
        {
            opt.Title = "Users";
            opt.ShowRowLines = false;
            opt.WrapData = false;
            opt.EnableDataCount = true;
        })
        .WithDataCollection(InMemoryDatabase.Users)
        .WithTheme(theme);

        Print(builder);
    }

    private static void ShowCustomColumnDefinitionExample(ITheme theme)
    {
        var builder = new TableBuilder(opt => { opt.Title = "Choosen Users Details"; })
            .WithColumns("ID", "Name")
            .WithColumn("Country", HorizontalAlignment.Center)
            .WithColumn("Occupation", HorizontalAlignment.Right, 30)
            .WithTheme(theme);

        foreach (var user in InMemoryDatabase.Users)
        {
            builder.AddRow(user.ID, user.Name, user.Country, user.Occupation);
        }

        Print(builder);
    }

    private static void ShowFormattedRowsExample(ITheme theme)
    {
        var builder = new TableBuilder(opt => { opt.Title = "Formatted Users Details"; })
            .WithColumns("ID", "Name", "Occupation", "Country")
            .WithTheme(theme);

        foreach (var user in InMemoryDatabase.Users)
        {
            builder.AddFormattedRow(
                (user.ID, HorizontalAlignment.Center, ConsoleColor.Yellow),
                (user.Name, HorizontalAlignment.Left, ConsoleColor.Green),
                (user.Occupation, HorizontalAlignment.Center, ConsoleColor.Cyan),
                (user.Country, HorizontalAlignment.Right, ConsoleColor.Magenta)
            );
        }

        Print(builder);
    }

    private static void ShowNonListData(ITheme theme)
    {
        TableBuilder builder = new TableBuilder(opt =>
        {
            opt.Title = "Users";
            opt.ShowRowLines = false;
            opt.WrapData = false;
            opt.EnableDataCount = true;
        })
        .WithData(InMemoryDatabase.NonListData)
        .WithTheme(theme);

        Print(builder);
    }

    private static void Print(TableBuilder builder)
    {
        var table = builder.Build();

        new ConsoleRenderer().Print(table);

        Console.WriteLine("\n\n");
    }
}