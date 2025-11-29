using UnityEngine;

[System.Serializable]
public class Slot
{
    public ActionState state { get; set; }
    public int activationsThisTurn { get; set; }
    public int activationsThisFight { get; set; }

    public Action action = null;
    public int size = 0;
    public int placementIndex = -1;
    public Slot_SO slotData;
    internal SlotVisual visual;

    public Slot()
    {
        action = new Action_Void();
    }
    public Slot(Action action)
    {
        this.action = action;
    }
    public Slot(Slot_SO _slotData, int _index)
    {
        this.slotData = _slotData;
        action = _slotData.actionActivate;
        size = _slotData.size;
        placementIndex = _index;
    }

    public void ExecuteInTurn(IEntity _owner, IEntity _target)
    {
        if (state == ActionState.Neutral)
        {
            action?.ExecuteInTurn(_owner, _target, placementIndex);
            if (slotData != null && slotData.canBeDeactivated)
            {
                state = ActionState.Deactivated;
                UpdateVisual();
            }
            activationsThisTurn++;
            activationsThisFight++;
        }
    }

    public void ExecuteEndTurn(IEntity _owner, IEntity _target)
    {
        if (activationsThisTurn > 0)
        {
            action?.ExecuteEndTurn(_owner, _target, placementIndex);
        }
    }

    public void ResetTurn(IEntity _owner, IEntity _target)
    {
        action?.ResetTurn(_owner, _target, placementIndex);

        state = ActionState.Neutral;

        UpdateVisual();

        activationsThisTurn++;
        activationsThisFight++;
    }

    public void ResetFight(IEntity _owner, IEntity _target)
    {
        action?.ResetFight(_owner, _target, placementIndex);

        activationsThisTurn = 0;
        activationsThisFight = 0;
    }

    public void MultiplyValue(float _multiplier)
    {
        action?.MultiplyValue(_multiplier);
    }

    void UpdateVisual()
    {
        switch (state)
        {
            case ActionState.Neutral:
                // Update visual to neutral state
                visual.UpdateVisual(ActionState.Neutral);
                break;
            case ActionState.Activated:
                // Update visual to activated state
                visual.UpdateVisual(ActionState.Activated);
                break;
            case ActionState.Deactivated:
                // Update visual to deactivated state
                visual.UpdateVisual(ActionState.Deactivated);
                break;
                // Add other states as needed
        }
    }
}