using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject bufferFullMessage;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject NextButton;

    [SerializeField] private Image niceOneMessage; 
    [SerializeField] private Image youRockMessage; 
    private Coroutine messageCoroutine;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        gameOverCanvas.SetActive(false);
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
        GameEvents.ShowMessage += ShowMessage;
        GameEvents.ComboLinesCompleted += ShowComboLinesCompleted;
    }
    private void OnDisable()
    { 
        GameEvents.GameOver -= OpenGameOverPanel;
        GameEvents.GameWin -= OpenGameWinPanel;
        GameEvents.ShowMessage -= ShowMessage;
        GameEvents.ComboLinesCompleted -= ShowComboLinesCompleted;
    }

    public void OpenGameOverPanel()
    {
        AudioManager.Instance.PlaySound("GameLose");
        gameOverCanvas.SetActive(true);
        gameOverPanel.SetActive(true);
    }
    public void OpenGameWinPanel()
    {
        AudioManager.Instance.PlaySound("GameWin");
        gameOverCanvas.SetActive(true);
        int currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if(currentLevel == 10)
        {
            NextButton.SetActive(false);
        }
        int savedLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        if (currentLevel >= savedLevel)
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            PlayerPrefs.Save();
        }
        gameWinPanel.SetActive(true);
    }

    public void ShowMessage()
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        messageCoroutine = StartCoroutine(ShowBufferFullMessage());
    }
    public IEnumerator ShowBufferFullMessage()
    {
        bufferFullMessage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        bufferFullMessage.SetActive(false);
    }

    public void ShowComboLinesCompleted()
    {
        youRockMessage.transform.localScale = Vector3.zero;

        youRockMessage.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();

        seq.Append(youRockMessage.DOFade(1, 0.2f));
        seq.Join(youRockMessage.transform.DOScale(0.01f, 0.3f).SetEase(Ease.OutBack));

        seq.AppendInterval(0.5f);

        seq.Append(youRockMessage.DOFade(0, 0.3f));
        seq.Join(youRockMessage.transform.DOScale(0.0049f, 0.3f));
    }
}