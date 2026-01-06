using UnityEngine;

[System.Serializable]
public class RuneData : IItem
{
    [SerializeField] private int _id;
    [SerializeField] private EItemGrade _grade;
    
    public int ID => _id;
    public EItemGrade Grade => _grade;

    public RuneData(int id, EItemGrade grade = EItemGrade.Normal)
    {
        _id = id; 
        _grade = grade;
    }
    
    public bool TypeEquals(IItem other)
    {
        if (other is null) return false;
        return _id == other.ID && _grade == other.Grade;
    }
    
    public override string ToString() => $"[ID:{_id}] 등급:{_grade})";
}
