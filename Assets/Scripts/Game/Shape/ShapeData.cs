using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[] column;
        private int size;

        public Row() { }
        public Row(int size)
        {
            CreateRow(size);
        }

        public void ClearRow()
        {
            for (int i = 0; i < size; i++)
            {
                column[i] = false;
            }
        }

        public void CreateRow(int size)
        {
            column = new bool[size];
            this.size = size;
            ClearRow();
        }
    }

    public int columns;
    public int rows;
    public Row[] board;

    public void Clear()
    {
        for(int i = 0; i < rows; i++)
        {
            board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        board = new Row[rows];
        for(int i = 0;i < rows; i++)
        {
            board[i] = new Row(columns);
        }
    }
}
