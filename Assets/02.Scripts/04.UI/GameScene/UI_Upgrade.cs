using UnityEngine;

public class UI_Upgrade : MonoBehaviour
{
    [SerializeField] private UpgradeDataSO _upgradeDB;
    private ItemUpgrader _itemUpgrader;

    private void Awake()
    {
        _itemUpgrader = new(_upgradeDB);
        _itemUpgrader.UpgradeInventory.Subscribe(RegisterSlot);
    }

    private void RegisterSlot(ItemData itemData)
    {
        ItemSO itemInfo = InventoryManager.Instance.GetItemInfo(itemData.ID);
        Debug.Log($"강화 슬롯에 등록 [{itemInfo.Name}] : {itemInfo.Tooltip}");
    }
}
