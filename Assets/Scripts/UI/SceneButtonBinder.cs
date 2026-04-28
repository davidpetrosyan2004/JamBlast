using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButtonBinder : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Button button;
    [SerializeField] private ButtonType buttonType;
    enum ButtonType
    {
        LoadSceneButton,
        StartButton,
    }
    private void Start()
    {
        if (buttonType == ButtonType.StartButton)
        {
            int levelIndex = PlayerPrefs.GetInt("CurrentLevel", 1);
            Debug.Log($"CurrentLevel: {levelIndex}");
            string levelSceneName = $"Level {levelIndex}";
            button.onClick.AddListener(() => SceneManager.LoadScene(levelSceneName));
        }
        else
        {
            button.onClick.AddListener(() => SceneManager.LoadScene(sceneName));
        }
    }


    public void PlaySound()
    {
        AudioManager.Instance.PlaySound("ButtonClick");
    }
}
