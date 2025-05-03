using TableTower.Core.Models;

namespace TableTower.Core.Rendering;
public interface IRenderer
{
    string Render(Table table);
    void Print(Table table);
}
