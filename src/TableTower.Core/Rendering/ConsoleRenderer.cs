using TableTower.Core.Models;
using TableTower.Core.Rendering.BuilderPattern;
using TableTower.Core.Rendering.BuilderPattern.ConcreteBuilders;

namespace TableTower.Core.Rendering;
public class ConsoleRenderer : IRenderer
{
    private readonly RenderDirector _director;
    private readonly IBuilder _builder;
    private readonly bool _buildSimpleTable;

    public ConsoleRenderer(bool buildSimpleTable = false)
    {
        _builder = new ConsoleTableBuilder();
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