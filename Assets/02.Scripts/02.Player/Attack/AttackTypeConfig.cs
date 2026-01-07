using System;
using UnityEngine;

[Serializable]
public class AttackTypeConfig
{
    [Tooltip ("공격 타입")]
    public EAttackType AttackType;

    [Tooltip("콤보 데이터 목록")]
    public ComboData[] Combos;

    [Tooltip("최대 콤보 수")]
    public int MaxComboCount => Combos?.Length ?? 0;

    public ComboData GetComboData(int comboIndex)
    {
        if (Combos == null || comboIndex <= 0 || comboIndex > Combos.Length)
        {
            return null;
        }
        return Combos[comboIndex -1];
    }
}
