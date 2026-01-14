using UnityEngine;

//팝업UI의 베이스
public abstract class PUBase : UIBase
{
    public virtual void Open()
    {
        Init();
        Show();

        PauseContext.Pause(EPauseReason.UI);
    }

    public virtual void Close()
    {
        PauseContext.Resume();
        Hide();
    }
}

