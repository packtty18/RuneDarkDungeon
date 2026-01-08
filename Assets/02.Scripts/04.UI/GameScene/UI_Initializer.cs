using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UI_Inventory _uiInventory;
    [SerializeField] private UI_Upgrade _uiUpgrade;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private SlotEventHandler _eventHandler;
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;
    
    private void Awake()
    {
        //var data = DataManager.Instance;
        var data = RuneUser.Instance;

        _uiInventory.Initialize(data.Inventory, data.ItemDB, data.ColorDB);
        _uiUpgrade.Initialize(_upgradeManager, data.ItemDB, data.ColorDB);
        _upgradeManager.Initialize(data.UpgradeDB, data.Inventory, data.GoldData);
        _eventHandler.Initialize(_upgradeManager, _uiInventory.Slots, _tooltip, _dragIcon, _backgrounds);

        _uiUpgrade.OnUIActived += SetUIMode;
    }

    private void OnDestroy()
    {
        _uiUpgrade.OnUIActived -= SetUIMode;
    }
    
    private void SetUIMode(bool isUpgrade)
    {
        _eventHandler.SetMode(isUpgrade);
    }
}
