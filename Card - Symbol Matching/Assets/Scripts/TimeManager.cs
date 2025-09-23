using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI timeText;

    private float _elapsedTime = 0f;
    private bool _timerRunning = false;

    private void Update()
    {
        if (!_timerRunning) return;
        _elapsedTime += Time.deltaTime;
        UpdateTimeUI();
    }

    public void StartTimer()
    {
        _elapsedTime = 0f;
        _timerRunning = true;
    }

    public void StopTimer() => _timerRunning = false;
    private void UpdateTimeUI() => timeText.text = "Time: " + Mathf.FloorToInt(_elapsedTime) + "s";
    public float GetElapsedTime() => _elapsedTime;

    public void ResetTimer()
    {
        _elapsedTime = 0f;
        _timerRunning = false;
        UpdateTimeUI();
    }
}
