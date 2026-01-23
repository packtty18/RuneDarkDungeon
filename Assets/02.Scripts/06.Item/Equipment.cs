using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Equipment : IEquipment
{
    [SerializeField] private SerializableDictionary<ESkillSlot, ItemData> _items;
    public IReadOnlyDictionary<ESkillSlot, ItemData> Items => _items;
    
    private event Action _onChanged;

    public Equipment(SerializableDictionary<ESkillSlot, ItemData> items = null)
    {
        _items = items ?? new();
    }
    
    public ItemData Equip(ESkillSlot slot, ItemData item)
    {
        _items.Remove(slot, out var oldItem);
        _items.Add(slot, item);
        Notify();

        return oldItem;
    }

    public ItemData UnEquip(ESkillSlot slot)
    {
        if (!_items.Remove(slot, out var item)) return null;
        Notify();
        
        return item;
    }

    public ItemData GetItem(ESkillSlot slot)
    {
        return _items.GetValueOrDefault(slot);
    }

    public bool TryGetItem(ESkillSlot slot, out ItemData item)
    {
        item = _items.GetValueOrDefault(slot);
        if (item == null) return false;
        return true;
    }
    
    public void Subscribe(Action action)
    {
        _onChanged += action;
    }

    public void Unsubscribe(Action action)
    {
        _onChanged -= action;
    }

    private void Notify()
    {
        _onChanged?.Invoke();
    }
}
