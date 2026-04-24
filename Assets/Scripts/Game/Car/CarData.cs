using UnityEngine;

[CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/CarData", order = 1)]
public class CarData : ScriptableObject
{
    public int capacity;
    public CarColor carColor;
    public enum CarColor
    {
        Purple = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3,
        Orange = 4
    }
}
