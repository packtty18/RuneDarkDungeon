using System.Collections.Generic;
using UnityEngine;

public class BossBuff : EnemyBuff
{
    [SerializeField] private List<BuffSO> _phase2; //공증10, 방증10
    [SerializeField] private List<BuffSO> _phase3;//공증20, 방증-5


    public void ApplyPhase2()
    {
        ClearAllBuffs();
        foreach (BuffSO buff in _phase2)
        {
            ApplyBuff(buff);
        }
    }

    public void ApplyPhase3()
    {
        ClearAllBuffs();
        foreach (BuffSO buff in _phase3)
        {
            ApplyBuff(buff);
        }
    }
}
