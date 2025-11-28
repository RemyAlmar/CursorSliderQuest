using System;

[Serializable]
public class Action_Power : Action
{
    public float powerMult = 1f;
    public override void Execute(IEntity executor, IEntity target, int indexSlot = -1)
    {
        if (executor is Player _player)
        {
            Slot _slotRight = _player.GetNeighboringSlot(indexSlot, 1);
            Slot _slotLeft = _player.GetNeighboringSlot(indexSlot, -1);

            _slotRight?.Upgrade(powerMult);
            _slotLeft?.Upgrade(powerMult);
        }
    }

    public override void Upgrade(float _multiplier)
    {
        powerMult *= _multiplier;
    }
}