using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Parkings : MonoBehaviour
{
    public List<TrafficSlot> parkings = new();

    public Car GetCarInParkingsTheSameColor(CarData.CarColor squareColor)
    {
        foreach (var parking in parkings)
        {
            var car = parking.car;

            if (car != null && car.carColor == squareColor)
            {
            Debug.Log($"Checking parking slot with car color: {car?.carColor} against square color: {squareColor}");
                return car;
            }
        }
        return null;
    }
}
