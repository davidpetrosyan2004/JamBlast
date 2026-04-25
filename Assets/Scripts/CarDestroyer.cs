using UnityEngine;

public class CarDestroyer : MonoBehaviour
{
    [SerializeField] private GameObject cars;
    [SerializeField] private int carsCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Destroy(other.gameObject);
            carsCount--;
            if (carsCount <= 0)
            {
                Debug.Log("You win!");
                GameEvents.GameWin();
            }
        }
    }
}
