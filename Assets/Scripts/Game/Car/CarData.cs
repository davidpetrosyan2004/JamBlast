using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/CarData", order = 1)]
public class CarData : ScriptableObject
{
    public int capacity;
    public CarColor carColor;
    public enum CarColor
    {
        Purple,
        Green,
        Blue,
        Yellow,
        Orange
    }
}
