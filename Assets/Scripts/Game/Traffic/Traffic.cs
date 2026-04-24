using System.Collections.Generic;
using UnityEngine;

public class Traffic : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject carPrefab;

    public List<TrafficSlot> trafficSlots = new();
    public List<TrafficSlot> parkingSlots = new();
    public List<Material> materials = new();

    [SerializeField] private LineRenderer parkingLine; 
    [SerializeField] private List<CarData> carDatas;
    private int carDataIndex = 0;

    private Queue<Car> queue = new();
    
    private Vector3[] pathPoints;

    public Material GetMaterial(CarData.CarColor color)
    {
        if(color == CarData.CarColor.Purple)
            return materials[0];
        else if (color == CarData.CarColor.Green)
            return materials[1];
        else if (color == CarData.CarColor.Blue)
            return materials[2];
        else if (color == CarData.CarColor.Yellow)
            return materials[3];
        else if (color == CarData.CarColor.Orange)
            return materials[4];
        else
            return null;
    }

    private void Start()
    {
        parkingLine.GetPositions(pathPoints = new Vector3[parkingLine.positionCount]);
        for (int i = 0; i < carDatas.Count; i++ )
        {
            SpawnCar();
        }
        InvokeRepeating(nameof(Tick), 0f, 0.5f);
    }
    public void SpawnCar()
    {
        var car = Instantiate(carPrefab, spawnPoint.position, Quaternion.identity)
                  .GetComponent<Car>();
        car.SetCapacityText(carDatas[carDataIndex].capacity);
        car.SetColor(GetMaterial(carDatas[carDataIndex].carColor), carDatas[carDataIndex].carColor);
        queue.Enqueue(car);
        carDataIndex++;
    }
    void Tick()
    {
        TrySendToParking();
        FillTrafficSlots();
    }
    void TrySendToParking()
    {
        var firstSlot = trafficSlots[0];

        if (firstSlot.car == null)
            return;

        foreach (var parking in parkingSlots)
        {
            if (parking.car == null)
            {
                var car = firstSlot.car;

                parking.car = car;

                car.MoveAlongPath(parking.point.position, pathPoints);

                firstSlot.car = null;

                ShiftTraffic();

                break;
            }
        }
    }
    void ShiftTraffic()
    {
        for (int i = 1; i < trafficSlots.Count; i++)
        {
            if (trafficSlots[i - 1].car == null && trafficSlots[i].car != null)
            {
                trafficSlots[i - 1].car = trafficSlots[i].car;

                trafficSlots[i - 1].car.MoveTo(trafficSlots[i - 1].point.position);

                trafficSlots[i].car = null;
            }
        }
    }
    void FillTrafficSlots()
    {
        for (int i = 0; i < trafficSlots.Count; i++)
        {
            if (trafficSlots[i].car == null && queue.Count > 0)
            {
                var car = queue.Dequeue();
                trafficSlots[i].car = car;
                car.MoveTo(trafficSlots[i].point.position);
            }
        }
    }
}