using System.Collections.Generic;
using UnityEngine;

public class UI_SlotContainer : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    private GradeColorSO _colorDB;
    
    [Header("슬롯 연결")]
    [SerializeField] protected List<UI_Slot> _slots;
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
    }
    
    private void OnDestroy()
    {
        _inventory?.Unsubscribe(SetSlot, ClearSlot);             
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
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
