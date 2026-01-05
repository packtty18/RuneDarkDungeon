using UnityEngine;

[CreateAssetMenu(fileName = "TestItemEffect_", menuName = "Item/ItemEffect/TestItemEffect")]
public class TestItemEffectSO : ItemEffectBaseSO
{
    [Header("수치 설정")]
    [SerializeField] private int _stat;
    [SerializeField] private int _damage;
    
    public override void OnUse(GameObject user)
    {
        Debug.Log($"데미지 : {_damage} 적용");
    }

    public override void OnEquip(GameObject user)
    {
        Debug.Log($"룬 장착으로 스탯 {_stat} 상승");
    }

    public override void OnUnequip(GameObject user)
    {
        Debug.Log($"룬 장착 해제로 스탯 {_stat} 하락");
    }
}
