using UnityEngine;

public class EliteStat : EnemyStat
{
    protected override void InitFromData(MonsterDataSO data)
    {
        _floatValues[EEnemyValueFloat.ChargeCooldown].Init(data.chargeCooldown);
        _floatValues[EEnemyValueFloat.ChargeRange].Init(data.chargeRange);
        _floatValues[EEnemyValueFloat.ChargeSpeed].Init(data.chargeSpeed);
        _floatValues[EEnemyValueFloat.ChargeDistance].Init(data.chargeDistance);
        base.InitFromData(data);
    }
}
