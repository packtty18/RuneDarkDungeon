using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UI_SlotContainer _upgradeUI;
    [SerializeField] private UI_InventoryFilter _inventoryFilter;
    [SerializeField] private UI_UpgradeFilter _upgradeFilter;
    [SerializeField] private InventoryController _controller;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private SwapEventHandler _swapEventHandler;
    
    public void Initialize(IDataHandler data)
    {
        _inventoryUI.Initialize(data.Inventory, data.ItemDB);
        _upgradeUI.Initialize(data.UpgradeInventory, data.ItemDB);
        
        _inventoryFilter.Initialize(_upgradeManager, data.UpgradeInventory, _inventoryUI.Slots);
        _upgradeFilter.Initialize(_upgradeManager, _upgradeUI.Slots);
        
        _upgradeManager.Initialize(data.UpgradeDB, data.UpgradeInventory, data.Inventory, data.GoldData);
        _swapEventHandler.Initialize(data.Inventory);
    }
}
