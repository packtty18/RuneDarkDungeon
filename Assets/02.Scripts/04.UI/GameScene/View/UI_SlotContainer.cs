using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_SlotContainer : UI_Base
{ 
    [SerializeField] private ItemBorderSO _borderDB;
    
    [Header("슬롯 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private UI_Slot _slotPrefab;
    [SerializeField] private UI_ScrollView _layoutController;

    public event Action<UI_Slot> OnSlotClicked;
    public event Action<UI_Slot> OnSlotHovered;
    
    private void Awake()
    {
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked += NotifySlotClicked;
            slot.OnSlotHovered += NotifySlotHovered;
        }
    }

    private void OnDestroy()
    {
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked -= NotifySlotClicked;
            slot.OnSlotHovered -= NotifySlotHovered;
        }
    }

    public void Refresh(IReadOnlyList<ItemData> items)
    {
        int targetCount = items.Count;
        while (_slots.Count < targetCount)
        {
            var newSlot = Instantiate(_slotPrefab, _slotParent);
            _slots.Add(newSlot);
            newSlot.OnSlotClicked += NotifySlotClicked;
            newSlot.OnSlotHovered += NotifySlotHovered;
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < targetCount)
            {
                var border = _borderDB.GetBorderSprite(items[i].Grade);
                _slots[i].SetItem(items[i], border);
                _slots[i].SetActive(true);
            }
            else
            {
                _slots[i].Clear();
                _slots[i].SetActive(false);
            }
        }
        
        _layoutController?.UpdateLayout(targetCount);
    }
    
    public void RefreshFilter(IReadOnlyForge forge = null)
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty) continue;
            bool isOn = forge == null || forge.CanRegister(slot.Item);
            slot.SetFilter(isOn);
        }
    }
    
    public void SetSlotCount(int count)
    {
        foreach (var slot in _slots)
        {
            slot.SetActive(count-- > 0);
        }
    }
    
    private void NotifySlotClicked(UI_Slot slot)
    {
        OnSlotClicked?.Invoke(slot);
    }

    private void NotifySlotHovered(UI_Slot slot)
    {
        OnSlotHovered?.Invoke(slot);   
    }
}
