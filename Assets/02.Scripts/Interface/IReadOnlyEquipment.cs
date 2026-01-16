using System;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyEquipment
{
    IReadOnlyDictionary<ESkillSlot, ItemData> Items { get; }
    ItemData GetItem(ESkillSlot slot);
    bool TryGetItem(ESkillSlot slot, out ItemData item);
    void Subscribe(Action action);
    void Unsubscribe(Action action);
}
