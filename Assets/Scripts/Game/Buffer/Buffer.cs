using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Buffer : MonoBehaviour
{
    [SerializeField] private ShapeStorage _shapeStorage;
    [SerializeField] private Transform startPos;
    [SerializeField] private int capacity;
    public List<Shape> shapes = new();
    public List<BufferSlot> slots;

    [SerializeField] private TextMeshProUGUI bufferText;
    [SerializeField] private Transform bufferSpace;

    private void Start()
    {
        bufferText.text = "Free: " + (capacity - shapes.Count).ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponentInParent<Shape>().surfaceName = transform.name;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.gameObject.GetComponentInParent<Shape>().surfaceName = transform.name;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            var shape = collision.gameObject.GetComponentInParent<Shape>();
            if (shape != null)
            {
                shape.surfaceName = "Grid";
            }
        }
    }



    public int GetNotOccupiedSlot()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].occupiedShape == null)
            {
                return i;
            }
         }
        return -1;
    }
    public Shape GetCurrentSelectedShape()
    {
        foreach (var shape in shapes)
        {
            if (!shape.IsOnStartPosition())
            {
                return shape;
            }
        }
        return null;
    }

    public void FreeSlotWithGivenShape(Shape shape) 
    {
        foreach(var slot  in slots)
        {
            if(slot.occupiedShape == shape)
                slot.occupiedShape = null;
        }
    }

    public bool HasFreeSlot()
    {
        foreach (var slot in slots)
        {
            if (slot.occupiedShape == null)
                return true;
        }
        return false;
    }
    public void RemoveShape(Shape shape)
    {
        int previousCapacity = capacity - shapes.Count;
        shapes.Remove(shape);
        BufferCapacityChangedAnimation(previousCapacity, 1, bufferText);
    }
    public void AddShape(Shape shape)
    {
        int previousCapacity = capacity - shapes.Count;
        shapes.Add(shape);
        AudioManager.Instance.PlaySound("BufferAdd");
        BufferCapacityChangedAnimation(previousCapacity, -1, bufferText);
    }

    public void BufferCapacityChangedAnimation(int startValue, int amount, TextMeshProUGUI scoreText)
    {
        int endValue = startValue+amount;
        scoreText.DOKill();
        scoreText.transform.DOKill();
        Sequence seq = DOTween.Sequence();
        seq.Join(DOTween.To(() => startValue, x =>
        {
            startValue = x;
            scoreText.text = "Free: " + x.ToString();
        }, endValue, 0.3f));

        seq.Join(scoreText.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 10, 1));
    }
}
