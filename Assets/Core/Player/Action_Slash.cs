public class Action_Slash : IAction
{
    public bool canBeDeactivated => true;

    public ActionState state { get; set; } = ActionState.Neutral;
    public int activationsThisTurn { get; set; } = 0;
    public int activationsThisFight { get; set; } = 0;

    public void Execute(IEntity executor, IEntity target)
    {
        target.TakeDamage(executor.Damage);
    }
}
