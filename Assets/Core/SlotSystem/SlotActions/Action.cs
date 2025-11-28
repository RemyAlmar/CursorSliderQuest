using System;

[Serializable]
public abstract class Action
{
    public abstract void Execute(IEntity executor, IEntity target, int indexSlot = -1);
    public abstract void Upgrade(float _multiplier);
}