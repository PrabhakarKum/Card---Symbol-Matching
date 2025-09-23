using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Card Configuration")]
    public CardData gameConfig;
    
    [Header("Grid")]
    public Transform gridParent;

    [Header("Managers")]
    public ScoreManager scoreManager;
    public TimeManager timeManager;
    public MatchChecker matchChecker;
    public GridManager gridManager;
    public UIManager uiManager;
    public AudioManager audioManager;

    void Awake()
    {
        Instance = this;
        SetupManagersWithConfig();
        SetupEvents();
    }

    private void Start()
    {
        StartNewGame();
    }

    private void SetupManagersWithConfig()
    {
        if (gameConfig == null)
        {
            Debug.Log("Game Config is not assigned!");
            return;
        }
        
        scoreManager.maxScore = gameConfig.maxScore;
        scoreManager.attemptPenalty = gameConfig.attemptPenalty;
        scoreManager.timePenalty = gameConfig.timePenalty;
        
        matchChecker.matchCheckDelay = gameConfig.matchCheckDelay;
    }
    
    private void SetupEvents()
    {
        matchChecker.onMatchFound.AddListener(OnMatchFound);
        matchChecker.onMatchNotFound.AddListener(OnMatchNotFound);
        matchChecker.onMatchCheckComplete.AddListener(OnMatchCheckComplete);
    }
    private void StartNewGame()
    {
        SetupBoard();
        scoreManager.ResetScore();
        timeManager.ResetTimer();
        timeManager.StartTimer();
        matchChecker.ResetSelection();
    }
    public void RestartGame()
    {
        StartNewGame();
    }
    private void SetupBoard()
    {
        if (gameConfig == null || gameConfig.cardPrefab == null)
        {
            Debug.Log("Game Config or Card Prefab is missing!");
            return;
        }
        
        gridManager.SetupGridConstraint(gameConfig);
        
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        var deck = new List<string>();
        foreach (var s in gameConfig.symbols) 
        { 
            deck.Add(s); 
            deck.Add(s); 
        }

        // Shuffle deck
        for (var i = 0; i < deck.Count; i++)
        {
            var temp = deck[i];
            var rand = Random.Range(i, deck.Count);
            deck[i] = deck[rand];
            deck[rand] = temp;
        }

        foreach (var t in deck)
        {
            var cardGo = Instantiate(gameConfig.cardPrefab, gridParent);
            var card = cardGo.GetComponent<Card>();
            card.SetupCard(t);
        }
    }

    public void CardSelected(Card selectedCard)
    {
        matchChecker.TrySelectCard(selectedCard);
    }

    private void OnMatchFound()
    {
        scoreManager.IncrementMatches();
        audioManager.PlaySound(gameConfig.matchFoundSound);
    }

    private void OnMatchNotFound()
    {
        audioManager.PlaySound(gameConfig.matchNotFoundSound);
    }

    private void OnMatchCheckComplete()
    {
        scoreManager.IncrementAttempts();

        if (scoreManager.GetMatches() == gameConfig.symbols.Count)
        {
           Invoke(nameof(EndGame), 0.7f);
        }
    }

    private void EndGame()
    {
        timeManager.StopTimer();
        var finalScore = scoreManager.CalculateFinalScore(timeManager.GetElapsedTime());
        var attempts = scoreManager.GetAttempts();
        var elapsedTime = timeManager.GetElapsedTime();
        
        uiManager.ShowGameOverPanel(finalScore, attempts, elapsedTime);
    }

    public bool IsCheckingMatch() => matchChecker.IsCheckingMatch;
    
    public AudioClip GetCardFlipSound() => gameConfig?.cardFlipSound;
}
