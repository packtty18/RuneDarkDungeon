using UnityEngine;

public enum EStatModType
{
    Add,            //합연산 ex) +20, +30
    Multiply        //곱연산 ex) *1.2
}

public struct StatModifier
{
    public float Value;
    public EStatModType Type;

    public StatModifier(float value, EStatModType type)
    {
        Value = value;
        Type = type;
    }
}