using UnityEngine;

public class EliteStat : EnemyStat
{
    private EliteController _controller;
    public bool OnBerserk { get; private set; }

    
    protected override void InitFromData(MonsterDataSO data)
    {
        OnBerserk = false;
        _floatValues[EEnemyValueFloat.ChargeCooldown].Init(data.chargeCooldown);
        _floatValues[EEnemyValueFloat.ChargeRange].Init(data.chargeRange);
        _floatValues[EEnemyValueFloat.ChargeSpeed].Init(data.chargeSpeed);
        _floatValues[EEnemyValueFloat.ChargeDistance].Init(data.chargeDistance);
        base.InitFromData(data);
    }

    public void ActiveBerserk()
    {
        if (OnBerserk)
        {
            return;
        }
        OnBerserk = true;
        EnableSuperArmor();
        _controller.Buff.ApplyBerserkBuff();
    }
}
