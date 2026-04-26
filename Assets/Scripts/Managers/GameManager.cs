using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
    private void OnDisable()
    {
        GameEvents.GameOver -= OpenGameOverPanel;
        GameEvents.GameWin -= OpenGameWinPanel;
    }

    public void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
    public void OpenGameWinPanel()
    {
        int currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        int savedLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        if (currentLevel >= savedLevel)
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            PlayerPrefs.Save();
        }
        gameWinPanel.SetActive(true);
    }
}