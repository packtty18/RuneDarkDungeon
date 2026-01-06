using UnityEngine;
using System;

[System.Serializable]
public class ItemData
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

    public void GradeUp()
    {
        if (_grade == EItemGrade.Legendary) return;
        ++_grade;
    }
    
    public override string ToString() => $"[ID:{_id}] 등급:{_grade})";
}
