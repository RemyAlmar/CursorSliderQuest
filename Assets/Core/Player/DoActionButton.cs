using UnityEngine;

public class DoActionButton : MonoBehaviour, IClickable
{
    [HideInInspector] public Player player;

    public void OnClick()
    {
        player.DoAction();
    }

    public void OnClickOutside() { }

    public void OnCursorDown() { }

    public void OnCursorEnter() { }

    public void OnCursorExit() { }

    public void OnCursorUp() { }
}
