using System;
using System.Collections.Generic;
using UnityEngine;

public class CursorSliderVisual : MonoBehaviour
{
    [SerializeField] private SlotVisual slotVisualPrefab;
    [SerializeField] private Transform cursorVisualTransform;
    [SerializeField] private Transform endTurnVisualTransform;
    private Animator endTurnVisualAnimator;
    private int endTurnVisual_StartTurnAnimHash = Animator.StringToHash("StartTurn");
    private int endTurnVisual_AnimationSpeedHash = Animator.StringToHash("AnimationSpeed");
    private int endTurnVisual_ResetAnimHash = Animator.StringToHash("Reset");
    float xCursorOffset = -0.5f;
    List<SlotVisual> slotVisuals = new();

    public void Initialize(List<Slot> slots)
    {
        ClearList();
        int targetIndex = 0;
        bool slotVisualInstantiated = false;
        SlotVisual lastSlotVisual = null;
        for (int i = 0; i < slots.Count; i++)
        {
            Slot slot = slots[i];

            if (!slotVisualInstantiated)
            {
                slotVisualInstantiated = true;
                SlotVisual slotVisual = Instantiate(slot.slotData.slotVisualPrefab, transform);
                lastSlotVisual = slotVisual;
                slot.visual = slotVisual;
                slotVisual.transform.localPosition = new Vector3(targetIndex + slot.slotData.size / 2f - 0.5f, 0f, 0f);

                slotVisual.Initialize(slot);
                slotVisuals.Add(slotVisual);
            }
            else
            {
                slot.visual = lastSlotVisual;
            }

            if (i == slot.slotData.size + targetIndex - 1)
            {
                targetIndex += slot.slotData.size;
                slotVisualInstantiated = false;
            }
        }

        // End Turn Animation Initialization
        endTurnVisualAnimator = endTurnVisualTransform.GetComponent<Animator>();
        endTurnVisualAnimator.enabled = false;
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

    public void EndTurn()
    {
        endTurnVisualAnimator.enabled = true;
        endTurnVisualAnimator.Rebind();
    }

    public void StartTurn(float delay)
    {
        if (endTurnVisualAnimator.enabled)
        {
            endTurnVisualAnimator.SetFloat(endTurnVisual_AnimationSpeedHash, delay);
            endTurnVisualAnimator.SetTrigger(endTurnVisual_StartTurnAnimHash);
        }
    }

    internal void ResetVisual()
    {
        endTurnVisualAnimator.SetTrigger(endTurnVisual_ResetAnimHash);
        UpdateCursorPosition(0f);
    }
}
