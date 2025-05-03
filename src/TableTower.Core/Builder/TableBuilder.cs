using TableTower.Core.Enums;
using TableTower.Core.Models;
using TableTower.Core.Themes;

namespace TableTower.Core.Builder;
public sealed class TableBuilder
{
    private string? _title;
    private readonly List<Column> _columns = [];
    private readonly List<Row> _rows = [];
    private ITheme? _theme;
    private bool _showRowLines = true;
    private bool _wrapData = true;
    private int _rowIndex;
    private int _columnCount;

    public TableBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public TableBuilder WithColumns(params string[] columns)
    {
        foreach (string column in columns)
        {
            _columns.Add(new Column(column, HorizontalAlignment.Left));
        }

        _columnCount += columns.Length;
        return this;
    }

    public TableBuilder WithColumn(string header, HorizontalAlignment alignment = HorizontalAlignment.Left)
    {
        _columns.Add(new Column(header, alignment));
        _columnCount++;
        return this;
    }

    public TableBuilder AddRow(params object[] values)
    {
        var cellFormats = values.Select(val =>
        {
            return ValueTuple.Create<object?, HorizontalAlignment, ConsoleColor?>(val, HorizontalAlignment.Left, ConsoleColor.White);
        }).ToArray();

        return AddFormattedRow(cellFormats);
    }

    public TableBuilder AddFormattedRow(params (object? Value, HorizontalAlignment Alignment, ConsoleColor? Color)[] cellFormats)
    {
        if (cellFormats.Length > _columnCount)
        {
            throw new ArgumentException("Parameter counts do not match with columns count.");
        }

        List<Cell> cells = [];
        foreach (var cellFormat in cellFormats)
        {
            cells.Add(new Cell(cellFormat.Value, cellFormat.Alignment, cellFormat.Color));
        }

        _rows.Add(new Row(cells, _rowIndex++));
        return this;
    }

    public TableBuilder SetTheme(ITheme theme)
    {
        _theme = theme;
        return this;
    }

    public TableBuilder ShowRowLines(bool show)
    {
        _showRowLines = show;
        return this;
    }

    public TableBuilder WrapData(bool wrapData)
    {
        _wrapData = wrapData;
        return this;
    }

    public Table Build()
    {
        _theme ??= new RoundedTheme();
        return new Table(_title, _columns, _rows, _theme, _showRowLines, _wrapData);
    }
}