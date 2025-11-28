using System;

[Serializable]
public class Action_Void : Action
{
    public override void Execute(IEntity executor, IEntity target)
    {
        // Does nothing
    }
}