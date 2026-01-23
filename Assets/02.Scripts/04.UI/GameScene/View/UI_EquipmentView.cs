using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentView : PUBase
{
    [Header("슬롯 연결")]
    [SerializeField] private SerializableDictionary<ESkillSlot, UI_Slot> _slots;
    private Dictionary<UI_Slot, ESkillSlot> _slotDict;
    
    [SerializeField] private ItemFrameSO _frameDB;
    
    public event Action<ESkillSlot> OnSlotDoubleClicked;
    public event Action<ESkillSlot> OnSlotClicked;
    public event Action<UI_Slot> OnSlotHovered;
    
    public void Initialize()
    {
        _slotDict = new();
        foreach (var pair in _slots)
        {
            _slotDict.Add(pair.Value, pair.Key);
            pair.Value.OnSlotDoubleClicked += NotifySlotDoubleClicked;
            pair.Value.OnSlotClicked += NotifySlotClicked;
            pair.Value.OnSlotHovered += NotifySlotHovered;
        }
    }

    private void OnDestroy()
    {
        foreach (var pair in _slots)
        {
            pair.Value.OnSlotDoubleClicked -= NotifySlotDoubleClicked;
            pair.Value.OnSlotClicked -= NotifySlotClicked;
            pair.Value.OnSlotHovered -= NotifySlotHovered;
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
                var border = _frameDB.GetBorderSprite(item.Grade);
                slot.SetItem(item, border);
            }
            else
            {
                slot.Clear();
            }
        }
    }
    
    private void NotifySlotDoubleClicked(UI_Slot slot)
    {
        if (!_slotDict.TryGetValue(slot, out ESkillSlot slotType)) return;
        OnSlotDoubleClicked?.Invoke(slotType);
    }

    private void NotifySlotClicked(UI_Slot slot)
    {
        if (!_slotDict.TryGetValue(slot, out ESkillSlot slotType)) return;
        OnSlotClicked?.Invoke(slotType);
    }

    private void NotifySlotHovered(UI_Slot slot)
    {
        OnSlotHovered?.Invoke(slot);
    }
}
