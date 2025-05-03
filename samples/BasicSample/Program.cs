using System.Reflection;
using System.Text;
using TableTower.Core.Builder;
using TableTower.Core.Enums;
using TableTower.Core.Models;
using TableTower.Core.Renderer;
using TableTower.Core.Themes;

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
                RunExample(themeType.Name, (ITheme)instance, true);
            }
        }
    }

    private static void RunExample(string title, ITheme theme, bool wrapData)
    {
        var builder = new TableBuilder()
            .WithTitle(title)
            .WithColumns("ID")
            .WithColumn("Name", HorizontalAlignment.Left, 20)
            .WithColumn("Occupation", HorizontalAlignment.Left, 20)
            .WithColumn("Country", HorizontalAlignment.Center, 20)
            .WithColumn("Description", HorizontalAlignment.Right, 30)
            .SetTheme(theme)
            .WrapData(true);

        builder.AddRow(1, "Mahammad Ahmadov", "Software Developer", "Azerbaijan", "Builds clean backend systems.");
        builder.AddRow(2, "Lale Hasanli", "UX Designer", "Azerbaijan", "Designs intuitive interfaces.");
        builder.AddRow(3, "Rashad Mammadov", "Product Manager", "Azerbaijan", "Leads user-focused products.");
        builder.AddRow(4, "Aygun Aliyeva", "Data Scientist", "Azerbaijan", "Finds insights in data.");
        builder.AddRow(5, "Togrul Huseynov", "DevOps Engineer", "Azerbaijan", "Builds automated pipelines.");
        builder.AddRow(6, "Narmin Karimova", "AI Researcher", "Azerbaijan", "Improves deep learning models.");
        builder.AddRow(7, "Kamran Safarov", "Security Analyst", "Azerbaijan", "Secures systems and data.");
        builder.AddRow(8, "Sevinc Ismayilova", "Frontend Engineer", "Azerbaijan", "Builds responsive UIs.");
        builder.AddRow(9, "Sahnise Shirinli", "Sales Specialist", "Azerbaijan", "Manages ticket sales.");

        var table = builder.Build();
        new ConsoleRenderer().Print(table);

        Console.WriteLine("\n\n");
    }
}