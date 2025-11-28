using System;

[Serializable]
public class Action_Slash : Action
{
    public int damage;
    public override void Execute(IEntity executor, IEntity target, int indexSlot = -1)
    {
        target.TakeDamage(damage);
    }

    public override void Upgrade(float _multiplier) // JE SAIS ON VA PAS FAIRE CA MAIS C'ETAIT POUR TEST
    {
        damage *= (int)_multiplier;
    }
}