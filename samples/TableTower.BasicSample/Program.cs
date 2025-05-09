using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
        Console.OutputEncoding = Encoding.UTF8;

        string dir = AppDomain.CurrentDomain.BaseDirectory;
        Assembly assembly = Assembly.LoadFrom(Path.Combine(dir, "TableTower.Core.dll"));

        var themeTypes = assembly
            .GetExportedTypes()
            .Where(t => !t.IsAbstract &&
                        !t.IsInterface &&
                        typeof(ITheme).IsAssignableFrom(t));

        //foreach (Type type in themeTypes)
        foreach (Type type in themeTypes.Where(t => t.IsAssignableFrom(typeof(RoundedTheme))))
        {
            object? instance = Activator.CreateInstance(type);
            if (instance != null)
            {
                ITheme theme = (ITheme)instance;

                ShowPrimitiveDataExample(theme);

                //ShowUserObjectsExample(theme);

                //ShowCustomColumnDefinitionExample(theme);

                //ShowFormattedRowsExample(theme);
            }
        }
    }

    private static void ShowPrimitiveDataExample(ITheme theme)
    {
        var method = typeof(TableBuilder)
            .GetMethods()
            .First(m => m.Name == "WithData" && m.IsGenericMethod && m.GetParameters().Length >= 1);

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

            var table = builder.Build();
            new ConsoleRenderer().Print(table);

            Console.WriteLine("\n\n");
        }
    }

    private static void ShowUserObjectsExample(ITheme theme)
    {
        var builder = new TableBuilder(opt => { opt.Title = "Users"; })
            .WithData(InMemoryDatabase.Users)
            .WithTheme(theme);

        var table = builder.Build();

        new ConsoleRenderer().Print(table);

        Console.WriteLine("\n\n");
    }

    private static void ShowCustomColumnDefinitionExample(ITheme theme)
    {
        var builder = new TableBuilder(opt => { opt.Title = "Choosen Users Details"; })
            .WithColumn("ID")
            .WithColumn("Name", HorizontalAlignment.Left)
            .WithColumn("Country", HorizontalAlignment.Center)
            .WithColumn("Occupation", HorizontalAlignment.Right, 30)
            .WithTheme(theme);

        foreach (var user in InMemoryDatabase.Users)
        {
            builder.AddRow(user.ID, user.Name, user.Country, user.Occupation);
        }

        var table = builder.Build();

        new ConsoleRenderer().Print(table);

        Console.WriteLine("\n\n");
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

        var table = builder.Build();

        new ConsoleRenderer().Print(table);

        Console.WriteLine("\n\n");
    }
}