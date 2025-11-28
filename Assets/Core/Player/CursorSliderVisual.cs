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

        // End Turn Animation Initialization
        endTurnVisualAnimator = endTurnVisualTransform.GetComponent<Animator>();
        endTurnVisualAnimator.enabled = false;
    }

    public void UpdateCursorPosition(float cursorValue)
    {
        cursorVisualTransform.localPosition = new Vector3(cursorValue + xCursorOffset, 0f, 0f);
    }

    internal void FeedbackAction(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slotVisuals.Count)
        {
            slotVisuals[slotIndex].ActivationFeedback();
        }
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
    }
}
