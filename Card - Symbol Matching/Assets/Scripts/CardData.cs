using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Card Game/Card Data")]
public class CardData : ScriptableObject
{
    [Header("Card Settings")]
    public GameObject cardPrefab;
    public List<string> symbols = new List<string>();

    [Header("Audio Settings")]
    public AudioClip cardFlipSound;
    public AudioClip matchFoundSound;
    public AudioClip matchNotFoundSound;

    [Header("Game Settings")]
    public int maxScore = 500;
    public int attemptPenalty = 10;
    public int timePenalty = 2;
    public float matchCheckDelay = 0.5f;
}
