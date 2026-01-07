
using System;
using UnityEngine;

[Serializable]
public struct RecoilData
{
    //반동
    public float YStrength;
    public float XStrength;

    public RecoilData(float upStrength, float sideStrength)
    {
        YStrength = upStrength;
        XStrength = sideStrength;
    }
}

[Serializable]
public struct KnockbackData
{
    //넉백
    public float Power;

    public KnockbackData(float power)
    {
        Power = power;
    }
}

[Serializable]
public struct DamageData
{
    public int AttackId;
    public ETeamType Team;
    public float Damage;            //데미지
    public Vector3 HitDirection;    //공격 방향 
    public KnockbackData Knockback;

    public DamageData(int attackId, ETeamType team,float damage, Vector3 hitDirection, KnockbackData knockback)
    {
        AttackId= attackId;
        Team = team;
        Damage = damage;
        HitDirection = hitDirection;
        Knockback = knockback;
    }
}

[Serializable]
public struct ItemEffectData
{
    public EItemGrade Grade;
    public ItemEffectBaseSO Effect;
}

[Serializable]
public struct UpgradeData
{
    public EItemGrade Grade;
    public int Cost;
    public int Count;
    [Range(0f, 1f)]
    public float Rate;
}

[Serializable]
public struct GradeData
{
    public EItemGrade Grade;
    public Color Color;
}

[Serializable]
public struct SlotData
{
    public ItemData Item;
    public ItemSO Info;
    public Color Color;

    public SlotData(ItemData item, ItemSO info, Color color)
    {
        Item = item;
        Info = info;
        Color = color;
    }

    public static SlotData Empty => new SlotData(null, null, Color.white);
}
