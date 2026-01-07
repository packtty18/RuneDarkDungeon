using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    private GradeColorSO _colorDB;
    
    [Header("UI 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    [SerializeField] private UI_InventoryEventHandler _eventHandler;
    
    public void Initialize(IReadOnlyInventory inventory, ItemDatabaseSO itemDB, GradeColorSO colorDB)
    {
        _inventory = inventory;
        _itemDB = itemDB;
        _colorDB = colorDB;
        _eventHandler.Initialize(_slots);
        BindInventory();
    }
    
    private void OnDestroy()
    {
        _inventory?.Unsubscribe(SetSlot);
    }

    private void BindInventory()
    {
        foreach (var item in _inventory.Items)
        {
            SetSlot(item);
        }
        _inventory.Subscribe(SetSlot);
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

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
