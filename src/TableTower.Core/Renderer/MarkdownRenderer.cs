using TableTower.Core.Models;
using TableTower.Core.Renderer.BuilderPattern;
using TableTower.Core.Renderer.BuilderPattern.ConcreteBuilders;

namespace TableTower.Core.Renderer;
public class MarkdownRenderer : IRenderer
{
    private readonly RenderDirector _director;
    private readonly MarkdownTableBuilder _builder;
    private readonly bool _buildSimpleTable;

    public MarkdownRenderer(bool buildSimpleTable = false)
    {
        _builder = new MarkdownTableBuilder();
        _director = new RenderDirector(_builder);
        _buildSimpleTable = buildSimpleTable;
    }

    public string Render(Table table)
    {
        if (_buildSimpleTable) _director.BuildSimpleTable(table);
        else _director.BuildComplexTable(table);

        return _builder.GetResult();
    }

    public void Print(Table table)
    {
        string rendered = Render(table);
        Console.WriteLine(rendered);
    }
}
