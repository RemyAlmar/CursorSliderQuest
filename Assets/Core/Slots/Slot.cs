using UnityEngine;

[System.Serializable]
public class Slot
{
    public ActionState state;
    public int activationsThisTurn;
    public int activationsThisFight;

    public SlotAction action = null;
    public int size = 0;
    public int placementIndex = -1;
    public Slot_SO slotData;
    internal SlotVisual visual;

    public Slot()
    {
        action = new SlotAction_Void();
    }
    public Slot(SlotAction action)
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

    public ActionState ExecuteInTurn(ActionState previsousActionState, IEntity _owner, IEntity _target)
    {
        if (state == ActionState.Neutral && slotData != null && activationsThisTurn < slotData.activationPerTurn)
        {
            action?.ExecuteInTurn(_owner, _target, placementIndex);
            ActionState previousState = state;
            state = ActionState.Activated;
            UpdateVisual(previousState, state);
            activationsThisTurn++;
            activationsThisFight++;
            GameManager.Instance.RegisterSlot(this);

            // Visual Feedback
            visual.ActivationFeedback(previsousActionState, state);
        }
        else
        {
            visual.NegativeFeedback();
        }
        return state;
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

        ActionState previousState = state;
        state = ActionState.Neutral;

        UpdateVisual(previousState, state);

        activationsThisTurn = 0;
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

    void UpdateVisual(ActionState startState, ActionState endState)
    {
        visual.UpdateVisual(startState, endState);
    }
}