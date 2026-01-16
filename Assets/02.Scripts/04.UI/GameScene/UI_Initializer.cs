using System.Collections.Generic;
using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI 연결")]
    [Space]
    [SerializeField] private InventoryPresenter _inventoryPresenter;
    [SerializeField] private UpgradePresenter _upgradePresenter;
    [SerializeField] private EquipmentPresenter _equipmentPresenter;
    [SerializeField] private CurrencyPresenter _currencyPresenter;
    
    [Space]
    [SerializeField] private InventoryManager _inventoryManager;
    
    public void Initialize(IDataHolder data)
    {
        _inventoryManager.Initialize(data.Inventory);
        
        Dictionary<EInventoryMode, ISlotEventHandler> inventoryHandlerDict = new()
        {
            { EInventoryMode.Normal, new SelectEventHandler(_inventoryManager) },
            { EInventoryMode.Upgrade , new RegisterEventHandler(data.Inventory, data.Forge) },
            { EInventoryMode.Equipment, new EquipEventHandler(_inventoryManager) },
            { EInventoryMode.Sell, new SellEventHandler(data.Inventory, data.GoldData, _inventoryManager) },
        };
        
        _inventoryPresenter.Initialize(data.Inventory, inventoryHandlerDict);
        _upgradePresenter.Initialize(data.Forge, data.Inventory, data.GoldData);
        _equipmentPresenter.Initialize(data.Equipment, data.Inventory);
        _currencyPresenter.Initialize(data.GoldData);
    }
}
