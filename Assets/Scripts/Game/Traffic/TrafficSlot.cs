using UnityEngine;

[System.Serializable]
public class TrafficSlot : MonoBehaviour
{
    public Transform point;
    public Car car; // кто стоит в этом слоте
}