using System.Collections.Generic;
using UnityEngine;

public class UI_Upgrade : MonoBehaviour
{
    private ItemDatabaseSO _itemDB;
    private UpgradeManager _upgradeManager;

    [Header("UI 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    
    public void Initialize(ItemDatabaseSO itemDB, UpgradeManager upgradeManager)
    {
        _itemDB = itemDB;
        _upgradeManager = upgradeManager;
    }

    public void Upgrade()
    {
        _upgradeManager.Upgrade();
    }
    
    private void RegisterSlot(ItemData itemData)
    {
        ItemSO itemInfo = _itemDB.GetItemInfo(itemData.ID);
        Debug.Log($"강화 슬롯에 등록 [{itemInfo.Name}] : {itemInfo.Tooltip}");
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }}
