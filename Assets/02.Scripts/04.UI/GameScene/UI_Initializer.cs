using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UI_Inventory _ui_Inventory;
    [SerializeField] private UI_Upgrade _ui_Upgrade;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private UI_UpgradeEventHandler _eventHandler;

    private void Awake()
    {
        //var data = DataManager.Instance;
        var data = RuneUser.Instance;
        

        _ui_Inventory.Initialize(data.Inventory, data.ItemDB, data.ColorDB);
        _ui_Upgrade.Initialize(_upgradeManager.UpgradeInventory, data.ItemDB, data.ColorDB);
        _upgradeManager.Initialize(data.UpgradeDB, data.Inventory, data.GoldData);
        _eventHandler.Initialize(_upgradeManager, _ui_Inventory.Slots, _ui_Upgrade.Slots);
    }
}
