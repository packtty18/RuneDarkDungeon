using UnityEngine;
using System;

[System.Serializable]
public class ItemData : IEquatable<ItemData>
{
    [SerializeField] private int _id;
    [SerializeField] private EItemGrade _grade;
    
    public int ID => _id;
    public EItemGrade Grade => _grade;

    public ItemData(int id, EItemGrade grade = EItemGrade.Normal)
    {
        _id = id; 
        _grade = grade;
    }

    public void Upgrade()
    {
        if (_grade == EItemGrade.Legendary) return;
        ++_grade;
    }

    public bool Equals(ItemData other)
    {
        if (other == null) return false;
        return _id == other._id && _grade == other._grade;
    }
    
    public static bool operator ==(ItemData left, ItemData right)
    {
        if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
        return left.Equals(right);
    }
    
    public static bool operator !=(ItemData left, ItemData right)
    {
        return !(left == right);
    }
    
    public override string ToString() => $"[ID:{_id}] 등급:{_grade})";
}
