using UnityEngine;

public interface IEquipmentManager
{
    void EquipItem(ESkillSlot slot, ItemData newItem);
    void UnEquipItem(ESkillSlot slot);
    bool UseItem(GameObject user, ESkillSlot slot, out float coolTime);
}
