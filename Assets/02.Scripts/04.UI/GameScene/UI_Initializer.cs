using System.Collections.Generic;
using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("UI 연결")]
    [Space]
    
    [Header("--- 공통 ---")]
    [SerializeField] private CurrencyPresenter _currencyPresenter;
    
    [Header("--- 로비씬 전용 ---")]
    [SerializeField] private InventoryPresenter _inventoryPresenter;
    [SerializeField] private UpgradePresenter _upgradePresenter;
    [SerializeField] private EquipmentPresenter _equipmentPresenter;
    
    [Space]
    [SerializeField] private SelectionManager _selectionManager;
    [SerializeField] private ItemPriceDataSO _priceDB;
    
    [Header("--- 게임씬 전용 ---")]
    [SerializeField] private RewardPresenter _rewardPresenter;
    [SerializeField] private RewardManager _rewardManager;
    
    public void Initialize(IDataHolder data)
    {
        Dictionary<EInventoryMode, ISlotEventHandler> inventoryHandlerDict = new()
        {
            { EInventoryMode.Normal, new SelectEventHandler(data.Inventory, _selectionManager) },
            { EInventoryMode.Upgrade , new RegisterEventHandler(data.Inventory, data.Forge) },
            { EInventoryMode.Equipment, new SelectEventHandler(data.Inventory, _selectionManager) },
            { EInventoryMode.Sell, new SellEventHandler(data.Inventory, data.GoldData, _selectionManager, _priceDB) },
        };
        
        _inventoryPresenter?.Initialize(data.Inventory, inventoryHandlerDict);
        _upgradePresenter?.Initialize(data.Forge, data.Inventory, data.GoldData);
        _equipmentPresenter?.Initialize(data.Equipment, data.Inventory);
        _currencyPresenter?.Initialize(data.GoldData);
        _rewardPresenter?.Initialize(_rewardManager.RewardItems, _rewardManager.RewardGold);
        
        Destroy(gameObject);
    }
}
