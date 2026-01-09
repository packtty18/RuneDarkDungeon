using System.Collections.Generic;
using UnityEngine;

public class UI_SlotContainer : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    
    [Header("슬롯 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    public IReadOnlyList<UI_Slot> Slots => _slots;
    
    public void Initialize(IReadOnlyInventory inventory, ItemDatabaseSO itemDB)
    {
        _inventory = inventory;
        _itemDB = itemDB;

        Refresh();
        _inventory.Subscribe(Refresh);
    }
    
    private void OnDestroy()
    {
        _inventory?.Unsubscribe(Refresh);
    }
    
    private void SetSlot(ItemData itemData)
    {
        var data = _itemDB.GetSlotData(itemData);

        foreach (var slot in _slots)
        {
            if (!slot.IsEmpty) continue;
            slot.SetItem(data);
            return;
        }
    }

    private void Refresh()
    {
        var items = _inventory.Items;

        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < items.Count)
            {
                var item = items[i];
                var data = _itemDB.GetSlotData(item); 
                _slots[i].SetItem(data);
            }
            else
            {
                _slots[i].Clear();
            }
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
