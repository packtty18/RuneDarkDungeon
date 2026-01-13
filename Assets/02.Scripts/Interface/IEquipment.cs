using UnityEngine;

public interface IEquipment : IReadOnlyEquipment
{
    ItemData Equip(ESkillSlot slot, ItemData item);
    ItemData UnEquip(ESkillSlot slot);
    bool UseItem(GameObject user, ESkillSlot slot, out float coolTime);
}
