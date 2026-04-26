using UnityEngine;

public class WinLose : MonoBehaviour
{
    public void Win()
    {
        GameEvents.GameWin();
    }
    public void Lose()
    {
        GameEvents.GameOver();
    }
}
