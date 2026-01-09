using System.Collections.Generic;
using UnityEngine;

public class UI_Initializer : MonoBehaviour
{
    [Header("슬롯 관련 UI")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UI_SlotContainer _upgradeUI;
    [SerializeField] private UI_SlotController _controller;
    
    [Header("강화 모드 관련")]
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private SlotEventHandler _eventHandler;
    [SerializeField] private SlotEventHandler _upgradeEventHandler;
    
    [Header("UI 연결")]
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;
    
    [SerializeField] private UI_WindowToggleButton _inventoryToggleButton;
    [SerializeField] private UI_WindowToggleButton _upgradeToggleButton;

    private Dictionary<EInventoryMode, ISlotEventHandler> _handlerDict;

    public void Initialize(IDataHandler data)
    {
        _inventoryUI.Initialize(data.Inventory, data.ItemDB);
        _upgradeUI.Initialize(data.UpgradeInventory, data.ItemDB);
        _controller.Initialize(_upgradeManager, _inventoryUI.Slots, _upgradeUI.Slots);
        
        _upgradeManager.Initialize(data.UpgradeDB, data.UpgradeInventory, data.Inventory, data.GoldData);
        _eventHandler.Initialize(_inventoryUI.Slots);
        _upgradeEventHandler.Initialize(_upgradeUI.Slots);
        
        _handlerDict = new()
        {
            { EInventoryMode.Normal, new SwapEventHandler(_tooltip, _dragIcon, _backgrounds)},
            { EInventoryMode.Upgrade , new RegisterEventHandler(_upgradeManager)}
        };
        
        ChangeInventoryMode(EInventoryMode.Closed);
        _upgradeEventHandler.SetMode(new UnregisterEventHandler(_upgradeManager));
        
        _inventoryToggleButton.OnModeChanged += ChangeInventoryMode;
        _upgradeToggleButton.OnModeChanged += ChangeInventoryMode;
    }

    private void OnDestroy()
    {
        _inventoryToggleButton.OnModeChanged -= ChangeInventoryMode;
        _upgradeToggleButton.OnModeChanged -= ChangeInventoryMode;
    }
    
    private void ChangeInventoryMode(EInventoryMode mode)
    {
        switch (mode)
        {
            case EInventoryMode.Closed:
                CloseInventory();
                break;
            case EInventoryMode.Normal:
                SetNormalMode();
                break;
            case EInventoryMode.Upgrade:
                SetUpgradeMode();
                break;
        }

        if (!_handlerDict.TryGetValue(mode, out var handler)) return;
        _eventHandler.SetMode(handler);
    }

    private void CloseInventory()
    {
        _inventoryUI.Hide();
    }

    private void SetNormalMode()
    {
        _inventoryUI.Show();
        _upgradeUI.Hide();
        _controller.ResetSlotState();
    }
    
    private void SetUpgradeMode()
    {
        _upgradeUI.Show();
        _controller.RefreshSlotState();
    }
}
