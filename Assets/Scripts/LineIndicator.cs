using System.Collections.Generic;

public class LineIndicator
{
    private int rows;
    private int columns;
    public int[,] squareIndexes;
    public List<int[]> lines { get; private set; } = new();

    public LineIndicator(int rows, int columns)
    {
        this.rows = rows;
        this.columns = columns;
        squareIndexes = new int[rows, columns];
        SetSquareIndexes();
        GetAllColumnsAndRows();
    }
    private void SetSquareIndexes()
    {
        int indexer = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                squareIndexes[row, col] = indexer;
                indexer++;
            }
        }
    }
    private int[] GetVerticalLine(int col)
    {
        int[] verticalLine = new int[rows];
        for(var row = 0; row < rows; row++)
        {
            verticalLine[row] = squareIndexes[row, col];
        }
        return verticalLine;
    }
    private int[] GetHorizontalLine(int row)
    {
        int[] horizontalLine = new int[columns];
        for (var col = 0; col < columns; col++)
        {
            horizontalLine[col] = squareIndexes[row, col];
        }
        return horizontalLine;
    }

    private void GetAllColumnsAndRows()
    {
        for (var row = 0; row < rows; row++)
        {
            lines.Add(GetVerticalLine(row));
        }
        for (var col = 0; col < columns; col++)
        {
            lines.Add(GetHorizontalLine(col));
        }
    }
}
