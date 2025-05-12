using TableTower.Core.Models;
using TableTower.Core.Rendering.BuilderPattern;

namespace TableTower.Core.Rendering;
public interface IRenderer
{
    string CustomRender(Action<IBuilder> builder);
    string Render(Table table);
    void Print(Table table);
}
