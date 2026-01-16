using UnityEngine;

[CreateAssetMenu(fileName = "Item_", menuName = "Item/Item")]
public class ItemSO : ScriptableObject
{
    [Header("아이템 데이터")]
    [SerializeField] private int _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField, TextArea] private string _tooltip;
    [SerializeField] private int _price;
    
    [Header("아이템 효과")]
    [SerializeField] private SkillBase _skill; 
    
    [Header("쿨타임")]
    [SerializeField] private float _coolTime;

    [Header("애니메이션 클립")]
    [SerializeField] private AnimationClip _clip;

    public float CoolTime => _coolTime;

    public AnimationClip Clip => _clip;
    
    public int ID => _id;
    public Sprite Icon => _icon;
    public string Name => _name;
    public string Tooltip => _tooltip;
    public int Price => _price;
    
    public void Use(GameObject user, EItemGrade grade)
    {
        var skill = Instantiate(_skill, user.transform.position, Quaternion.identity);
        skill.OnUse(user, grade);
    }

    public void Equip(GameObject user, EItemGrade grade) { }
    public void UnEquip(GameObject user, EItemGrade grade) { }
}
