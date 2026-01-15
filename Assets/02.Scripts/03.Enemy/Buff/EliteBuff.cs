using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;

public class EliteBuff : EnemyBuff
{
    [SerializeField] private List<BuffSO> _berserkBuffs;

    public void ApplyBerserkBuff()
    {
        foreach (BuffSO buff in _berserkBuffs)
        {
            ApplyBuff(buff);
        }
    }
}
