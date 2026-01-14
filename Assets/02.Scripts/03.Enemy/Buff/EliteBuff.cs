using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;

public class EliteBuff : EnemyBuff
{
    [SerializeField] private List<BuffSO> _berserkBuffs;

    public bool OnBerserk { get; private set; }

    public override void Init()
    {
        base.Init();
        OnBerserk = false;
    }

    public void ActiveBerserk()
    {
        if(OnBerserk)
        {
            return;
        }
        OnBerserk = true;
        _controller.Stat.EnableSuperArmor();
        foreach (BuffSO buff in _berserkBuffs)
        {
            ApplyBuff(buff);
        }
    }

}
