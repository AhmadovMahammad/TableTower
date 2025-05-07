namespace ConsoleTable.Core.Models;
public class Row : IEquatable<Row>
{
    public IReadOnlyList<Cell> Cells { get; }
    public int Index { get; }
    public ConsoleColor? Background { get; }

    public Row(IEnumerable<Cell> cells, int index, ConsoleColor? background = null)
    {
        Cells = cells.ToList();
        Index = index;
        Background = background;
    }

    public bool Equals(Row? other)
    {
        if (other is null) return false;
        return Cells.SequenceEqual(other.Cells) &&
               Index == other.Index &&
               Background == other.Background;
    }

    public override bool Equals(object? obj) => Equals(obj as Row);

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();

        foreach (Cell cell in Cells)
        {
            hash.Add(cell.GetHashCode());
        }

        hash.Add(Index);
        hash.Add(Background);

        return hash.ToHashCode();
    }
}
