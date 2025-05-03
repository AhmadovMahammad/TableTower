using TableTower.Core.Themes;

namespace TableTower.Core.Models;
public class Table : IEquatable<Table>
{
    public string? Title { get; }
    public IReadOnlyList<Column> Columns { get; }
    public IReadOnlyList<Row> Rows { get; }
    public ITheme? Theme { get; }
    public bool ShowRowLines { get; }

    public Table(string? title, IReadOnlyList<Column> columns, IReadOnlyList<Row> rows, ITheme? theme, bool showRowLines)
    {
        Title = title;
        Columns = columns;
        Rows = rows;
        Theme = theme;
        ShowRowLines = showRowLines;
    }

    public bool Equals(Table? other)
    {
        if (other == null) return false;
        return Title == other.Title &&
               Columns.SequenceEqual(other.Columns) &&
               Rows.SequenceEqual(other.Rows) &&
               Theme == other.Theme &&
               ShowRowLines == other.ShowRowLines;
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

        return hashCode.ToHashCode();
    }
}
