using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UI_Inventory ui_Inventory;
    [SerializeField] private UI_Upgrade ui_Upgrade;

    private void Awake()
    {
        var data = DataManager.Instance;
        
        var ingredientManager = new IngredientManager(data.UpgradeDB);
        
        var upgradeManager = new UpgradeManager(
            ingredientManager, 
            data.Inventory, 
            data.GoldData
        );

        ui_Inventory.Initialize(data.Inventory, data.ItemDB);
        ui_Upgrade.Initialize(data.ItemDB, upgradeManager);
    }
}
