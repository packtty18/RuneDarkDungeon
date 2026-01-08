using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Upgrade : MonoBehaviour
{
    private IReadOnlyInventory _upgradeInventory;
    private ItemDatabaseSO _itemDB;
    private GradeColorSO _colorDB;

    [Header("UI 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    [SerializeField] private TextMeshProUGUI _costTextUI;
    [SerializeField] private TextMeshProUGUI _rateTextUI;
    private UpgradeManager _upgradeManager;
    
    public List<UI_Slot> Slots => _slots;
    
    public event Action<bool> OnUIActived;
    
    public void Initialize(UpgradeManager upgradeManager, ItemDatabaseSO itemDB, GradeColorSO colorDB)
    {
        _upgradeManager = upgradeManager;
        _upgradeInventory = upgradeManager.UpgradeInventory;
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
        
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked += OnClickSlot;
        }
        
        _upgradeInventory.Subscribe(SetSlot, ClearSlot);
        _upgradeManager.OnUpgradeDataChanged += SetUpgradeInfo;
    }
        
    private void OnDestroy()
    {
        _upgradeInventory?.Unsubscribe(SetSlot, ClearSlot);
        _upgradeManager.OnUpgradeDataChanged -= SetUpgradeInfo;
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
    
    private void SetSlotCount(int count)
    {
        foreach (var slot in _slots)
        {
            if (count-- > 0)
            {
                slot.gameObject.SetActive(true);
            }
            else
            {
                slot.gameObject.SetActive(false);
            }
        }
    }
    
    private void OnClickSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _upgradeManager.Unregister(slot.Data.Item);
    }

    private void SetUpgradeInfo(UpgradeData data)
    {
        SetSlotCount(data.Count);
        _costTextUI.SetText("{0} 골드", data.Cost);
        _rateTextUI.SetText("{0}% 성공", data.Rate * 100);
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
