using UnityEngine;

public class BossStat : EliteStat
{
    private BossBuff _buff;
    public bool OnPhase1 { get; private set; }
    public bool OnPhase2 { get; private set; }
    public bool OnPhase3 { get; private set; }

    protected override void InitFromData(MonsterDataSO data)
    {
        _buff = _controller.Buff as BossBuff;
        OnPhase1 = true;
        OnPhase2 = false;
        OnPhase3 = false;

        base.InitFromData(data);
    }

    public void ActivePhase2()
    {
        if (!OnPhase1 && OnPhase2)
        {
            return;
        }

        OnPhase2 = true;
        _buff.ApplyPhase2();
    }

    public void ActivePhase3()
    {
        if (!OnPhase2 && OnPhase3)
        {
            return;
        }

        OnPhase3 = true;
        _buff.ApplyPhase3();
    }
}
