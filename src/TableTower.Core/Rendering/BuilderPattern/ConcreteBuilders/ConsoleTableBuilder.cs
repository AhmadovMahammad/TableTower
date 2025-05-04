using System.Text;
using TableTower.Core.Extensions;
using TableTower.Core.Models;
using TableTower.Core.Renderer.BuilderPattern;
using TableTower.Core.Themes;

namespace TableTower.Core.Rendering.BuilderPattern.ConcreteBuilders;
public class ConsoleTableBuilder : IBuilder
{
    private Table _table = null!;
    private ITheme _theme = null!;

    // Pre-allocate StringBuilder with a reasonable capacity to avoid resizing
    private readonly StringBuilder _stringBuilder = new(4096);
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

            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                if (i < rows[rowIndex].Cells.Count)
                {
                    var cellWidth = rows[rowIndex].Cells[i].Width;
                    if (cellWidth > currentMax)
                    {
                        currentMax = cellWidth;
                    }
                }
            }

            widths.Add(currentMax + 4);
        }

        return widths;
    }


    public void BuildTitle()
    {
        _stringBuilder.Clear();

        if (!string.IsNullOrWhiteSpace(_table.Title))
        {
            _stringBuilder.AppendLine(_table.Title.ToUpper());
        }
    }

    public void BuildHeader()
    {
        IReadOnlyList<Column> columns = _table.Columns;

        _stringBuilder.Append(_theme.TopLeftCorner);

        int columnsCount = columns.Count;
        int lastColumnIndex = columnsCount - 1;

        for (int i = 0; i < columnsCount; i++)
        {
            _stringBuilder.Append(_theme.HorizontalLine, _columnWidths[i]);

            if (i != lastColumnIndex)
            {
                _stringBuilder.Append(_theme.TopTee);
            }
        }

        _stringBuilder.Append(_theme.TopRightCorner).AppendLine();
        _stringBuilder.Append(_theme.VerticalLine);

        for (int i = 0; i < columnsCount; i++)
        {
            string header = columns[i].Header.ApplyAlignment(_columnWidths[i], columns[i].HorizontalAlignment);
            _stringBuilder.Append(header);

            if (i != lastColumnIndex)
            {
                _stringBuilder.Append(_theme.VerticalLine);
            }
        }

        _stringBuilder.Append(_theme.VerticalLine).AppendLine();
        _stringBuilder.Append(_theme.LeftTee);

        for (int i = 0; i < columnsCount; i++)
        {
            _stringBuilder.Append(_theme.HorizontalLine, _columnWidths[i]);

            if (i != lastColumnIndex)
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
            int lastCellIndex = cellsCount - 1;

            for (int j = 0; j < cellsCount; j++)
            {
                Cell cell = currentRow.Cells[j];
                string formatted = cell.Value.ApplyAlignment(_columnWidths[j], cell.HorizontalAlignment);
                _stringBuilder.Append(formatted);

                if (j != lastCellIndex)
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

        int columnsCount = _table.Columns.Count;
        int lastColumnIndex = columnsCount - 1;

        for (int i = 0; i < columnsCount; i++)
        {
            _stringBuilder.Append(_theme.HorizontalLine, _columnWidths[i]);

            if (i != lastColumnIndex)
            {
                _stringBuilder.Append(_theme.BottomTee);
            }
        }

        _stringBuilder.Append(_theme.BottomRightCorner);

        if (_table.EnableDataCount)
        {
            _stringBuilder.AppendLine();

            var totalRows = _table.Rows.Count;
            var pageInfo = $"Total: {totalRows} row{(totalRows == 1 ? "" : "s")}";

            // Calculate the total width of the table footer line:
            // - Sum of all column widths
            // - (_table.Columns.Count - 1) vertical separators between columns
            // - 2 characters for the left and right borders of the table

            int totalWidth = 0;

            for (int i = 0; i < _columnWidths.Count; i++)
            {
                totalWidth += _columnWidths[i];
            }

            totalWidth += (columnsCount - 1) + 2;

            _stringBuilder.AppendLine(pageInfo.PadLeft(totalWidth));
        }
    }

    public string GetResult() => _stringBuilder.ToString();
}