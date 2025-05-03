using TableTower.Core.Models;

namespace TableTower.Core.Renderer.BuilderPattern;
public interface IBuilder
{
    void SetTable(Table table);
    void BuildTitle();
    void BuildHeader();
    void BuildBody();
    void BuildFooter();
    string GetResult();
}
