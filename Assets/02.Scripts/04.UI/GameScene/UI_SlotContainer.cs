using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_SlotContainer : UI_Base
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    
    [Header("슬롯 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    public IReadOnlyList<UI_Slot> Slots => _slots;

    [SerializeField] private Transform _slotParent;
    [SerializeField] private UI_Slot _slotPrefab;
    [SerializeField] private UI_ScrollView _layoutController;

    public event Action<UI_Slot> OnSlotAdded;
    
    public void Initialize(IReadOnlyInventory inventory, ItemDatabaseSO itemDB)
    {
        _inventory = inventory;
        _itemDB = itemDB;

        _inventory.Subscribe(Refresh);
    }

    private void OnDestroy()
    {
        _inventory.Unsubscribe(Refresh);
    }

    private void Refresh()
    {
        var items = _inventory.Items;
        int targetCount = items.Count;

        while (_slots.Count < targetCount)
        {
            var newSlot = Instantiate(_slotPrefab, _slotParent);
            _slots.Add(newSlot);
            OnSlotAdded?.Invoke(newSlot);
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < targetCount)
            {
                var data = _itemDB.GetSlotData(items[i]);
                _slots[i].SetItem(data);
            }
            else
            {
                _slots[i].Clear();
            }
        }
        
        _layoutController?.UpdateLayout(targetCount);
    }
    
    public override void Show()
    {
        base.Show();
        Refresh();
    }
}
