using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image frontImage; // symbol
    public Image backImage; // card back
    public TextMeshProUGUI symbolText;

    [Header("Animation Settings")]
    public float shakeIntensity = 10f;
    public float shakeDuration = 0.3f;
    
    private bool _isFlipping = false;
    private bool _isRevealed = false;
    
    public void SetupCard(string symbol)
    {
        symbolText.text = symbol;
        HideCard();
    }

    public void OnCardClicked()
    {
        if (_isRevealed || _isFlipping || GameManager.Instance.IsCheckingMatch()) return;
        
        var flipSound = GameManager.Instance.GetCardFlipSound();
        if (flipSound != null)
        {
            GameManager.Instance.audioManager.PlaySound(flipSound);
        }
        
        StartCoroutine(FlipCard(true));
        GameManager.Instance.CardSelected(this);
    }

    private void RevealCard()
    {
        _isRevealed = true;
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
    }

    private void HideCard()
    {
        _isRevealed = false;
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
    }
    
    public void ShakeAndHide()
    {
        if (!_isRevealed || _isFlipping) return;
        StartCoroutine(ShakeAndHideCoroutine());
    }
    public string GetSymbol() => symbolText.text;
    private IEnumerator ShakeAndHideCoroutine()
    {
        _isFlipping = true;
        
        var currentPosition = transform.localPosition;
        // Shake animation
        var elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            var offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            var offsetY = Random.Range(-shakeIntensity, shakeIntensity);
            
            transform.localPosition = currentPosition + new Vector3(offsetX, offsetY, 0);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        
        transform.localPosition = currentPosition;
        yield return StartCoroutine(FlipCard(false));
    }
    
    private IEnumerator FlipCard(bool reveal)
    {
        _isFlipping = true;
        
        var time = 0f;
        var duration = 0.2f;
        var startRot = transform.rotation;
        var midRot = Quaternion.Euler(0f, 90f, 0f);

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRot, midRot, time / duration);
            yield return null;
        }

        // Swapping front/back at halfway point
        if (reveal)
            RevealCard();
        else
            HideCard();

        // Rotating back to 0 degrees
        time = 0f;
        var endRot = Quaternion.Euler(0f, 0f, 0f);
        while (time < duration)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(midRot, endRot, time / duration);
            yield return null;
        }

        transform.rotation = endRot; // ensure final rotation exactly 0

        _isFlipping = false;
    }
}
