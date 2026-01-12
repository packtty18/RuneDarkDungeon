using UnityEngine;

public abstract class HUDBase : UIBase
{
    public virtual void Open()
    {
        Init();
        Show();
    }

    public virtual void Close()
    {
        Hide();
    }
}
