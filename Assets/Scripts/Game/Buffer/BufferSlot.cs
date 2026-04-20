using UnityEngine;

public class BufferSlot : MonoBehaviour
{
    public Vector3 pos;
    public Shape occupiedShape = null;

    private void Start()
    {
        pos = transform.localPosition;
    }
}
