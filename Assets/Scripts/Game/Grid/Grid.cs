using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class Grid : MonoBehaviour
{
    public int columns;
    public int rows;

    private LineIndicator _lineIndicator;
    [SerializeField] private ShapeStorage _shapeStorage;
    [SerializeField] private Buffer _buffer;
    [SerializeField] private GameObject gridSquare;

    [SerializeField] private float squareGap = 0.1f;
    [SerializeField] private Vector2 startPosition = new();
    [SerializeField] private float squareScale = 0.5f;

    [SerializeField] private CameraShake cameraShaker;

    private Vector2 offset = new();
    private List<GameObject> gridSquares = new();

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        GameEvents.CheckIfShapeCanBePlacedInBuffer += CheckIfShapeCanBePlacedInBuffer;

    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        GameEvents.CheckIfShapeCanBePlacedInBuffer -= CheckIfShapeCanBePlacedInBuffer;

    }

    void Start()
    {
        _lineIndicator = new LineIndicator(rows, columns);
        Application.targetFrameRate = 60;
        CreateGrid();
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPositions();
    }

    private void SpawnGridSquares()
    {
        int square_index = 0;
        for(int row = 0; row < rows;  row++)
        {
            for(int col = 0; col < columns; col++)
            {
                var currentSquare = Instantiate(gridSquare);
                currentSquare.transform.SetParent(transform);
                currentSquare.transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                currentSquare.GetComponent<GridSquare>().SetImage(false);
                currentSquare.GetComponent<GridSquare>().SquareIndex = square_index;
                gridSquares.Add(currentSquare);
                square_index++;
            }
        }
    }

    private void SetGridSquaresPositions()
    {
        int column_number = 0;
        int row_number = 0;

        var square_rect = gridSquares[0].GetComponent<RectTransform>();

        offset.x = square_rect.rect.width * square_rect.transform.localScale.x;
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y;

        foreach (var square in gridSquares)
        {
            if (column_number >= columns)
            {
                column_number = 0;
                row_number++;
            }

            float posX = column_number * (offset.x + squareGap);
            float posY = row_number * (offset.y + squareGap);

            square.GetComponent<RectTransform>().localPosition =
                new Vector3(startPosition.x + posX, startPosition.y - posY);

            column_number++;
        }
    }
    public void CheckIfLinesCompleted(List<int[]> lines)
    {
        List<int[]> completedLines = new();
        foreach(var line in lines)
        {
            bool IsLineCompleted = true;
            foreach(var squareIndex in line)
            {
                if (!gridSquares[squareIndex].GetComponent<GridSquare>().SquareOccupied)
                {
                    IsLineCompleted = false; 
                    break;
                }
            }
            if (IsLineCompleted)
            { 
                completedLines.Add(line);
            }
        }
        if(completedLines.Count == 1)
        {
            cameraShaker.Shake();
            AudioManager.Instance.PlaySound("OneLineCompleted");
        }
        else if(completedLines.Count > 1) 
        {
            cameraShaker.Shake();
            GameEvents.ComboLinesCompleted();
            AudioManager.Instance.PlaySound("TwoLinesCompleted");
        }
        else if(completedLines.Count < 1)
        {
            AudioManager.Instance.PlaySound("ShapePlaceInBoard");
        }
        foreach (var line in completedLines)
        {
            foreach(var squareIndex in line)
            {
                gridSquares[squareIndex].GetComponent<GridSquare>().DeactivateSquare();
            }
        }
        CheckIfPlayerLost();
    }
    private void CheckIfShapeCanBePlaced()
    {
        List<Shape> allShapes= new List<Shape>();
        foreach(var shape in _shapeStorage.shapes)
        {
            allShapes.Add(shape);
        }
        foreach(var shape in _buffer.shapes)
        {
            allShapes.Add(shape);
        }

        List<int> squareIndexes = new();
        foreach (var square in gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if (gridSquare.Selected && !gridSquare.SquareOccupied)
            {
                squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
            }
        }
        var currentSelectedShape = GetCurrentSelectedShape(allShapes);

        if (currentSelectedShape == null) return;
        if (currentSelectedShape.TotalSquareShapeCount == squareIndexes.Count)
        {
            foreach (var index in squareIndexes)
            {
                var gridSquareScript = gridSquares[index].GetComponent<GridSquare>();
                gridSquareScript.SetActivateImage(currentSelectedShape.shapeSprite);
                gridSquareScript.ActivateSquare();
            }
            if (currentSelectedShape.IsInBuffer)
            {
                _buffer.RemoveShape(currentSelectedShape);
                _buffer.FreeSlotWithGivenShape(currentSelectedShape);
            }
            else
                _shapeStorage.shapes.Remove(currentSelectedShape);
            Destroy(currentSelectedShape.gameObject);
            _shapeStorage.CheckIsThereAnyShapesInStorage();
            CheckIfLinesCompleted(_lineIndicator.lines);
        }
        else
        {
            currentSelectedShape.MoveShapeToStartPosition();
        }

    }
    public Shape GetCurrentSelectedShape(List<Shape> shapes)
    {
        foreach(var shape in shapes)
        {
            if (!shape.IsOnStartPosition())
            {
                return shape;
            }
        }
        return null;
    }
    public void CheckIfPlayerLost()
    {
        List<Shape> allShapes = new List<Shape>();
        foreach (var shape in _shapeStorage.shapes)
        {
            allShapes.Add(shape);
        }

        foreach (var shapeBuffer in _buffer.shapes)
        {
            allShapes.Add(shapeBuffer);
        }
        foreach(var sh in allShapes)
        {
            if (CanPlaceAnyWhere(sh))
                return;
        }
        if (_buffer.HasFreeSlot())
        {
            Debug.Log("No moves, but buffer can still save you");
            return;
        }
        Debug.Log("Game Over");
        GameEvents.GameOver(); 
    }
    private bool CanPlaceAtPosition(Shape shape, int startRow, int startCol)
    {
        var data = shape.CurrentShapeData;

        for (int r = 0; r < data.rows; r++)
        {
            for (int c = 0; c < data.columns; c++)
            {
                if (!data.board[r].column[c])
                    continue;

                int index = _lineIndicator.squareIndexes[startRow + r, startCol + c];

                var comp = gridSquares[index].GetComponent<GridSquare>();

                if (comp.SquareOccupied)
                    return false;
            }
        }

        return true;
    }
    public bool CanPlaceAnyWhere(Shape shape)
    {
        var data = shape.CurrentShapeData;

        int gridWidth = columns;   // или твой размер
        int gridHeight = rows;

        for (int startRow = 0; startRow <= gridHeight - data.rows; startRow++)
        {
            for (int startCol = 0; startCol <= gridWidth - data.columns; startCol++)
            {
                if (CanPlaceAtPosition(shape, startRow, startCol))
                    return true;
            }
        }

        return false;
    }
    public void CheckIfShapeCanBePlacedInBuffer()
    {
        var shape = _shapeStorage.GetCurrentSelectedShape();
        var slot = _buffer.GetNotOccupiedSlot();
        if (!shape.IsInBuffer && slot != -1)
        {
            _shapeStorage.shapes.Remove(shape);
            _shapeStorage.CheckIsThereAnyShapesInStorage();

            shape.transform.SetParent(_buffer.transform, false);

            shape.transform
                .DOLocalMove(_buffer.slots[slot].pos, 0.2f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    shape._startPosition = shape._transform.localPosition;
                });

            _buffer.slots[slot].occupiedShape = shape;
            shape.IsInBuffer = true;
            _buffer.AddShape(shape);
            CheckIfPlayerLost();
        }
        else if(!shape.IsInBuffer && slot == -1)
        {
            GameEvents.ShowMessage();
            shape.MoveShapeToStartPosition();
        }
        else
        {
            shape.MoveShapeToStartPosition();
        }
    }
}
