using System;
using UnityEngine;

public interface IReadOnlyEquipment
{

    ItemData GetItem(ESkillSlot slot);
    void Subscribe(Action action);
    void Unsubscribe(Action action);
}
