using ConsoleTable.Core.Models;

namespace ConsoleTable.Core.Rendering;
public interface IRenderer
{
    string Render(Table table);
    void Print(Table table);
}
