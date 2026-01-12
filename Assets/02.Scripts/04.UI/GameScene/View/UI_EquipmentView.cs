using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentView : UI_Base
{
    [Header("슬롯 연결")]
    [SerializeField] private SerializableDictionary<ESkillSlot, UI_Slot> _slots;
    private Dictionary<UI_Slot, ESkillSlot> _slotDict;
    
    private ItemDatabaseSO _itemDB;
    
    public event Action<ESkillSlot> OnSlotDoubleClicked;
    
    public void Initialize(ItemDatabaseSO itemDB)
    {
        _itemDB = itemDB;
        
        _slotDict = new();
        foreach (var pair in _slots)
        {
            _slotDict.Add(pair.Value, pair.Key);
            pair.Value.OnSlotDoubleClicked += NotifySlotClicked;
        }
    }

    private void OnDestroy()
    {
        foreach (var pair in _slots)
        {
            pair.Value.OnSlotDoubleClicked -= NotifySlotClicked;
        }
    }

    public void Refresh(IEquipment equipment)
    {
        foreach (var pair in _slots)
        {
            ESkillSlot type = pair.Key;
            UI_Slot slot = pair.Value;

            ItemData item = equipment.GetItem(type);

            if (item != null)
            {
                var data = _itemDB.GetSlotData(item);
                slot.SetItem(data);
            }
            else
            {
                slot.Clear();
            }
        }
    }
    
    private void NotifySlotClicked(UI_Slot slot)
    {
        if (!_slotDict.TryGetValue(slot, out ESkillSlot slotType)) return;
        OnSlotDoubleClicked?.Invoke(slotType);
    }
}
