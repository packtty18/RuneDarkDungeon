using UnityEngine;

[System.Serializable]
public class ItemData
{
    [SerializeField] private int _id;
    [SerializeField] private EItemGrade _grade;
    [System.NonSerialized] private ItemSO _info;
    
    public int ID => _id;
    public EItemGrade Grade => _grade;
    public ItemSO Info => _info;
    public Sprite Icon => _info.Icon;
    public EPoolType Skill => _info.Skill;
    
    public ItemData(int id, EItemGrade grade = EItemGrade.Normal)
    {
        _id = id; 
        _grade = grade;
    }
    
    public void SetInfo(ItemSO info)
    {
        _info = info;
    }

    public float GetCoolTime()
    {
        return _info.CoolTime;
    }

    public AnimationClip GetClip()
    {
        return _info.Clip;
    }
    
    public bool IsMaxGrade=> _grade.IsMaxGrade();

    public ItemData GetUpgradedItem()
    {
        return new ItemData(_id, _grade.Next());
    }
    
    public bool TypeEquals(ItemData other)
    {
        if (other is null) return false;
        return _id == other._id && _grade == other._grade;
    }
    
    public override string ToString() => $"[ID:{_id}] 등급:{_grade})";
}
