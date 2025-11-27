using System.Collections;
using UnityEngine;

public class DoActionButton : MonoBehaviour, IClickable
{
    [HideInInspector] public Player player;

    Vector3 originalScale;
    Coroutine clickFeedbackRoutine;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnClick()
    {
        player.DoAction();

        if (clickFeedbackRoutine != null)
        {
            StopCoroutine(clickFeedbackRoutine);
            transform.localScale = originalScale;
        }
        clickFeedbackRoutine = StartCoroutine(ClickFeedback());
    }

    public void OnClickOutside() { }

    public void OnCursorDown() { }

    public void OnCursorEnter() { }

    public void OnCursorExit() { }

    public void OnCursorUp() { }

    IEnumerator ClickFeedback()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.1f;
        float duration = 0.1f;
        float elapsed = 0f;

        // Scale up
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;

        // Scale back down
        elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;
    }
}
