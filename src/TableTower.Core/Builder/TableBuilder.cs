using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using TableTower.Core.Enums;
using TableTower.Core.Models;
using TableTower.Core.Themes;

namespace TableTower.Core.Builder;
public sealed class TableBuilder
{
    private static readonly Dictionary<Type, PropertyInfo[]> _propertyCache = new Dictionary<Type, PropertyInfo[]>(10);
    private readonly Dictionary<string, Type> _referenceTypes = [];
    private readonly TableOptions _tableOptions;

    private readonly List<Column> _columns = new(20);
    private readonly List<Row> _rows = new(100);

    private int _rowIndex;
    private int _columnCount;
    private ITheme? _theme;

    public TableBuilder(Action<TableOptions>? configure = null)
    {
        _tableOptions = new TableOptions();
        configure?.Invoke(_tableOptions);
    }

    public TableBuilder WithData<T>(T data, bool usePredefinedColumns = false)
    {
        return WithDataCollection([data], usePredefinedColumns);
    }

    public TableBuilder WithDataCollection<T>(IEnumerable<T> data, bool usePredefinedColumns = false)
    {
        if (data == null)
        {
            return this;
        }

        List<T> dataList = data as List<T> ?? [.. data];
        Type typeFromHandle = typeof(T);

        if (IsSimpleType(typeFromHandle))
        {
            return HandleSimpleType(dataList, usePredefinedColumns);
        }

        return HandleComplexType(dataList, typeFromHandle, usePredefinedColumns);
    }

    private TableBuilder HandleSimpleType<T>(List<T> dataList, bool usePredefinedColumns)
    {
        if (!usePredefinedColumns)
        {
            _columns.Clear();
            AddColumn("Value", HorizontalAlignment.Center);
        }

        if (_columns.Count > 1)
        {
            throw new ArgumentException("Primitive data types should only have one column name.");
        }

        foreach (T data in dataList)
        {
            var tuple = ValueTuple.Create((object?)data, (HorizontalAlignment?)HorizontalAlignment.Left, (ConsoleColor?)ConsoleColor.White);
            AddFormattedRow(tuple);
        }

        return this;
    }

    private TableBuilder HandleComplexType<T>(List<T> dataList, Type dataType, bool usePredefinedColumns)
    {
        if (!_propertyCache.TryGetValue(dataType, out PropertyInfo[]? propertiesInfo) && propertiesInfo == null)
        {
            propertiesInfo = dataType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            _propertyCache.Add(dataType, propertiesInfo);
        }

        if (!usePredefinedColumns)
        {
            PropertyInfo[] array = propertiesInfo;

            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo propInfo = array[i];
                string name = propInfo.Name;
                Type propType = propInfo.PropertyType;

                // Check if property is a complex type (potential reference and not a collection)
                if (!IsSimpleType(propType) && !propType.IsArray && !typeof(IEnumerable).IsAssignableFrom(propType))
                {
                    _referenceTypes[name] = propType;
                    name = $"{name} (Ref)";
                }
                else if (typeof(IEnumerable).IsAssignableFrom(propType) && propType != typeof(string))
                {
                    Type? elementType = propType.IsArray
                        ? propType.GetElementType()
                        : propType.IsGenericType
                            ? propType.GetGenericArguments().FirstOrDefault()
                            : null;

                    if (elementType != null && !IsSimpleType(elementType))
                    {
                        _referenceTypes[name] = elementType;
                        name = $"{name} (List Ref)";
                    }
                }

                AddColumn(name);
            }

            object?[] array2 = new object[propertiesInfo.Length];
            foreach (T data in dataList)
            {
                if (data == null)
                {
                    continue;
                }

                for (int j = 0; j < propertiesInfo.Length; j++)
                {
                    PropertyInfo propertyInfo = propertiesInfo[j];
                    object? propertyValue = propertyInfo.GetValue(data);

                    if (propertyValue == null)
                    {
                        array2[j] = "[null]";
                        continue;
                    }

                    if (_referenceTypes.TryGetValue(propertyInfo.Name, out Type? refType) && refType != null)
                    {
                        // Individual reference
                        if (!propertyInfo.PropertyType.IsArray && !typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            array2[j] = $"[{propertyInfo.Name}]";
                        }
                        else if (propertyValue is IEnumerable collection) // Collection reference
                        {
                            int count = 0;

                            IEnumerator enumerator = collection.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                count++;
                            }

                            array2[j] = $"[{count} item(s)]";
                        }
                    }
                    else
                    {
                        array2[j] = propertyValue ?? "[null]";
                    }
                }

                AddRow(array2);
            }
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
        (object?, HorizontalAlignment?, ConsoleColor?)[] array = new (object?, HorizontalAlignment?, ConsoleColor?)[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            array[i] = ValueTuple.Create<object?, HorizontalAlignment?, ConsoleColor?>(values[i], null, ConsoleColor.White);
        }

        return AddFormattedRow(array);
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

    // helper
    private bool IsSimpleType(Type type)
    {
        return
            type.IsPrimitive ||
            type.IsEnum ||
            type == typeof(string) ||
            type == typeof(decimal) ||
            type == typeof(DateTime) ||
            type == typeof(Guid);
    }
}
