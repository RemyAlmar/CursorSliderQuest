public interface IAction
{
    public ActionState state { get; set; }
    public int activationsThisTurn { get; set; }
    public int activationsThisFight { get; set; }
    public bool CanBeDeactivated { get; }
    public void Execute(IEntity executor, IEntity target);
}
