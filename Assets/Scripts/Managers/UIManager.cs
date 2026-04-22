using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;
    private void OnEnable()
    {
        GameEvents.GameOver += OpenGameOverPanel;
        GameEvents.GameWin += OpenGameWinPanel;
    }

    public void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
    public void OpenGameWinPanel()
    {
        gameWinPanel.SetActive(true);
    }


}
