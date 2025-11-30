using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotVisual : MonoBehaviour, IClickable
{
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    Coroutine feeedbackCoroutine;
    Vector3 originalScale;
    Vector3 originalPosition;

    [SerializeField] private Transform neutralTransform;
    [SerializeField] private Transform activatedTransform;
    [SerializeField] private Transform deactivatedTransform;

    [Header("Sounds")]
    [SerializeField] private AudioClip dragSound;
    [SerializeField] private AudioClip dropSound;

    // Slot
    public Slot slot;

    public void Initialize(Slot slot)
    {
        this.slot = slot;
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
        InitializeVisual();
    }

    public void ActivationFeedback(ActionState startActionState, ActionState endActionState)
    {
        if (feeedbackCoroutine != null)
        {
            StopCoroutine(feeedbackCoroutine);
            transform.localScale = originalScale;
            transform.localPosition = originalPosition;
        }
        feeedbackCoroutine = StartCoroutine(FeedbackRoutine());
    }

    IEnumerator FeedbackRoutine()
    {
        float duration = 0.2f;
        float elapsed = 0f;

        Vector3 originalScale = transform.localScale;
        Vector3 highlightScale = originalScale * 1.2f;

        // Lerp scale up
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(originalScale, highlightScale, t);
            yield return null;
        }

        elapsed = 0f;
        // Lerp scale back down
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(highlightScale, originalScale, t);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    public void SetColor(Color32 color)
    {
        Debug.Log("Setting SlotVisual color to: " + color);
        spriteRenderers[0].color = color;
    }

    public void InitializeVisual()
    {
        neutralTransform.gameObject.SetActive(true);
        activatedTransform.gameObject.SetActive(false);
        deactivatedTransform.gameObject.SetActive(false);
    }

    public void UpdateVisual(ActionState startState, ActionState endState)
    {
        switch (startState)
        {
            case ActionState.Neutral:
                neutralTransform.gameObject.SetActive(false);
                break;
            case ActionState.Activated:
                activatedTransform.gameObject.SetActive(false);
                break;
            case ActionState.Deactivated:
                deactivatedTransform.gameObject.SetActive(false);
                break;
        }
        switch (endState)
        {
            case ActionState.Neutral:
                neutralTransform.gameObject.SetActive(true);
                break;
            case ActionState.Activated:
                activatedTransform.gameObject.SetActive(true);
                break;
            case ActionState.Deactivated:
                deactivatedTransform.gameObject.SetActive(true);
                break;
        }
    }

    public void NegativeFeedback()
    {
        if (feeedbackCoroutine != null)
        {
            StopCoroutine(feeedbackCoroutine);
            transform.localScale = originalScale;
            transform.localPosition = originalPosition;
        }
        feeedbackCoroutine = StartCoroutine(TrembleRoutine());

        IEnumerator TrembleRoutine()
        {
            float duration = 0.3f;
            float elapsed = 0f;
            float trembleMagnitude = 0.1f;
            int trembleCount = 8;
            Vector3 basePosition = transform.localPosition;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float angle = Mathf.Sin(t * trembleCount * Mathf.PI * 2) * trembleMagnitude;
                transform.localPosition = basePosition + new Vector3(angle, 0, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = basePosition;
        }
    }

    public void OnCursorDown()
    {
        if (!GameManager.Instance.inFight)
        {
            Debug.Log("SlotVisual Cursor Down while out of fight: " + gameObject.name);

            if (slot != null)
            {
                // Check où le joueur a cliqué pour connaitre sur quelle partie de la slot il a cliqué
                Vector3 worldClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 localClickPosition = transform.InverseTransformPoint(worldClickPosition);
                float halfWidth = (float)slot.size / 2;
                for (int i = 0; i < slot.size; i++)
                {
                    float segmentStart = -halfWidth + i;
                    float segmentEnd = segmentStart + 1f;
                    if (localClickPosition.x >= segmentStart && localClickPosition.x < segmentEnd)
                    {
                        Debug.Log("Clicked on segment " + i + " of slot " + name);
                        break;
                    }
                }
            }

            GameManager.Instance.SelectSlot(this);

            // Play drag sound
            if (dragSound != null)
            {
                var options = new SoundManager.SoundOptions { volume = 0.5f, pitch = 1.1f };
                SoundManager.Instance.PlaySound(options, dragSound);
            }
        }
    }

    public void OnCursorUp()
    {
        if (!GameManager.Instance.inFight)
        {
            if (GameManager.Instance.selectedSlotVisual == this)
            {
                // Drop the slot
                GameManager.Instance.SelectSlot(null);
                // Snap back to original position
                transform.localPosition = new Vector3(slot.placementIndex + slot.size / 2f - 0.5f, 0f, 0f);

                // Play drag sound
                if (dropSound != null)
                {
                    var options = new SoundManager.SoundOptions { volume = 0.5f, pitch = 0.9f };
                    SoundManager.Instance.PlaySound(options, dropSound);
                }
            }
        }
    }

    public void OnCursorEnter() { }

    public void OnCursorExit() { }

    public void OnClick() { }

    public void OnClickOutside() { }
}