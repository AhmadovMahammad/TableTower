using TableTower.Core.Models;
using TableTower.Core.Renderer.BuilderPattern.ConcreteBuilders;

namespace TableTower.Core.Renderer.BuilderPattern;
public class RenderDirector
{
    private readonly IBuilder _builder;
    public RenderDirector(IBuilder builder) => _builder = builder;

    public void BuildSimpleTable(Table table)
    {
        _builder.SetTable(table);
        _builder.BuildHeader();
        _builder.BuildBody();
    }

    public void BuildComplexTable(Table table)
    {
        _builder.SetTable(table);
        _builder.BuildTitle();
        _builder.BuildHeader();
        _builder.BuildBody();
        _builder.BuildFooter();
    }
}