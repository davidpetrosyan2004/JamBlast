using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShapeData))]
[CanEditMultipleObjects]
public class ShapeDataDrawer : Editor
{
    private ShapeData Data => (ShapeData)target;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        DrawSizeFields();
        EditorGUILayout.Space();

        DrawButtons();
        EditorGUILayout.Space();

        if (IsBoardValid())
        {
            DrawBoard();
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(Data);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSizeFields()
    {
        int newColumns = EditorGUILayout.IntField("Columns", Data.columns);
        int newRows = EditorGUILayout.IntField("Rows", Data.rows);

        if ((newColumns != Data.columns || newRows != Data.rows) && newColumns > 0 && newRows > 0)
        {
            Data.columns = newColumns;
            Data.rows = newRows;
            Data.CreateNewBoard();
        }
    }

    private void DrawButtons()
    {
        if (GUILayout.Button("Clear Board"))
        {
            Data.Clear();
        }
    }

    private bool IsBoardValid()
    {
        return Data.board != null &&
               Data.board.Length == Data.rows &&
               Data.columns > 0 &&
               Data.rows > 0;
    }

    private void DrawBoard()
    {
        for (int row = 0; row < Data.rows; row++)
        {
            if (Data.board[row] == null || Data.board[row].column == null)
                continue;

            EditorGUILayout.BeginHorizontal();

            for (int col = 0; col < Data.columns; col++)
            {
                bool value = Data.board[row].column[col];

                bool newValue = GUILayout.Toggle(value, "", GUILayout.Width(25), GUILayout.Height(25));

                if (value != newValue)
                {
                    Data.board[row].column[col] = newValue;
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}