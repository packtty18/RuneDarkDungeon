using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UI_SlotContainer _upgradeUI;
    [SerializeField] private UI_EquipmentView _equipmentUI;
    
    [SerializeField] private InventoryManager _inventoryManager;
    
    [SerializeField] private InventoryPresenter _inventoryPresenter;
    [SerializeField] private UpgradePresenter _upgradePresenter;
    [SerializeField] private EquipmentPresenter _equipmentPresenter;
    
    public void Initialize(IDataHandler data)
    {
        _inventoryUI.Initialize(data.ItemDB);
        _upgradeUI.Initialize(data.ItemDB);
        _equipmentUI.Initialize(data.ItemDB);
        
        _inventoryManager.Initialize(data.Inventory);
        EquipmentManager equipmentManager = new(data.Equipment, data.Inventory, data.ItemDB);
        
        _inventoryPresenter.Initialize(data.Inventory, data.Forge);
        _upgradePresenter.Initialize(data.UpgradeInventory, data.Forge);
        _equipmentPresenter.Initialize(equipmentManager, data.Equipment);
    }
}
