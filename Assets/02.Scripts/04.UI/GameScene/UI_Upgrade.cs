using UnityEngine;

public class UI_Upgrade : MonoBehaviour
{
    private ItemDatabaseSO _itemDB;
    private UpgradeManager _upgradeManager;

    public void Initialize(ItemDatabaseSO itemDB, UpgradeManager upgradeManager)
    {
        _itemDB = itemDB;
        _upgradeManager = upgradeManager;
    }

    public void Upgrade()
    {
        _upgradeManager.Upgrade();
    }
    
    private void RegisterSlot(RuneData runeData)
    {
        ItemSO itemInfo = _itemDB.GetItemInfo(runeData.ID);
        Debug.Log($"강화 슬롯에 등록 [{itemInfo.Name}] : {itemInfo.Tooltip}");
    }
}
