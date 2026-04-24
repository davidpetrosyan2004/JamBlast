using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class SmallActiveSquare : MonoBehaviour
{
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private TrailRenderer trail;
    //[SerializeField] private GameObject parkings;
    public string squareColor { get; set; }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetColor(string color)
    {
        squareColor = color;
        if (color == "Purple")
        {
            trail.startColor = new Color(0.5f, 0f, 0.5f);
            trail.endColor = new Color(0.5f, 0f, 0.5f);
        }
        else if (color == "Green")
        {
            trail.startColor = Color.green;
            trail.endColor = Color.green;
        }
        else if (color == "Blue")
        {
            trail.startColor = Color.blue;
            trail.endColor = Color.blue;
        }
        else if (color == "Yellow")
        {
            trail.startColor = Color.yellow;
            trail.endColor = Color.yellow;
        }
        else if (color == "Orange")
        {
            trail.startColor = new Color(1f, 0.65f, 0f);
            trail.endColor = new Color(1f, 0.65f, 0f);
        }
        //GetCarInParkingsTheSameColor();
    }

    //public void GetCarInParkingsTheSameColor()
    //{
    //    foreach (Transform parking in parkings.transform)
    //    {
    //        var car = parking.GetComponentInChildren<TrafficSlot>().car;
    //        if (car != null && car.carColor == squareColor)
    //        {
    //            transform.DOMove(car.transform.position, 0.5f).SetEase(Ease.OutQuad);
    //        }
    //    }
    //}
}