public class Slot
{
    public IAction action = null;

    public Slot()
    {
        action = new Action_Void();
    }
    public Slot(IAction action)
    {
        this.action = action;
    }
}