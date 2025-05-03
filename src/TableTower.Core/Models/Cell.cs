using TableTower.Core.Enums;
using TableTower.Core.Formatters;

namespace TableTower.Core.Models;
public class Cell : IEquatable<Cell>
{
    private readonly static ICellFormatter _cellFormatter = new DefaultCellFormatter();

    public object? Value { get; }
    public HorizontalAlignment HorizontalAlignment { get; }
    public ConsoleColor? Foreground { get; }

    public Cell(object? value, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, ConsoleColor? foreground = null)
    {
        Value = _cellFormatter.Format(value);
        HorizontalAlignment = horizontalAlignment;
        Foreground = foreground;
    }

    public bool Equals(Cell? other)
    {
        if (other == null) return false;
        return Equals(Value, other.Value) &&
               HorizontalAlignment == other.HorizontalAlignment &&
               Foreground == other.Foreground;
    }

    public override bool Equals(object? obj) => Equals(obj as Cell);

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, HorizontalAlignment, Foreground);
    }
}
