using System;

[Serializable]
public abstract class SlotAction
{
    public abstract void ExecuteInTurn(IEntity executor, IEntity target, int indexSlot = -1);
    public abstract void ExecuteEndTurn(IEntity executor, IEntity target, int indexSlot = -1);
    public abstract void ResetTurn(IEntity executor, IEntity target, int indexSlot = -1);
    public abstract void ResetFight(IEntity executor, IEntity target, int indexSlot = -1);
    public abstract void MultiplyValue(float _multiplier);
}