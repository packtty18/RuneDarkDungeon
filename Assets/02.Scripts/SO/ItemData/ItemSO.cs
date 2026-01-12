using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Item/Item")]
public class ItemSO : ScriptableObject
{
    [Header("아이템 데이터")]
    [SerializeField] private int _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField, TextArea] private string _tooltip;
    
    [Header("아이템 효과")]
    [SerializeField] private ItemEffectBaseSO _effect; 
    
    [Header("쿨타임")]
    [SerializeField] private float _coolTime;
    public float CoolTime => _coolTime;
    
    public int ID => _id;
    public Sprite Icon => _icon;
    public string Name => _name;
    public string Tooltip => _tooltip;
    
    public void Use(GameObject user, EItemGrade grade)
    {
        _effect.OnUse(user, grade);
    }

    public void Equip(GameObject user, EItemGrade grade)
    {
        _effect.OnEquip(user, grade);
    }

    public void UnEquip(GameObject user, EItemGrade grade)
    {
        _effect.OnUnequip(user, grade);
    }
}
