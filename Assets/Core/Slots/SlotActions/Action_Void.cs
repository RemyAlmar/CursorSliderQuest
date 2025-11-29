using System;

[Serializable]
public class Action_Void : Action
{
    public override void ExecuteInTurn(IEntity executor, IEntity target, int indexSlot = -1)
    {
        // Does nothing
    }
    public override void ExecuteEndTurn(IEntity executor, IEntity target, int indexSlot = -1)
    {
        // Does nothing
    }

    public override void ResetTurn(IEntity executor, IEntity target, int indexSlot = -1)
    {
        // Does nothing
    }

    public override void ResetFight(IEntity executor, IEntity target, int indexSlot = -1)
    {
        // Does nothing
    }

    public override void MultiplyValue(float _multiplier)
    {
        // Does nothing
    }
}