using System.Reflection;
using TableTower.Core.Enums;
using TableTower.Core.Models;
using TableTower.Core.Themes;

namespace TableTower.Core.Builder;
public sealed class TableBuilder
{
    private readonly List<Column> _columns = new(20);
    private readonly List<Row> _rows = new(100);

    private int _rowIndex;
    private int _columnCount;
    private ITheme? _theme;
    private readonly TableOptions _tableOptions;

    private static readonly Dictionary<Type, PropertyInfo[]> _propertyCache = new Dictionary<Type, PropertyInfo[]>(10);

    public TableBuilder(Action<TableOptions>? configure = null)
    {
        _tableOptions = new TableOptions();
        configure?.Invoke(_tableOptions);
    }

    public TableBuilder WithData<T>(IEnumerable<T> data, bool usePredefinedColumns = false)
    {
        if (data == null)
        {
            return this;
        }

        Type dataType = typeof(T);

        PropertyInfo[] properties;
        lock (_propertyCache)
        {
            if (!_propertyCache.TryGetValue(dataType, out properties!))
            {
                properties = dataType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                _propertyCache[dataType] = properties;
            }
        }

        var dataList = data as List<T> ?? data.ToList();

        if (!usePredefinedColumns)
        {
            _columns.Clear();
            foreach (var prop in properties)
            {
                string columnName = prop.Name;
                AddColumn(columnName, HorizontalAlignment.Left);
            }
        }

        object?[] values = new object?[properties.Length];

        foreach (T item in dataList)
        {
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
        if (_columns.Capacity < columns.Length)
        {
            _columns.Capacity = columns.Length;
        }

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
        var cellFormats = new ValueTuple<object?, HorizontalAlignment?, ConsoleColor?>[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            cellFormats[i] = ValueTuple.Create<object?, HorizontalAlignment?, ConsoleColor?>(values[i], null, ConsoleColor.White);
        }

        return AddFormattedRow(cellFormats);
    }

    public TableBuilder AddFormattedRow(params (object? Value, HorizontalAlignment? HorizontalAlignment, ConsoleColor? Color)[] cellFormats)
    {
        if (cellFormats.Length != _columnCount)
        {
            throw new ArgumentException("Parameter counts do not match with columns count.");
        }

        List<Cell> cells = new(_columnCount);

        for (int i = 0; i < _columnCount; i++)
        {
            Column currentColumn = _columns[i];
            var cellFormat = cellFormats[i];
            cellFormat.HorizontalAlignment ??= currentColumn.HorizontalAlignment;

            cells.Add(new Cell(cellFormat.Value,
                               cellFormat.HorizontalAlignment ?? HorizontalAlignment.Left,
                               cellFormat.Color));
        }

        _rows.Add(new Row(cells, _rowIndex++));
        return this;
    }

    public TableBuilder WithTheme(ITheme theme)
    {
        _theme = theme;
        return this;
    }

    public Table Build()
    {
        _theme ??= new RoundedTheme();
        return new Table(_columns, _rows, _theme, _tableOptions);
    }
}