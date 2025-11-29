using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotVisual : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    Coroutine feeedbackCoroutine;
    Vector3 originalScale;
    Color32 originalColor;

    public void Initialize()
    {
        originalScale = transform.localScale;
        originalColor = spriteRenderers[0].color;
    }

    public void ActivationFeedback()
    {
        if (feeedbackCoroutine != null)
        {
            StopCoroutine(feeedbackCoroutine);
            spriteRenderers[0].color = originalColor;
            transform.localScale = originalScale;
        }
        feeedbackCoroutine = StartCoroutine(FeedbackRoutine());
    }

    IEnumerator FeedbackRoutine()
    {
        Color32 originalColor = spriteRenderers[0].color;
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
            spriteRenderers[0].color = Color32.Lerp(originalColor, highlightColor, t);
            transform.localScale = Vector3.Lerp(originalScale, highlightScale, t);
            yield return null;
        }

        elapsed = 0f;
        // Lerp color and scale back down
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            spriteRenderers[0].color = Color32.Lerp(highlightColor, originalColor, t);
            transform.localScale = Vector3.Lerp(highlightScale, originalScale, t);
            yield return null;
        }

        spriteRenderers[0].color = originalColor;
        transform.localScale = originalScale;
    }

    public void SetColor(Color32 color)
    {
        Debug.Log("Setting SlotVisual color to: " + color);
        spriteRenderers[0].color = color;
    }

    internal void UpdateVisual(ActionState state)
    {
        switch (state)
        {
            case ActionState.Neutral:
                SetColor(originalColor);
                break;
            case ActionState.Activated:
                SetColor(new Color32(200, 200, 200, 255)); // Lighter color for activated
                break;
            case ActionState.Deactivated:
                SetColor(new Color32(100, 100, 100, 255)); // Darker color for deactivated
                break;
                // Add other states as needed
        }
    }
}