using TableTower.Core.Themes;

namespace TableTower.Core.Models;
public class Table : IEquatable<Table>
{
    public string? Title { get; }
    public IReadOnlyList<Column> Columns { get; }
    public IReadOnlyList<Row> Rows { get; }
    public ITheme Theme { get; }
    public bool ShowRowLines { get; }
    public bool WrapData { get; }

    public Table(string? title,
                 IReadOnlyList<Column> columns,
                 IReadOnlyList<Row> rows,
                 ITheme theme,
                 bool showRowLines,
                 bool wrapData)
    {
        Title = title;
        Columns = columns;
        Rows = rows;
        Theme = theme;
        ShowRowLines = showRowLines;
        WrapData = wrapData;
    }

    public bool Equals(Table? other)
    {
        if (other == null) return false;
        return Title == other.Title &&
               Columns.SequenceEqual(other.Columns) &&
               Rows.SequenceEqual(other.Rows) &&
               Theme == other.Theme &&
               ShowRowLines == other.ShowRowLines &&
               WrapData == other.WrapData;
    }

    public override bool Equals(object? obj) => Equals(obj as Table);

    public override int GetHashCode()
    {
        HashCode hashCode = new HashCode();

        foreach (Column column in Columns)
        {
            hashCode.Add(column.GetHashCode());
        }

        foreach (Row row in Rows)
        {
            hashCode.Add(row.GetHashCode());
        }

        hashCode.Add(Title);
        hashCode.Add(Theme);
        hashCode.Add(ShowRowLines);
        hashCode.Add(WrapData);

        return hashCode.ToHashCode();
    }
}
