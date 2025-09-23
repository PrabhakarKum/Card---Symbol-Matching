using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Scene Transition Settings")]
    public float startGameDelay = 0.3f;
    public string gameSceneName = "GameScene";

    public void OnPlayButtonPressed() => Invoke(nameof(LoadGameScene), startGameDelay);
    public void OnExitButtonPressed()
    {
        Application.Quit();
        
        // For testing in editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    private void LoadGameScene() => SceneManager.LoadScene(gameSceneName);
}
