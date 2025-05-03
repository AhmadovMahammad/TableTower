using System.Text;
using TableTower.Core.Extensions;
using TableTower.Core.Models;
using TableTower.Core.Themes;

namespace TableTower.Core.Renderer.BuilderPattern.ConcreteBuilders;
public class ConsoleTableBuilder : IBuilder
{
    private Table _table = null!;
    private ITheme _theme = null!;
    private readonly StringBuilder _stringBuilder = new();
    private List<int> _columnWidths = [];

    public void SetTable(Table table)
    {
        _table = table;
        _theme = table.Theme;
        _columnWidths = table.WrapData
            ? _table.Columns.Select(column => column.Width + 4).ToList()
            : CalculateColumnWidths(table.Columns, table.Rows);
    }

    private List<int> CalculateColumnWidths(IReadOnlyList<Column> columns, IReadOnlyList<Row> rows)
    {
        var widths = new List<int>(columns.Count);

        for (int i = 0; i < columns.Count; i++)
        {
            int currentMax = columns[i].Width;
            List<Cell> nthCells = rows.SelectMany(row => row.Cells.Skip(i).Take(1)).ToList();

            for (int j = 0; j < nthCells.Count; j++)
            {
                Cell currentCell = nthCells[j];
                if (currentCell.Width > currentMax)
                {
                    currentMax = currentCell.Width;
                }
            }

            widths.Add(currentMax + 4);
        }

        return widths;
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
        IReadOnlyList<Column> columns = _table.Columns;

        _stringBuilder.Append(_theme.TopLeftCorner);

        for (int i = 0; i < columns.Count; i++)
        {
            _stringBuilder.Append(_theme.HorizontalLine, _columnWidths[i]);

            if (i != columns.Count - 1)
            {
                _stringBuilder.Append(_theme.TopTee);
            }
        }

        _stringBuilder.Append(_theme.TopRightCorner).AppendLine();

        _stringBuilder.Append(_theme.VerticalLine);

        for (int i = 0; i < columns.Count; i++)
        {
            string header = columns[i].Header.ApplyAlignment(_columnWidths[i], columns[i].HorizontalAlignment);
            _stringBuilder.Append(header);

            if (i != columns.Count - 1)
            {
                _stringBuilder.Append(_theme.VerticalLine);
            }
        }

        _stringBuilder.Append(_theme.VerticalLine).AppendLine();

        _stringBuilder.Append(_theme.LeftTee);

        for (int i = 0; i < columns.Count; i++)
        {
            _stringBuilder.Append(_theme.HorizontalLine, _columnWidths[i]);

            if (i != columns.Count - 1)
            {
                _stringBuilder.Append(_theme.Intersection);
            }
        }

        _stringBuilder.Append(_theme.RightTee);
    }

    public void BuildBody()
    {
        int rowCount = _table.Rows.Count;

        for (int i = 0; i < rowCount; i++)
        {
            _stringBuilder.AppendLine();

            _stringBuilder.Append(_theme.VerticalLine);

            Row currentRow = _table.Rows[i];
            int cellsCount = currentRow.Cells.Count;

            for (int j = 0; j < cellsCount; j++)
            {
                Cell cell = currentRow.Cells[j];
                string formatted = cell.Value.ApplyAlignment(_columnWidths[j], cell.HorizontalAlignment);
                _stringBuilder.Append(formatted);

                if (j != cellsCount - 1)
                {
                    _stringBuilder.Append(_theme.VerticalLine);
                }
            }

            _stringBuilder.Append(_theme.VerticalLine);
        }
    }

    public void BuildFooter()
    {
        _stringBuilder.AppendLine();

        _stringBuilder.Append(_theme.BottomLeftCorner);

        for (int i = 0; i < _table.Columns.Count; i++)
        {
            _stringBuilder.Append(_theme.HorizontalLine, _columnWidths[i]);

            if (i != _table.Columns.Count - 1)
            {
                _stringBuilder.Append(_theme.BottomTee);
            }
        }

        _stringBuilder.Append(_theme.BottomRightCorner);
    }

    public string GetResult() => _stringBuilder.ToString();
}