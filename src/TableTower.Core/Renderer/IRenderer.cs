using TableTower.Core.Models;

namespace TableTower.Core.Renderer;
public interface IRenderer
{
    string Render(Table table);
    void Print(Table table);
}
