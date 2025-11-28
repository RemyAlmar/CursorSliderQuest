public class Action_Slash : IAction
{
    public void Execute(IEntity executor, IEntity target)
    {
        target.TakeDamage(executor.Damage);
    }
}
