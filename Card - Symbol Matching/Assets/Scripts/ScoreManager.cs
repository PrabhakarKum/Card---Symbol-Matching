using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI attemptsText;
    
    [HideInInspector] public int maxScore = 1000;
    [HideInInspector] public int attemptPenalty = 10;
    [HideInInspector] public int timePenalty = 2;

    private int _attempts = 0;
    private int _matches = 0;

    public void IncrementAttempts()
    {
        _attempts++;
        UpdateAttemptsUI();
    }

    public void IncrementMatches()
    {
        _matches++;
    }

    public int CalculateFinalScore(float elapsedTime)
    {
        return Mathf.Max(0, maxScore - (_attempts * attemptPenalty + Mathf.FloorToInt(elapsedTime) * timePenalty));
    }
    private void UpdateAttemptsUI()
    {
        attemptsText.text = "Attempts: " + _attempts;
    }

    public void ResetScore()
    {
        _attempts = 0;
        _matches = 0;
        UpdateAttemptsUI();
        scoreText.text = "";
    }

    public int GetMatches() => _matches;
    public int GetAttempts() => _attempts;
}
