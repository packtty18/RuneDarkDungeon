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
    
    [Header("UI 연결")]
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;
    
    [SerializeField] private UI_WindowToggleButton _upgradeUIButton;

    private SwapEventHandler _swapEventHandler;
    private RegisterEventHandler _registerEventHandler;
    private UnregisterEventHandler _unregisterEventHandler;
    
    private void Awake()
    {
        //var data = DataManager.Instance;
        var data = RuneUser.Instance;

        _inventoryUI.Initialize(data.Inventory, data.ItemDB, data.ColorDB);
        _upgradeUI.Initialize(_upgradeManager.UpgradeInventory, data.ItemDB, data.ColorDB);
        
        _upgradeManager.Initialize(data.UpgradeDB, data.Inventory, data.GoldData);
        _eventHandler.Initialize(_upgradeManager, _inventoryUI.Slots);
        _upgradeEventHandler.Initialize(_upgradeManager, _upgradeUI.Slots);
        
        _swapEventHandler = new(_tooltip, _dragIcon, _backgrounds);
        _registerEventHandler = new(_upgradeManager);
        _unregisterEventHandler = new (_upgradeManager);

        _eventHandler.SetMode(_swapEventHandler);
        _upgradeEventHandler.SetMode(_unregisterEventHandler);
        _upgradeUIButton.OnUpgradeMode += ChangeInventoryMode;
    }

    private void OnDestroy()
    {
        _upgradeUIButton.OnUpgradeMode -= ChangeInventoryMode;
    }

    private void ChangeInventoryMode(bool isUpgrade)
    {
        _eventHandler.SetMode(isUpgrade ? _registerEventHandler : _swapEventHandler);
    }
}
