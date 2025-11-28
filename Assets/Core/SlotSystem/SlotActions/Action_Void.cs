using System;

[Serializable]
public class Action_Void : Action
{
    public override void Execute(IEntity executor, IEntity target, int indexSlot = -1)
    {
        // Does nothing
    }

    public override void Upgrade(float _multiplier)
    {
        throw new NotImplementedException();
    }
}