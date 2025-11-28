using UnityEngine;

internal class SlotVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Initialize(Color32 color)
    {
        spriteRenderer.color = color;
    }
}