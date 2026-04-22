using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[] column;

        public Row(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            column = new bool[size];
            ClearRow();
        }

        public void ClearRow()
        {
            if (column == null) return;

            for (int i = 0; i < column.Length; i++)
            {
                column[i] = false;
            }
        }
    }

    public int columns;
    public int rows;
    public Row[] board;

    public void Clear()
    {
        if (board == null) return;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != null)
            {
                board[i].ClearRow();
            }
        }
    }

    public void CreateNewBoard()
    {
        if (columns <= 0 || rows <= 0) return;

        board = new Row[rows];

        for (int i = 0; i < rows; i++)
        {
            board[i] = new Row(columns);
        }
    }
}