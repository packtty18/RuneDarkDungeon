using UnityEngine;

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

    public bool CanUpgrade(ItemData item)
    {
        if (item != null) return TypeEquals(item);
        return !_grade.IsMaxGrade();
        
    }
    
    public bool TypeEquals(ItemData other)
    {
        if (other is null) return false;
        return _id == other._id && _grade == other._grade;
    }
    
    public override string ToString() => $"[ID:{_id}] 등급:{_grade})";
}
