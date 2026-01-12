using UnityEngine;

//팝업UI의 베이스
public abstract class PUBase : UIBase
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

