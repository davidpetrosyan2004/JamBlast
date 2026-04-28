using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class SmallActiveSquare : MonoBehaviour
{
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private TrailRenderer trail;
    public CarData.CarColor squareColor { get; set; }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetColor(CarData.CarColor color)
    {
        squareColor = color;
        if (color == CarData.CarColor.Purple)
        {
            trail.startColor = new Color(0.5f, 0f, 0.5f);
            trail.endColor = new Color(0.5f, 0f, 0.5f);
        }
        else if (color == CarData.CarColor.Green)
        {
            trail.startColor = Color.green;
            trail.endColor = Color.green;
        }
        else if (color == CarData.CarColor.Blue)
        {
            trail.startColor = Color.blue;
            trail.endColor = Color.blue;
        }
        else if (color == CarData.CarColor.Yellow)
        {
            trail.startColor = Color.yellow;
            trail.endColor = Color.yellow;
        }
        else if (color == CarData.CarColor.Orange)
        {
            trail.startColor = new Color(1f, 0.65f, 0f);
            trail.endColor = new Color(1f, 0.65f, 0f);
        }
    }
    public void MoveToCars()
    {
        var parkings = FindAnyObjectByType<Parkings>();
        var parking = parkings.GetParkingInParkingsTheSameColor(squareColor);
        if (parking != null)
        {
            var car = parking.car;
            if (car.squaresCount < car.capacity)
            {
                transform.DOMove(car.transform.position, 0.5f)
                        .SetEase(Ease.InOutSine)
                        .OnComplete(() => { car.UpdateCapacityText(parking); Destroy(transform.gameObject);});
            }
            else
            {
                Destroy(transform.gameObject);
            }
        }
        else
        {
            Destroy(transform.gameObject);
        }
    }
}