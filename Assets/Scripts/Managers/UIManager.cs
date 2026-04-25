using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;

    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public void OnBackMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene("Game");
    }
}