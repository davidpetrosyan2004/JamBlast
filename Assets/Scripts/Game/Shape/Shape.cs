using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject squareShapeImage;
    public Vector3 shapeSelectedScale;
    public Vector2 offset;
    public int TotalSquareShapeCount { get; set; }
    public RectTransform _transform { get; set; }
    public Vector3 _startPosition;
    public bool IsInBuffer;
    public string surfaceName { get; set; }

    // [HideInInspector]
    public ShapeData CurrentShapeData;

    private List<GameObject> _currentShape = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private Canvas _canvas;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _shapeStartScale = _transform.localScale;
        _startPosition = _transform.localPosition;
    }

    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;
        TotalSquareShapeCount = GetNumberOfSquares(shapeData);
        while(_currentShape.Count <= TotalSquareShapeCount)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform));
        }
        foreach(var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }
        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x,
            squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;
        for(int row = 0; row < shapeData.rows; row++)
        {
            for(int col = 0; col < shapeData.columns; col++)
            {
                if (shapeData.board[row].column[col])
                {
                    _currentShape[currentIndexInList].SetActive(true);
                    _currentShape[currentIndexInList].GetComponent<RectTransform>().localPosition = 
                        new Vector2(GetXPositionForShapeSquare(shapeData, col, moveDistance),
                        GetYPositionForShapeSquare(shapeData, row, moveDistance));
                    currentIndexInList++;
                }
            }
        }
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0;
        if(shapeData.rows > 1)
        {
            if(shapeData.rows % 2 != 0)
            {
                var middleSquareIndex = (shapeData.rows - 1) / 2;
                var multiplier = (shapeData.rows - 1) / 2;
                if(row < middleSquareIndex)
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
                else if(row > middleSquareIndex)
                {
                    shiftOnY = moveDistance.y - 1;
                    shiftOnY *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.rows == 2) ? 1 : (shapeData.rows / 2);
                var middleSquareIndex1 = (shapeData.rows == 2) ? 0 : (shapeData.rows - 2);
                var multiplier = shapeData.rows / 2;

                if(row == middleSquareIndex1 || row == middleSquareIndex2)
                {
                    if(row == middleSquareIndex2) shiftOnY = (moveDistance.y / 2) * -1;
                    if (row == middleSquareIndex1) shiftOnY = (moveDistance.y / 2);
                }
                if(row < middleSquareIndex2 && row < middleSquareIndex1)
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
                else if (row > middleSquareIndex1 && row > middleSquareIndex2)
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
            }
        }
        return shiftOnY;
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float shiftOnX = 0;
        if(shapeData.columns > 1)
        {
            if(shapeData.columns % 2 != 0)
            {
                var middleSquareIndex = (shapeData.columns - 1) / 2;
                var multiplier = (shapeData.columns - 1) / 2;
                if(column < middleSquareIndex)
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if(column > middleSquareIndex)
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.columns == 2) ? 1 : (shapeData.columns / 2);
                var middleSquareIndex1 = (shapeData.columns == 2) ? 0 : (shapeData.columns - 2);
                var multiplier = shapeData.columns / 2;

                if(column == middleSquareIndex1 || column == middleSquareIndex2)
                {
                    if (column == middleSquareIndex2) shiftOnX = moveDistance.x / 2;
                    if(column == middleSquareIndex1) shiftOnX = (moveDistance.x / 2) * -1; 
                }
                if(column < middleSquareIndex1 && column < middleSquareIndex2)
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if(column > middleSquareIndex1 && column > middleSquareIndex2)
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
        }
        return shiftOnX;
    }

    private int GetNumberOfSquares(ShapeData shapeData)
    {
        int number = 0;
        foreach(var rowData in shapeData.board)
        {
            foreach(var colData in rowData.column)
            {
                if(colData) number++;
            }
        }
        return number;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _transform.localScale = shapeSelectedScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, 
            eventData.position, Camera.main, out pos);
        _transform.localPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _transform.localScale = _shapeStartScale;
        if(surfaceName != "Buffer" || IsInBuffer)
            GameEvents.CheckIfShapeCanBePlaced();
        else
            GameEvents.CheckIfShapeCanBePlacedInBuffer();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPosition;
    }

    public void MoveShapeToStartPosition()
    {
        _transform.localPosition = _startPosition;
    }
}
