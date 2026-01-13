using System.Collections.Generic;
using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI 연결")]
    [Space]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UI_SlotContainer _upgradeUI;
    [SerializeField] private UI_EquipmentView _equipmentUI;
    
    [Space]
    [SerializeField] private InventoryManager _inventoryManager;
    
    [Space]
    [SerializeField] private InventoryPresenter _inventoryPresenter;
    [SerializeField] private UpgradePresenter _upgradePresenter;
    [SerializeField] private EquipmentPresenter _equipmentPresenter;
    
    public void Initialize(IDataHandler data)
    {
        _inventoryUI.Initialize(data.ItemDB);
        _upgradeUI.Initialize(data.ItemDB);
        _equipmentUI.Initialize(data.ItemDB);
        
        _inventoryManager.Initialize(data.Inventory);
        EquipmentManager equipmentManager = new(data.Equipment, data.Inventory);
        
        Dictionary<EInventoryMode, ISlotEventHandler> inventoryHandlerDict = new()
        {
            { EInventoryMode.Normal, new NormalEventHandler(_inventoryManager) },
            { EInventoryMode.Upgrade , new RegisterEventHandler(data.Forge) },
            { EInventoryMode.Equipment, new NormalEventHandler(_inventoryManager) },
            { EInventoryMode.Sell, new SellEventHandler(data.Inventory, data.GoldData, _inventoryManager) },
        };
        
        _inventoryPresenter.Initialize(data.Inventory, inventoryHandlerDict);
        _upgradePresenter.Initialize(data.UpgradeInventory, data.Forge, new UnregisterEventHandler(data.Forge));
        _equipmentPresenter.Initialize(equipmentManager, data.Equipment);
    }
}
