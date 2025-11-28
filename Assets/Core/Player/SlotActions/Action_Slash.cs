using System;

[Serializable]
public class Action_Slash : Action
{
    public override void Execute(IEntity executor, IEntity target)
    {
        target.TakeDamage(executor.Damage);
    }
}