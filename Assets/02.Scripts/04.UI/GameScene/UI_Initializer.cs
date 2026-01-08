using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UI_Inventory ui_Inventory;
    [SerializeField] private UI_Upgrade ui_Upgrade;

    private void Awake()
    {
        //var data = DataManager.Instance;
        var data = RuneUser.Instance;
        
        var upgradeManager = new UpgradeManager(
            data.Inventory, 
            data.GoldData
        );

        ui_Inventory.Initialize(data.Inventory, data.ItemDB, data.ColorDB);
        ui_Upgrade.Initialize(upgradeManager.UpgradeInventory, data.ItemDB);
    }
}
