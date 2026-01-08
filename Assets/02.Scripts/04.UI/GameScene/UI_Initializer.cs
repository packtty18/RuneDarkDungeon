using TMPro;
using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UI_Inventory _inventoryUI;
    [SerializeField] private UI_Upgrade _upgradeUI;
    
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private InventoryEventHandler _eventHandler;
    [SerializeField] private UpgradeEventHandler _upgradeEventHandler;
    
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;
    
    [SerializeField] TextMeshProUGUI _costTextUI;
    [SerializeField] TextMeshProUGUI _rateTextUI;
    
    private void Awake()
    {
        //var data = DataManager.Instance;
        var data = RuneUser.Instance;

        _inventoryUI.Initialize(data.Inventory, data.ItemDB, data.ColorDB);
        _upgradeUI.Initialize(_upgradeManager.UpgradeInventory, data.ItemDB, data.ColorDB);
        _upgradeManager.Initialize(data.UpgradeDB, data.Inventory, data.GoldData);
        _eventHandler.Initialize(_upgradeManager, _inventoryUI.Slots, _tooltip, _dragIcon, _backgrounds);
        _upgradeEventHandler.Initialize(_upgradeManager, _upgradeUI.Slots, _costTextUI, _rateTextUI);
        
        _upgradeUI.OnUIActived += SetUpgradeUIMode;
    }

    private void OnDestroy()
    {
        _upgradeUI.OnUIActived -= SetUpgradeUIMode;
    }
    
    private void SetUpgradeUIMode(bool isUpgrade)
    {
        _eventHandler.SetMode(isUpgrade);
        _inventoryUI.SetMode(isUpgrade);
    }
}
