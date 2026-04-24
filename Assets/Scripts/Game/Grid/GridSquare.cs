using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering;
public class GridSquare : MonoBehaviour
{
    [SerializeField] private Image normalImage;
    [SerializeField] private Image hooverImage;
    [SerializeField] private Image activeImage;
    [SerializeField] private ParticleSystem gridEffect;
    [SerializeField] private GameObject activeSquare;
    private GameObject activeSquarePrefab;
    public bool Selected { get; set; }
    public bool SquareOccupied { get; set; }
    public int SquareIndex { get; set; }

    [SerializeField] private List<Sprite> normalImages;

    private void Start()
    {
        SquareOccupied = false;
        Selected = false;
    }

    public bool CanWeUseThisSquare()
    {
        return hooverImage.gameObject.activeSelf;
    }
    public void SetActivateImage(Sprite sprite)
    {
        activeImage.sprite = sprite;

        activeSquarePrefab = Instantiate(activeSquare, transform.position, Quaternion.identity, transform);
        activeSquarePrefab.SetActive(false);

        var currentActiveSquare = activeSquarePrefab.GetComponent<SmallActiveSquare>();
        currentActiveSquare.SetSprite(sprite);

        switch (sprite.name)
        {
            case "squares_1":
                currentActiveSquare.SetColor(CarData.CarColor.Green);
                break;
            case "squares_2":
                currentActiveSquare.SetColor(CarData.CarColor.Yellow);
                break;
            case "squares_3":
                currentActiveSquare.SetColor(CarData.CarColor.Blue);
                break;
            case "squares_4":
                currentActiveSquare.SetColor(CarData.CarColor.Orange);
                break;
            case "squares_5":
                currentActiveSquare.SetColor(CarData.CarColor.Purple);
                break;
        }
    }
    public void ActivateSquare()
    {
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        SquareOccupied = true;
    }
    public void DeactivateSquare()
    {
        gridEffect.Play();
        activeImage.gameObject.SetActive(false);
        Selected = false;
        SquareOccupied = false;
        activeSquarePrefab.SetActive(true);
        activeSquarePrefab.GetComponent<SmallActiveSquare>().MoveToCars();
    }

    public void SetImage(bool IsFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = IsFirstImage ? normalImages[0] : normalImages[1];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!SquareOccupied)
        {
            Selected = true;
            hooverImage.gameObject.SetActive(true);
        }
        else if(collision != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;
        if (!SquareOccupied)
        {
            hooverImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!SquareOccupied)
        {
            Selected = false;
            hooverImage.gameObject.SetActive(false);
        }
        else if (collision != null)
        {
            collision.GetComponent<ShapeSquare>().UnSetOccupied();
        }
    }
}
