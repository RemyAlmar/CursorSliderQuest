using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IEntity
{
    public int maxHealth;
    private Health health;
    public Health Health { get => health; }
    public HealthBarVisual healthBarVisual;
    public Slot_SO slotDefault;


    public DoActionButton doActionButton;

    [Range(0f, 5f)] public float currentCursor = 0f;
    public int direction = 1;
    float cursorSliderMin = 0f;
    float cursorSliderMax = 5f;
    public List<Slot> slots = new();
    public List<Slot_SO> slotsSO = new();
    public CursorSliderVisual cursorSliderVisual;
    public float turnTimeSecond = 4f;
    public bool isMyTurn { get; set; } = false;
    public bool isOccupied { get; set; } = false;

    // Out Fight
    private List<Slot> outFightSlots = new();

    public void Initialize()
    {
        health = new(maxHealth);
        doActionButton.player = this;
        isMyTurn = true;
        health.OnDie += GameManager.Instance.StopFight;
        health.OnCurrentHealthChanged += () => healthBarVisual.SetSize(health.HealthNormalized);
        InitializeSlots(slotsSO);
        FillOutRestArray();

        InitializeSliderVisual();
    }
    void FillOutRestArray()
    {
        if (slots.Count == cursorSliderMax) return;
        int _toAdd = (int)cursorSliderMax - slots.Count;
        for (int i = 0; i < _toAdd; i++)
        {
            int _currentIndex = slots.Count;
            Slot _slot = new(slotDefault, _currentIndex);

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
            Slot _slot = new(_slotData, _currentIndex);
            for (int i = 0; i < _slotData.size; i++)
            {
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

            if (GameManager.Instance.anySlotRegistered && resetSlider)
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
            Slot _targetSlot = slots[slotIndex];
            ActionState previsousActionState = _targetSlot.state;
            ActionState endActionState = _targetSlot.ExecuteInTurn(previsousActionState, this, null);
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

    // OUT FIGHT
    public void OutFight()
    {
        Debug.Log("Resetting Player State.");
        health.Reset();
        cursorSliderVisual.ResetVisual();

        outFightSlots.Clear();
        // Copy current slots to outFightSlots
        foreach (Slot slot in slots)
        {
            Slot_SO slotDataCopy = Instantiate(slot.slotData);
            Slot slotCopy = new Slot(slotDataCopy, slot.placementIndex);
            outFightSlots.Add(slotCopy);
        }
    }
    public void OutFightUpdate()
    {
        // Update slots based on selected slot and click offset
        SlotVisual selectedSlotVisual = GameManager.Instance.selectedSlotVisual;
        if (selectedSlotVisual != null)
        {
            Vector3 clickOffset = GameManager.Instance.currentClickOffset;
            // Mouse Input in World Space
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Drag and Drop Logic
            selectedSlotVisual.transform.position = new Vector3(worldMousePosition.x - clickOffset.x, worldMousePosition.y - clickOffset.y, 0);
        }
    }
}
