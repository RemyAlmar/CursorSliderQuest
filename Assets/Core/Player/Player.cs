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
    public List<Slot> slots = new();
    public List<Slot_SO> slotsSO = new();
    public CursorSliderVisual cursorSliderVisual;
    public float turnTimeSecond = 4f;
    public bool isMyTurn = false;

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
        InitializeSlots(slotsSO);
        FillOutRestArray(); // TO SUPP AVEC LA FONCTION

        //slots = new List<Slot>()      // J'AI COMMENTE CA POUR TEST LES LIGNES JUSTE AU DESSUS
        //{
        //    new Slot(),
        //    new Slot(),
        //    new Slot(),
        //    new Slot(new Action_Slash()),
        //    new Slot(),
        //    new Slot(),
        //    new Slot(),
        //};

        InitializeSliderVisual();
    }
    void FillOutRestArray()    //A SUPPRIMER, ICI JUSTE POUR TESTER ET PAS TOUT NIQUER
    {
        if (slots.Count == cursorSliderMax) return;
        int _toAdd = (int)cursorSliderMax - slots.Count;
        for (int i = 0; i < _toAdd; i++)
        {
            Slot _slot = new()
            {
                size = 1,
                placementIndex = slots.Count
            };
            slots.Add(_slot);
        }
    }


    public void InitializeSlots(List<Slot_SO> _slotsSO)
    {
        slots.Clear();
        foreach (Slot_SO _slotSO in _slotsSO)
        {
            Slot_SO _slotData = Instantiate(_slotSO);
            if ((slots.Count + _slotData.size) > cursorSliderMax) continue;

            int _currentIndex = slots.Count;
            for (int i = 0; i < _slotData.size; i++)
            {
                Slot _slot = new(_slotData, _currentIndex);
                slots.Add(_slot);
            }
        }
    }
    public Slot GetNeighboringSlot(int _slotIndex, int _dir = 1)
    {
        int _neighborIndex = GetNeighboringSlotIndex(_slotIndex, _dir);
        return _neighborIndex == -1 ? null : slots[_neighborIndex];
    }
    public int GetNeighboringSlotIndex(int _slotIndex, int _dir = 1)
    {
        _dir = Math.Sign(_dir);
        Slot _slot = slots[Mathf.Clamp(0, _slotIndex, (int)cursorSliderMax)];

        int _neighborIndex = _dir > 0 ? _slot.placementIndex + _slot.size : _slot.placementIndex - 1;
        _neighborIndex = _neighborIndex >= slots.Count || _neighborIndex < 0 ? -1 : _neighborIndex; // Retourne -1 si hors liste

        return _neighborIndex;
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
        }
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
        if (GameManager.CanDoAction && isMyTurn)
        {
            int slotIndex = Mathf.Clamp((int)currentCursor, 0, slots.Count - 1);
            IAction targetAction = slots[slotIndex].action;
            Debug.Log($"Index [{slotIndex}]  ||||||    Voisin Gauche => {GetNeighboringSlotIndex(slotIndex, -1)} || Voisin Droit => {GetNeighboringSlotIndex(slotIndex, 1)}");
            if (targetAction.CanBeDeactivated)
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
