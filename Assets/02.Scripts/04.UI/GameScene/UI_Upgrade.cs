using UnityEngine;

public class UI_Upgrade : MonoBehaviour
{
    private UpgradeManager _upgradeManager = new();

    private void Start()
    {
        _upgradeManager.UpgradeInventory.Subscribe(RegisterSlot);
    }

    private void RegisterSlot(ItemData itemData)
    {
        ItemSO itemInfo = InventoryManager.Instance.GetItemInfo(itemData.ID);
        Debug.Log($"강화 슬롯에 등록 [{itemInfo.Name}] : {itemInfo.Tooltip}");
    }
}
