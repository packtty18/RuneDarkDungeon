using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UI_Inventory _ui_Inventory;
    [SerializeField] private UI_Upgrade _ui_Upgrade;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private SlotEventHandler _eventHandler;
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;
    
    private void Awake()
    {
        //var data = DataManager.Instance;
        var data = RuneUser.Instance;

        _ui_Inventory.Initialize(data.Inventory, data.ItemDB, data.ColorDB);
        _ui_Upgrade.Initialize(_upgradeManager, data.ItemDB, data.ColorDB);
        _upgradeManager.Initialize(data.UpgradeDB, data.Inventory, data.GoldData);
        _eventHandler.Initialize(_upgradeManager, _ui_Inventory.Slots, _tooltip, _dragIcon, _backgrounds);
    }
}
