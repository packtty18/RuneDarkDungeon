using System.Collections.Generic;
using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("슬롯 관련 UI")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UI_SlotContainer _upgradeUI;
    [SerializeField] private UI_InventoryFilter _inventoryFilter;
    [SerializeField] private UI_UpgradeFilter _upgradeFilter;
    [SerializeField] private InventoryController _controller;
    
    [Header("강화 모드 관련")]
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private SlotEventHandler _eventHandler;
    [SerializeField] private SlotEventHandler _upgradeEventHandler;
    
    [Header("기타 UI")]
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;
    
    private Dictionary<EInventoryMode, ISlotEventHandler> _handlerDict;
    
    public void Initialize(IDataHandler data)
    {
        _inventoryUI.Initialize(data.Inventory, data.ItemDB);
        _upgradeUI.Initialize(data.UpgradeInventory, data.ItemDB);
        
        _inventoryFilter.Initialize(_upgradeManager, data.UpgradeInventory, _inventoryUI.Slots);
        _upgradeFilter.Initialize(_upgradeManager, _upgradeUI.Slots);
        
        _upgradeManager.Initialize(data.UpgradeDB, data.UpgradeInventory, data.Inventory, data.GoldData);
        _eventHandler.Initialize(_inventoryUI);
        _upgradeEventHandler.Initialize(_upgradeUI);
        
        _handlerDict = new()
        {
            { EInventoryMode.Normal, new SwapEventHandler(data.Inventory, _tooltip, _dragIcon, _backgrounds)},
            { EInventoryMode.Upgrade , new RegisterEventHandler(_upgradeManager)}
        };
        
        _upgradeEventHandler.SetMode(new UnregisterEventHandler(_upgradeManager));
        _controller.Initialize(_handlerDict, _eventHandler, _upgradeManager);
    }
}
