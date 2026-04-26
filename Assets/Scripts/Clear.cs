using UnityEngine;

public class Clear : MonoBehaviour
{
    public void ClearData()
    {
        Debug.Log("Clearing all player data...");
        PlayerPrefs.DeleteAll();
    }
}
