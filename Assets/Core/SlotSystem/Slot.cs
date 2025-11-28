public class Slot
{
    public IAction action = null;
    public int size = 0;
    public int placementIndex = -1;
    public Slot()
    {
        action = new Action_Void();
    }
    public Slot(IAction action)
    {
        this.action = action;
    }
    public Slot(Slot_SO _slotData, int _index)
    {
        action = _slotData.action;
        size = _slotData.size;
        placementIndex = _index;
    }
}