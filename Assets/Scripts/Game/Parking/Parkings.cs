using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Parkings : MonoBehaviour
{
    public List<TrafficSlot> parkings = new();

    public TrafficSlot GetParkingInParkingsTheSameColor(CarData.CarColor squareColor)
    {
        foreach (var parking in parkings)
        {
            var car = parking.car;

            if (car != null && car.carColor == squareColor)
            {
                return parking;
            }
        }
        return null;
    }
}
