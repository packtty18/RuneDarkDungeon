using System;
using UnityEngine;

[Serializable]
public class Equipment : IEquipment
{
    [SerializeField] private SerializableDictionary<ESkillSlot, ItemData> _items = new();

    private SafeEvent _onChanged = new();
    
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
    
    public void Subscribe(Action action)
    {
        _onChanged.Subscribe(action);
    }

    public void Unsubscribe(Action action)
    {
        _onChanged.Unsubscribe(action);
    }

    private void Notify()
    {
        _onChanged?.Invoke();
    }
}
