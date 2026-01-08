using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    private GradeColorSO _colorDB;
    
    [Header("UI 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    [SerializeField] private UI_Upgrade _upgrade;
    
    public List<UI_Slot> Slots => _slots;
    
    public void Initialize(IReadOnlyInventory inventory, ItemDatabaseSO itemDB, GradeColorSO colorDB)
    {
        _inventory = inventory;
        _itemDB = itemDB;
        _colorDB = colorDB;
        BindInventory();
    }

    private void BindInventory()
    {
        foreach (var item in _inventory.Items)
        {
            SetSlot(item);
        }
        _inventory.Subscribe(SetSlot, ClearSlot);
        _upgrade.OnUIActived += UpgradeMode;
    }
    
    private void OnDestroy()
    {
        _inventory?.Unsubscribe(SetSlot, ClearSlot);             
        _upgrade.OnUIActived -= UpgradeMode;
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
    
    private void UpgradeMode(bool isOn)
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty) continue;
            slot.SetUpgradeMode(isOn);
        }
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
