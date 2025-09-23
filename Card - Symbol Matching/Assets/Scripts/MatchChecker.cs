using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MatchChecker : MonoBehaviour
{
    [Header("Match Settings")]
    public float matchCheckDelay = 0.5f;

    public UnityEvent onMatchFound;
    public UnityEvent onMatchNotFound;
    public UnityEvent onMatchCheckComplete;

    private Card _firstCard, _secondCard;
    public bool IsCheckingMatch { get; private set; }

    public bool TrySelectCard(Card selectedCard)
    {
        if (IsCheckingMatch) return false;

        if (_firstCard != null)
        {
            if (_secondCard != null) return false;
            _secondCard = selectedCard;
            StartCoroutine(CheckMatch());
            return true;
        }

        _firstCard = selectedCard;
        return true;

    }

    private IEnumerator CheckMatch()
    {
        IsCheckingMatch = true;

        if (_firstCard.GetSymbol() == _secondCard.GetSymbol())
        {
            yield return new WaitForSeconds(0.2f);
            onMatchFound?.Invoke();
        }
        else
        {
            yield return new WaitForSeconds(matchCheckDelay);
            
            onMatchNotFound?.Invoke();
            _firstCard.ShakeAndHide();
            _secondCard.ShakeAndHide();
            
            // Wait for shake animation to complete
            yield return new WaitForSeconds(0.3f);
        }

        _firstCard = null;
        _secondCard = null;
        onMatchCheckComplete?.Invoke();

        IsCheckingMatch = false;
    }

    public void ResetSelection()
    {
        _firstCard = null;
        _secondCard = null;
        IsCheckingMatch = false;
    }
}
