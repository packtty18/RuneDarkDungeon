using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("슬롯 관련 UI")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UI_SlotContainer _upgradeUI;
    [SerializeField] private UI_SlotController _controller;
    
    [Header("강화 모드 관련")]
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private InventoryEventHandler _eventHandler;
    [SerializeField] private InventoryEventHandler _upgradeEventHandler;
    
    [Header("UI 연결")]
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;
    
    [SerializeField] private UI_WindowToggleButton _upgradeUIButton;

    private Dictionary<EInventoryMode, ISlotEventHandler> _handlerDict;

    private void Awake()
    {
        //var data = DataManager.Instance;
        var data = RuneUser.Instance;

        _inventoryUI.Initialize(data.Inventory, data.ItemDB, data.ColorDB);
        _upgradeUI.Initialize(_upgradeManager.UpgradeInventory, data.ItemDB, data.ColorDB);
        _controller.Initialize(_upgradeManager, _inventoryUI.Slots, _upgradeUI.Slots);
        
        _upgradeManager.Initialize(data.UpgradeDB, data.Inventory, data.GoldData);
        _eventHandler.Initialize(_inventoryUI.Slots);
        _upgradeEventHandler.Initialize(_upgradeUI.Slots);
        
        _handlerDict = new()
        {
            { EInventoryMode.Normal, new SwapEventHandler(_tooltip, _dragIcon, _backgrounds)},
            { EInventoryMode.Upgrade , new RegisterEventHandler(_upgradeManager)}
        };
        
        ChangeInventoryMode(EInventoryMode.Normal);
        _upgradeEventHandler.SetMode(new UnregisterEventHandler(_upgradeManager));
        _upgradeUIButton.OnModeChanged += ChangeInventoryMode;
    }

    private void OnDestroy()
    {
        _upgradeUIButton.OnModeChanged -= ChangeInventoryMode;
    }
    
    private void ChangeInventoryMode(EInventoryMode mode)
    {
        _eventHandler.SetMode(_handlerDict[mode]);

        if (mode == EInventoryMode.Normal)
        {
            _inventoryUI.Show();
            _upgradeUI.Hide();
            _controller.ResetSlotState();
        }
        else if (mode == EInventoryMode.Upgrade)
        {
            _upgradeUI.Show();
            _controller.RefreshSlotState();
        }
    }
}
