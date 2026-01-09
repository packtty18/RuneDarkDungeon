using System.Collections.Generic;
using UnityEngine;

public class UI_SlotContainer : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    
    [Header("슬롯 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    public IReadOnlyList<UI_Slot> Slots => _slots;

    [SerializeField] private Transform _slotParent;
    [SerializeField] private UI_Slot _slotPrefab;
    [SerializeField] private UI_ScrollView _layoutController;
    
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

    private void Refresh()
    {
        var items = _inventory.Items;
        int targetCount = items.Count;

        while (_slots.Count < targetCount)
        {
            var newSlot = Instantiate(_slotPrefab, _slotParent);
            _slots.Add(newSlot);
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            if (i < targetCount)
            {
                _slots[i].SetActive(true);
                var data = _itemDB.GetSlotData(items[i]);
                _slots[i].SetItem(data);
            }
            else
            {
                _slots[i].SetActive(false);
            }
        }

        _layoutController?.UpdateLayout(targetCount);
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
