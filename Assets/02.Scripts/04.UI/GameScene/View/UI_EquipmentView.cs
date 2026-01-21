using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipmentView : PUBase
{
    [Header("슬롯 연결")]
    [SerializeField] private SerializableDictionary<ESkillSlot, UI_Slot> _slots;
    private Dictionary<UI_Slot, ESkillSlot> _slotDict;
    
    [Header("슬롯 설정")]
    [SerializeField] private bool _isInteractable;
    [SerializeField] private bool _showFrame;
    [SerializeField, ShowIf(nameof(_showFrame))] private ItemFrameSO _frameDB;
    
    public event Action<ESkillSlot> OnSlotDoubleClicked;
    public event Action<ESkillSlot> OnSlotClicked;
    public event Action<UI_Slot> OnSlotHovered;
    
    public void Initialize()
    {
        _slotDict = new();
        foreach (var pair in _slots)
        {
            UI_Slot slot = pair.Value;
            ESkillSlot type = pair.Key;
            _slotDict.Add(slot, type);

            if (!_isInteractable) continue;
            slot.OnSlotDoubleClicked += NotifySlotDoubleClicked;
            slot.OnSlotClicked += NotifySlotClicked;
            slot.OnSlotHovered += NotifySlotHovered;
        }
    }

    private void OnDestroy()
    {
        foreach (var pair in _slots)
        {
            UI_Slot slot = pair.Value;
            slot.OnSlotDoubleClicked -= NotifySlotDoubleClicked;
            slot.OnSlotClicked -= NotifySlotClicked;
            slot.OnSlotHovered -= NotifySlotHovered;
        }
    }

    public void Refresh(IReadOnlyEquipment equipment)
    {
        foreach (var pair in _slots)
        {
            ESkillSlot type = pair.Key;
            UI_Slot slot = pair.Value;

            ItemData item = equipment.GetItem(type);

            if (item == null)
            {
                slot.Clear();
                continue;
            }

            if (_showFrame)
            {
                var frame = _frameDB.GetFrameSprite(item.Grade); 
                slot.SetItem(item, frame);
            }
            else
            {
                slot.SetItem(item);
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
