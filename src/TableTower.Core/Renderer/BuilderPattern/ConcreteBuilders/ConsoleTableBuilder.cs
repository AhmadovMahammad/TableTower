using System.Text;
using TableTower.Core.Models;

namespace TableTower.Core.Renderer.BuilderPattern.ConcreteBuilders;
public class ConsoleTableBuilder : IBuilder
{
    private Table _table = null!;
    private readonly StringBuilder _stringBuilder = new();

    public void SetTable(Table table)
    {
        _table = table;
    }

    public void BuildTitle()
    {
        if (!string.IsNullOrWhiteSpace(_table.Title))
        {
            _stringBuilder.AppendLine(_table.Title.ToUpper());
        }
    }

    public void BuildHeader()
    {
        // mock
        _stringBuilder.AppendLine("HEADER");
    }

    public void BuildBody()
    {
        // mock
        _stringBuilder.AppendLine("BODY");
    }

    public void BuildFooter()
    {
        // mock
        _stringBuilder.AppendLine("FOOTER");
    }

    public string GetResult() => _stringBuilder.ToString();
}