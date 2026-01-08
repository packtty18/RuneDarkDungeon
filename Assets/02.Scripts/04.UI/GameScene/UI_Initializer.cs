using TMPro;
using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UI_SlotContainer _upgradeUI;
    
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private InventoryEventHandler _eventHandler;
    [SerializeField] private UpgradeEventHandler _upgradeEventHandler;
    
    private void Awake()
    {
        //var data = DataManager.Instance;
        var data = RuneUser.Instance;

        _inventoryUI.Initialize(data.Inventory, data.ItemDB, data.ColorDB);
        _upgradeUI.Initialize(_upgradeManager.UpgradeInventory, data.ItemDB, data.ColorDB);
        
        _upgradeManager.Initialize(data.UpgradeDB, data.Inventory, data.GoldData);
        _eventHandler.Initialize(_upgradeManager, _inventoryUI.Slots);
        _upgradeEventHandler.Initialize(_upgradeManager, _upgradeUI.Slots);
    }
}
