using System.Collections;
using UnityEngine;

internal class SlotVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    Coroutine feeedbackCoroutine;
    Vector3 originalScale;
    Color32 originalColor;

    public void Initialize(Color32 color)
    {
        spriteRenderer.color = color;
        originalScale = transform.localScale;
        originalColor = color;
    }

    public void ActivationFeedback()
    {
        if (feeedbackCoroutine != null)
        {
            StopCoroutine(feeedbackCoroutine);
            spriteRenderer.color = originalColor;
            transform.localScale = originalScale;
        }
        feeedbackCoroutine = StartCoroutine(FeedbackRoutine());
    }

    IEnumerator FeedbackRoutine()
    {
        Color32 originalColor = spriteRenderer.color;
        Color32 highlightColor = new Color32(255, 255, 255, 255);
        float duration = 0.2f;
        float elapsed = 0f;

        Vector3 originalScale = transform.localScale;
        Vector3 highlightScale = originalScale * 1.2f;

        // Lerp color and scale up
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            spriteRenderer.color = Color32.Lerp(originalColor, highlightColor, t);
            transform.localScale = Vector3.Lerp(originalScale, highlightScale, t);
            yield return null;
        }

        elapsed = 0f;
        // Lerp color and scale back down
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            spriteRenderer.color = Color32.Lerp(highlightColor, originalColor, t);
            transform.localScale = Vector3.Lerp(highlightScale, originalScale, t);
            yield return null;
        }

        spriteRenderer.color = originalColor;
        transform.localScale = originalScale;
    }
}