using TableTower.Core.Enums;

namespace TableTower.Core.Models;
public class Column : IEquatable<Column>
{
    public string Header { get; }
    public HorizontalAlignment HorizontalAlignment { get; }
    public int? Width { get; }
    public bool Wrap { get; }

    public Column(string header, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, int? width = null, bool wrap = false)
    {
        Header = header;
        HorizontalAlignment = horizontalAlignment;
        Width = width;
        Wrap = wrap;
    }

    public bool Equals(Column? other)
    {
        if (other == null) return false;
        return Header == other.Header &&
               HorizontalAlignment == other.HorizontalAlignment &&
               Width == other.Width &&
               Wrap == other.Wrap;
    }

    public override bool Equals(object? obj) => Equals(obj as Column);

    public override int GetHashCode()
    {
        return HashCode.Combine(Header, HorizontalAlignment, Width, Wrap);
    }
}
