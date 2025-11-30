using System;

[Serializable]
public class SlotAction_Slash : SlotAction
{
    public int damage;
    public float powerMult = 1f;
    
    public override void ExecuteInTurn(IEntity executor, IEntity target, int indexSlot = -1)
    {

    }
    public override void ExecuteEndTurn(IEntity executor, IEntity target, int indexSlot = -1)
    {
        int targetDamage = (int)(damage * powerMult);
        target.TakeDamage(targetDamage);
    }

    public override void ResetTurn(IEntity executor, IEntity target, int indexSlot = -1)
    {
        powerMult = 1f;
    }
    public override void ResetFight(IEntity executor, IEntity target, int indexSlot = -1)
    {

    }

    public override void MultiplyValue(float _multiplier) // JE SAIS ON VA PAS FAIRE CA MAIS C'ETAIT POUR TEST
    {
        powerMult += _multiplier;
    }
}