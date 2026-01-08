using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Upgrade : MonoBehaviour
{
    private IReadOnlyInventory _upgradeInventory;
    private ItemDatabaseSO _itemDB;
    private GradeColorSO _colorDB;

    [Header("UI 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    public List<UI_Slot> Slots => _slots;
    
    public event Action<bool> OnUIActived;
    
    public void Initialize(IReadOnlyInventory upgradeInventory , ItemDatabaseSO itemDB, GradeColorSO colorDB)
    {
        _upgradeInventory = upgradeInventory;
        _itemDB = itemDB;
        _colorDB = colorDB;
        BindInventory();
    }
    
    private void BindInventory()
    {
        foreach (var item in _upgradeInventory.Items)
        {
            SetSlot(item);
        }
        
        _upgradeInventory.Subscribe(SetSlot, ClearSlot);
    }
        
    private void OnDestroy()
    {
        _upgradeInventory?.Unsubscribe(SetSlot, ClearSlot);
    }
    
    private void SetSlot(ItemData itemData)
    {
        ItemSO itemInfo = _itemDB.GetItemInfo(itemData.ID);
        Color color = _colorDB.GetColor(itemData.Grade);

        SlotData data = new(itemData, itemInfo, color);
        
        foreach (var slot in _slots)
        {
            if (!slot.IsEmpty) continue;
            slot.SetItem(data);
            return;
        }
    }

    private void ClearSlot(ItemData itemData)
    {
        foreach (var slot in _slots)
        {
            if (slot.Data.Item != itemData) continue;
            slot.Clear();
            return;
        }
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        OnUIActived?.Invoke(gameObject.activeSelf);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        OnUIActived?.Invoke(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        OnUIActived?.Invoke(false);
    }
}
