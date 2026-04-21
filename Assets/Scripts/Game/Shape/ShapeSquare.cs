using UnityEngine;
using UnityEngine.UI;
public class ShapeSquare : MonoBehaviour
{
    public Image occupiedImage;
    public Image normalImage;

    void Start()
    {
        occupiedImage.gameObject.SetActive(false);
    }
    public void SetImage(Sprite sprite)
    {
        normalImage.sprite = sprite;
    }
    public void DeactivateSquare()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }
    public void ActivateSquare()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
    }

    public void SetOccupied()
    {
        occupiedImage.gameObject.SetActive(true);
    }
    public void UnSetOccupied()
    {
        occupiedImage.gameObject.SetActive(false);
    }
}
