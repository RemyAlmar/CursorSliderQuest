using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
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

    public void Initialize()
    {
        health = 100;
        damage = 15;
        doActionButton.player = this;

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
    }

    public void TakeDamage(int _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            Debug.Log("Player Defeated!");
        }
    }

    // Player Update
    public void Turn(IEntity _entity)
    {
        float t = (currentCursor - cursorSliderMin) / (cursorSliderMax - cursorSliderMin);
        currentCursor = Mathf.Lerp(cursorSliderMin, cursorSliderMax, t + Time.deltaTime * direction / 4); 
        if (currentCursor >= cursorSliderMax || currentCursor <= cursorSliderMin)
        {
            direction *= -1;
        }
    }

    internal void DoAction()
    {
        IAction targetAction = slots[Mathf.Clamp((int)currentCursor, 0, slots.Count - 1)].action;
        GameManager.Instance.RegisterAction(targetAction);
        EndTurn();
    }

    private void EndTurn()
    {
        GameManager.Instance.EndTurn();
    }
}

internal class Slot
{
    public IAction action = null;

    public Slot()
    {
        action = new Action_Void();
    }
    public Slot(IAction action)
    {
        this.action = action;
    }
}