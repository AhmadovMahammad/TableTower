using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;
using TableTower.Core.Extensions;
using TableTower.Core.Models;
using TableTower.Core.Themes;

namespace TableTower.Core.Rendering.BuilderPattern.ConcreteBuilders;
public class ConsoleTableBuilder : IBuilder
{
    private Table _table = null!;
    private ITheme _theme = null!;

    private readonly StringBuilder _stringBuilder = new(8192);
    private List<int> _columnWidths = [];

    private char[]? _charBuffer;
    private int _bufferSize;

    public void SetTable(Table table)
    {
        _table = table;
        _theme = table.Theme;

        _columnWidths = table.WrapData
            ? _table.Columns.Select(column => column.Width + 4).ToList()
            : CalculateColumnWidths(table.Columns, table.Rows);

        int totalWidth = 0;
        foreach (var width in _columnWidths)
        {
            totalWidth += width;
        }

        totalWidth = _columnWidths.Sum() + (_table.Columns.Count - 1) + 2;

        if (_charBuffer == null || _bufferSize < totalWidth)
        {
            _charBuffer = ArrayPool<char>.Shared.Rent(totalWidth);
            _bufferSize = totalWidth;
        }
    }

    private List<int> CalculateColumnWidths(IReadOnlyList<Column> columns, IReadOnlyList<Row> rows)
    {
        var widths = new List<int>(columns.Count);

        for (int i = 0; i < columns.Count; i++)
        {
            int currentMax = columns[i].Width;

            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                var row = rows[rowIndex];
                if (i < row.Cells.Count)
                {
                    var cellWidth = row.Cells[i].Width;
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
        int columnsCount = columns.Count;
        int lastColumnIndex = columnsCount - 1;

        _stringBuilder.Append(_theme.TopLeftCorner);

        for (int i = 0; i < columnsCount; i++)
        {
            AppendRepeated(_stringBuilder, _theme.HorizontalLine, _columnWidths[i]);

            if (i != lastColumnIndex)
            {
                _stringBuilder.Append(_theme.TopTee);
            }
        }

        _stringBuilder.Append(_theme.TopRightCorner).AppendLine();
        _stringBuilder.Append(_theme.VerticalLine);

        for (int i = 0; i < columnsCount; i++)
        {
            Span<char> cellBuffer = _charBuffer.AsSpan(0, _columnWidths[i]);
            columns[i].Header.AsSpan().ApplyAlignment(cellBuffer, _columnWidths[i], columns[i].HorizontalAlignment);
            _stringBuilder.Append(cellBuffer);

            if (i != lastColumnIndex)
            {
                _stringBuilder.Append(_theme.VerticalLine);
            }
        }

        _stringBuilder.Append(_theme.VerticalLine).AppendLine();
        _stringBuilder.Append(_theme.LeftTee);

        for (int i = 0; i < columnsCount; i++)
        {
            AppendRepeated(_stringBuilder, _theme.HorizontalLine, _columnWidths[i]);

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
        int columnsCount = _table.Columns.Count;
        int cellsCount = _table.Rows[0].Cells.Count;
        int lastCellIndex = cellsCount - 1;

        for (int i = 0; i < rowCount; i++)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append(_theme.VerticalLine);

            Row currentRow = _table.Rows[i];

            for (int j = 0; j < cellsCount; j++)
            {
                Cell cell = currentRow.Cells[j];
                Span<char> cellBuffer = _charBuffer.AsSpan(0, _columnWidths[j]);
                cell.Value.AsSpan().ApplyAlignment(cellBuffer, _columnWidths[j], cell.HorizontalAlignment);
                _stringBuilder.Append(cellBuffer);

                if (j != lastCellIndex)
                {
                    _stringBuilder.Append(_theme.VerticalLine);
                }
            }

            // If there are fewer cells than columns, pad with empty cells, probably we will never reach here.
            if (cellsCount < columnsCount)
            {
                for (int j = cellsCount; j < columnsCount; j++)
                {
                    Span<char> cellBuffer = _charBuffer.AsSpan(0, _columnWidths[j]);
                    cellBuffer.Fill(' ');
                    _stringBuilder.Append(cellBuffer);

                    if (j != columnsCount - 1)
                    {
                        _stringBuilder.Append(_theme.VerticalLine);
                    }
                }
            }

            _stringBuilder.Append(_theme.VerticalLine);

            if (_table.ShowRowLines && i < rowCount - 1)
            {
                BuildRowLines();
            }
        }
    }

    private void BuildRowLines()
    {
        _stringBuilder.AppendLine();
        _stringBuilder.Append(_theme.VerticalLine);

        int columnsCount = _table.Columns.Count;
        int lastColumnIndex = columnsCount - 1;

        for (int i = 0; i < columnsCount; i++)
        {
            AppendRepeated(_stringBuilder, _theme.HorizontalLine, _columnWidths[i]);

            if (i != lastColumnIndex)
            {
                _stringBuilder.Append(_theme.TopTee);
            }
        }

        _stringBuilder.Append(_theme.VerticalLine);
    }

    public void BuildFooter()
    {
        int columnsCount = _table.Columns.Count;
        int lastColumnIndex = columnsCount - 1;

        _stringBuilder.AppendLine();
        _stringBuilder.Append(_theme.BottomLeftCorner);

        for (int i = 0; i < columnsCount; i++)
        {
            AppendRepeated(_stringBuilder, _theme.HorizontalLine, _columnWidths[i]);

            if (i != lastColumnIndex)
            {
                _stringBuilder.Append(_theme.BottomTee);
            }
        }

        _stringBuilder.Append(_theme.BottomRightCorner);

        // The length of the page information may exceed the width of the columns plus the left and right borders (total 2 chars).
        // Thus, always rent a counter buffer as maximum number of them
        if (_table.EnableDataCount)
        {
            _stringBuilder.AppendLine();

            var totalRows = _table.Rows.Count;
            var pageInfo = $"Total: {totalRows} row{(totalRows == 1 ? "" : "s")}";

            int totalWidth = _columnWidths.Sum() + (columnsCount - 1) + 2;
            totalWidth = Math.Max(totalWidth, pageInfo.Length);

            Span<char> counterBuffer = _charBuffer.AsSpan(0, totalWidth);
            counterBuffer.Fill(' ');


            ReadOnlySpan<char> pageInfoSpan = pageInfo.AsSpan();
            pageInfoSpan.CopyTo(counterBuffer[(totalWidth - pageInfoSpan.Length)..]);

            _stringBuilder.Append(counterBuffer[..totalWidth]);
        }
    }

    public string GetResult()
    {
        if (_charBuffer != null)
        {
            ArrayPool<char>.Shared.Return(_charBuffer);
            _charBuffer = null;
        }

        string result = _stringBuilder.ToString();
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendRepeated(StringBuilder sb, char c, int count)
    {
        // For small counts, just use a loop
        if (count <= 8)
        {
            for (int i = 0; i < count; i++)
            {
                sb.Append(c);
            }

            return;
        }

        // For larger counts, use string constructor
        sb.Append(c, count);
    }

    // Return the buffer to the pool when object is disposed
    ~ConsoleTableBuilder()
    {
        if (_charBuffer != null)
        {
            ArrayPool<char>.Shared.Return(_charBuffer);
            _charBuffer = null;
        }
    }
}