using System;

[Serializable]
public class SlotAction_Power : SlotAction
{
    public float powerMult = 1f;
    public override void ExecuteInTurn(IEntity executor, IEntity target, int indexSlot = -1)
    {

    }
    public override void ExecuteEndTurn(IEntity executor, IEntity target, int indexSlot = -1)
    {
        if (executor is Player _player)
        {
            Slot _slotRight = _player.GetNeighboringSlot(indexSlot, 1);
            Slot _slotLeft = _player.GetNeighboringSlot(indexSlot, -1);

            _slotRight?.MultiplyValue(powerMult);
            _slotLeft?.MultiplyValue(powerMult);
        }
    }

    public override void ResetTurn(IEntity executor, IEntity target, int indexSlot = -1)
    {

    }

    public override void ResetFight(IEntity executor, IEntity target, int indexSlot = -1)
    {

    }

    public override void MultiplyValue(float _multiplier)
    {
        powerMult *= _multiplier;
    }
}