using System.Reflection;
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

    public TableBuilder WithData<T>(IEnumerable<T> data, bool usePredefinedColumns = false)
    {
        if (data == null)
        {
            return this;
        }

        Type dataType = typeof(T);
        PropertyInfo[] properties = dataType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (!usePredefinedColumns)
        {
            _columns.Clear();
            foreach (var prop in properties)
            {
                string columnName = prop.Name;
                AddColumn(columnName, HorizontalAlignment.Left);
            }
        }

        foreach (object? item in data)
        {
            object?[] values = new object?[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                values[i] = properties[i].GetValue(item);
            }

            AddRow(values);
        }

        return this;
    }

    public TableBuilder WithColumns(params string[] columns)
    {
        foreach (string column in columns)
        {
            AddColumn(column, HorizontalAlignment.Left);
        }

        return this;
    }

    public TableBuilder WithColumn(string header, HorizontalAlignment alignment = HorizontalAlignment.Left, int? width = null)
    {
        AddColumn(header, alignment, width);
        return this;
    }

    private void AddColumn(string header, HorizontalAlignment alignment = HorizontalAlignment.Left, int? width = null)
    {
        _columns.Add(new Column(header, alignment, width));
        _columnCount++;
    }

    public TableBuilder AddRow(params object?[] values)
    {
        var cellFormats = values.Select(val =>
        {
            return ValueTuple.Create<object?, HorizontalAlignment?, ConsoleColor?>(val, null, ConsoleColor.White);
        }).ToArray();

        return AddFormattedRow(cellFormats);
    }

    public TableBuilder AddFormattedRow(params (object? Value, HorizontalAlignment? HorizontalAlignment, ConsoleColor? Color)[] cellFormats)
    {
        if (cellFormats.Length != _columnCount)
        {
            throw new ArgumentException("Parameter counts do not match with columns count.");
        }

        List<Cell> cells = [];
        List<Column> columns = _columns;

        for (int i = 0; i < _columnCount; i++)
        {
            Column currentColumn = columns[i];

            var cellFormat = cellFormats[i];
            cellFormat.HorizontalAlignment ??= currentColumn.HorizontalAlignment;

            cells.Add(new Cell(cellFormat.Value,
                               cellFormat.HorizontalAlignment ?? HorizontalAlignment.Left,
                               cellFormat.Color));
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