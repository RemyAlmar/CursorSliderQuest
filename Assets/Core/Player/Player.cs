using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IEntity
{
    public int health = 100;
    public int damage = 15;
    public int Health { get => health; }
    public int Damage { get => damage; }

    public IAction[] actions;

    public DoActionButton doActionButton;

    [Range(0f, 7f)] public float currentCursor = 0f;
    public int direction = 1;
    float cursorSliderMin = 0f;
    float cursorSliderMax = 7f;
    List<Slot> slots = new();
    public CursorSliderVisual cursorSliderVisual;
    public float turnTimeSecond = 4f;
    public bool isMyTurn { get; set; } = false;
    public bool isOccupied { get; set; } = false;

    public void Initialize()
    {
        health = 100;
        damage = 15;
        doActionButton.player = this;
        isMyTurn = true;

        actions = new IAction[]
        {
            new Action_Slash()
        };

        slots = new List<Slot>()
        {
            new Slot(),
            new Slot(),
            new Slot(),
            new Slot(new Action_Slash()),
            new Slot(),
            new Slot(),
            new Slot(),
        };

        InitializeSliderVisual();
    }

    private void InitializeSliderVisual()
    {
        if (cursorSliderVisual != null)
            cursorSliderVisual.Initialize(slots);
    }

    public void TakeDamage(int _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            Debug.Log("Player Defeated!");
            GameManager.Instance.StopFight();
        }
    }

    public void OutFightReset()
    {
        Debug.Log("Resetting Player State.");
        health = 100;
        cursorSliderVisual.ResetVisual();
    }

    // Player Update
    public void Turn(IEntity _entity)
    {
        if (isMyTurn)
        {
            bool resetSlider = false;

            float t = (currentCursor - cursorSliderMin) / (cursorSliderMax - cursorSliderMin);
            currentCursor = Mathf.Lerp(cursorSliderMin, cursorSliderMax, t + Time.deltaTime * direction / turnTimeSecond);
            if (currentCursor >= cursorSliderMax || currentCursor <= cursorSliderMin)
            {
                if (direction < 0)
                {
                    resetSlider = true;
                }
                direction *= -1;
            }

            if (GameManager.Instance.anyActionRegistered && resetSlider)
            {
                EndTurn();
            }

            // Visual Update
            cursorSliderVisual.UpdateCursorPosition(currentCursor);
        }
    }

    public void DoAction()
    {
        if (GameManager.CanDoAction && GameManager.Instance.inFight && isMyTurn)
        {
            int slotIndex = Mathf.Clamp((int)currentCursor, 0, slots.Count - 1);
            IAction targetAction = slots[slotIndex].action;
            if (targetAction.canBeDeactivated)
            {
                // TODO Visual Feedback for Deactivation and deactivate
            }
            GameManager.Instance.RegisterAction(targetAction);

            // Visual Feedback
            cursorSliderVisual.FeedbackAction(slotIndex);
        }
    }

    public void StartTurn()
    {
        // Visual Feedback
        cursorSliderVisual.StartTurn(1 / GameManager.Instance.startTurnDelay);
        StartCoroutine(StartTurnRoutine());
    }

    IEnumerator StartTurnRoutine()
    {
        yield return new WaitForSeconds(GameManager.Instance.startTurnDelay);
        isMyTurn = true;
    }

    private void EndTurn()
    {
        isMyTurn = false;
        cursorSliderVisual.EndTurn();
        GameManager.Instance.EndTurn();
    }
}
