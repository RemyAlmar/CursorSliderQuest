using System.Collections.Generic;
using UnityEngine;

public class CursorSliderVisual : MonoBehaviour
{
    [SerializeField] private SlotVisual slotVisualPrefab;
    [SerializeField] private Transform cursorVisualTransform;
    float xCursorOffset = -0.5f;
    List<SlotVisual> slotVisuals = new();
    
    public void Initialize(List<Slot> slots)
    {
        ClearList();
        for (int i = 0; i < slots.Count; i++)
        {
            Slot slot = slots[i];
            SlotVisual slotVisual = Instantiate(slotVisualPrefab, transform);
            slotVisual.transform.localPosition = new Vector3(i, 0f, 0f);
            Color32 targetColor = new Color32(100, 100, 100, 255);
            if (slot.action != null)
            {
                switch (slot.action)
                {
                    case Action_Slash:
                        targetColor = new Color32(255, 0, 0, 255);
                        break;
                    case Action_Void:
                        targetColor = new Color32(200, 200, 200, 255);
                        break;
                }
            }
            slotVisual.Initialize(targetColor);
            slotVisuals.Add(slotVisual);
        }
    }

    public void UpdateCursorPosition(float cursorValue)
    {
        cursorVisualTransform.localPosition = new Vector3(cursorValue + xCursorOffset, 0f, 0f);
    }

    private void ClearList()
    {
        foreach (SlotVisual slotVisual in slotVisuals)
        {
            Destroy(slotVisual.gameObject);
        }
        slotVisuals.Clear();
    }
}
