using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Item/Item")]
public class ItemSO : ScriptableObject
{
    [Header("아이템 데이터")]
    [SerializeField] private int _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField, TextArea] private string _tooltip;
    
    [Header("아이템 사용 효과")]
    [SerializeField] private SerializableDictionary<EItemGrade, ItemEffectBaseSO> _effectDict;    
    
    public int ID => _id;
    public Sprite Icon => _icon;
    public string Name => _name;
    public string Tooltip => _tooltip;

    private ItemEffectBaseSO GetEffect(EItemGrade grade)
    {
        return _effectDict.GetValueOrDefault(grade);
    }
    
    public void Use(GameObject user, EItemGrade grade)
    {
        GetEffect(grade)?.OnUse(user);
    }

    public void Equip(GameObject user, EItemGrade grade)
    {
        GetEffect(grade)?.OnEquip(user);
    }

    public void UnEquip(GameObject user, EItemGrade grade)
    {
        GetEffect(grade)?.OnUnequip(user);
    }
}
