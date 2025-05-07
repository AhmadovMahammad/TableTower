using ConsoleTable.Core.Models;

namespace ConsoleTable.Core.Rendering.BuilderPattern;
public class RenderDirector(IBuilder builder)
{
    private readonly IBuilder _builder = builder;

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