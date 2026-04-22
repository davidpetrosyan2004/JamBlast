using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using DG.Tweening;
public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject squareShapeImage;
    public Sprite shapeSprite;
    public Vector3 shapeSelectedScale;
    public Vector2 offset;
    public int TotalSquareShapeCount { get; set; }
    public RectTransform _transform { get; set; }
    public Vector3 _startPosition;
    public bool IsInBuffer;
    public string surfaceName { get; set; }

    [HideInInspector]
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

    public void CreateShape(ShapeData shapeData, Sprite sprite)
    {
        CurrentShapeData = shapeData;
        TotalSquareShapeCount = GetNumberOfSquares(shapeData);
        shapeSprite = sprite;
        while (_currentShape.Count <= TotalSquareShapeCount)
        {
            var squarePrefab = Instantiate(squareShapeImage, transform);
            squarePrefab.GetComponent<ShapeSquare>().SetImage(sprite);
            _currentShape.Add(squarePrefab);
        }
        foreach (var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }
        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x,
            squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;

        float offsetX = (shapeData.columns - 1) / 2f;
        float offsetY = (shapeData.rows - 1) / 2f;

        for (int row = 0; row < shapeData.rows; row++)
        {
            for (int col = 0; col < shapeData.columns; col++)
            {
                if (shapeData.board[row].column[col])
                {
                    _currentShape[currentIndexInList].SetActive(true);

                    float posX = (col - offsetX) * moveDistance.x;
                    float posY = (offsetY - row) * moveDistance.y;

                    _currentShape[currentIndexInList]
                        .GetComponent<RectTransform>()
                        .localPosition = new Vector2(posX, posY);

                    currentIndexInList++;
                }
            }
        }
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
        AudioManager.Instance.PlaySound("ShapePickUp", true);
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
        _transform.DOLocalMove(_startPosition, 0.2f).SetEase(Ease.InOutQuad);
    }
}
