using UnityEngine;

public class LevelsCountText : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI levelsCountText;

    private void Start()
    {
        int currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        levelsCountText.text = $"Level: {currentLevel}";
    }
}
