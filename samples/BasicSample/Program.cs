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
        Table table = new TableBuilder()
            .WithTitle("Users Details")
            .WithColumns("Name", "Age", "City")
            .AddRow("Mahammad", 21, "Sumgait")
            .AddFormattedRow
            (
                ("Ali", HorizontalAlignment.Left, null),
                (21, HorizontalAlignment.Center, null),
                ("Ganja", HorizontalAlignment.Right, null)
            ).SetTheme(new ClassicTheme())
            .Build();

        new ConsoleRenderer().Print(table);
    }
}