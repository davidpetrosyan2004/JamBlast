using UnityEngine;
using System.Collections.Generic;
using System;

public class Buffer : MonoBehaviour
{
    [SerializeField] private ShapeStorage _shapeStorage;
    [SerializeField] private Transform startPos;
    [SerializeField] private int capacity;
    public List<Shape> shapes = new();
    public List<BufferSlot> slots;

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
        if(collision.gameObject !=  null) 
            collision.gameObject.GetComponentInParent<Shape>().surfaceName = ".name";
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
}
