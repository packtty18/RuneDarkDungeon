using System;
using UnityEngine;

public interface IEquipment
{
    ItemData Equip(ESkillSlot slot, ItemData item);
    ItemData UnEquip(ESkillSlot slot);
    ItemData GetItem(ESkillSlot slot);
    void Subscribe(Action action);
    void Unsubscribe(Action action);
}
