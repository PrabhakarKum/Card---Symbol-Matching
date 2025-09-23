using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameplayPanel;
    public GameObject gameOverPanel;
    
    [Header("Game Over UI Elements")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalAttemptsText;
    public TextMeshProUGUI finalTimeText;

    private void Start()
    {
        ShowGameplayPanel();
    }

    private void ShowGameplayPanel()
    {
        gameplayPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOverPanel(int finalScore, int attempts, float elapsedTime)
    {
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        
        // Display final stats
        finalScoreText.text = "Final Score: " + finalScore;
        finalAttemptsText.text = "Attempts: " + attempts;
        finalTimeText.text = "Time Taken: " + Mathf.FloorToInt(elapsedTime) + "s";
    }

    public void OnRestartButtonPressed()
    {
        GameManager.Instance.RestartGame();
        ShowGameplayPanel();
    }

    public void OnMainMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
