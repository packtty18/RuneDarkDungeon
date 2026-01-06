using UnityEngine;

public class UI_Upgrade : MonoBehaviour
{
    private DataManager _dataManager;
    private ItemDatabaseSO _itemDB;
    private UpgradeDataSO _upgradeDB;
    private UpgradeManager _upgradeManager;
    private IngredientManager _ingredientManager;

    private void Awake()
    {
        _dataManager = DataManager.Instance;
        _itemDB = _dataManager.ItemDB;
        _upgradeDB = _dataManager.UpgradeDB;
        _ingredientManager = new(_upgradeDB);
        _upgradeManager = new(_ingredientManager, _dataManager.Inventory, _dataManager.GoldData);
        _ingredientManager.Ingredients.Subscribe(RegisterSlot);
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
}
