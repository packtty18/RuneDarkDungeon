using UnityEngine;

public interface IEquipmentManager
{
    void EquipItem(ESkillSlot slot, ItemData newItem);
    void UnEquipItem(ESkillSlot slot);
}
