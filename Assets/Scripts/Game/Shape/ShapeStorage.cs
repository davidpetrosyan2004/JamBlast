using UnityEngine;
using System.Collections.Generic;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeDatas;


    [SerializeField] private int capacity;
    [SerializeField] private GameObject shapePrefab;
    [SerializeField] private Transform startPos;
    public List<Shape> shapes;
    private Vector2[] shapesPoses;

    void Start()
    {
        shapesPoses = new Vector2[capacity];
        for(int i = 0; i < capacity; i++)
        {
            shapesPoses[i] = startPos.position + new Vector3(i * 1.5f, 0,0);
        }
        SpawnNewShapes();
    }

    public Shape GetCurrentSelectedShape()
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

    public void SpawnNewShapes()
    {
        for(int i = 0; i < capacity; i++)
        {
            var shape = Instantiate(shapePrefab, shapesPoses[i], Quaternion.identity, transform).GetComponent<Shape>();
            var newShapeData = Random.Range(0, shapeDatas.Count);
            shape.CreateShape(shapeDatas[newShapeData]);
            shapes.Add(shape);
        }
    }

    public void CheckIsThereAnyShapesInStorage()
    {
        if (shapes.Count <= 0)
        {
            SpawnNewShapes();
        }
    }
}
