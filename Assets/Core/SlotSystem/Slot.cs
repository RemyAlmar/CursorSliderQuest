public class Slot
{
    public ActionState state { get; set; }
    public int activationsThisTurn { get; set; }
    public int activationsThisFight { get; set; }

    public Action action = null;
    public int size = 0;
    public int placementIndex = -1;
    public Slot_SO slotData;
    public Slot()
    {
        action = new Action_Void();
    }
    public Slot(Action action)
    {
        this.action = action;
    }
    public Slot(Slot_SO _slotData, int _index)
    {
        this.slotData = _slotData;
        action = _slotData.action;
        size = _slotData.size;
        placementIndex = _index;
    }

    public void Execute(IEntity _owner, IEntity _target)
    {
        action?.Execute(_owner, _target, placementIndex);
    }

    public void Upgrade(float _multiplier)
    {
        action?.Upgrade(_multiplier);
    }
}