using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UI_SlotContainer _upgradeUI;
    private UI_InventoryFilter _inventoryFilter = new();
    [SerializeField] private UI_UpgradeInfo _upgradeInfo;
    
    [Header("버튼 UI")]
    [SerializeField] private UI_WindowToggleButton _inventoryToggleButton;
    [SerializeField] private UI_WindowToggleButton _upgradeToggleButton;
    
    [Header("이벤트 핸들러")]
    [SerializeField] private SlotEventHandler _eventHandler;
    [SerializeField] private SlotEventHandler _upgradeEventHandler;
    
    [SerializeField] private SwapEventHandler _swapEventHandler;
    [SerializeField] private RegisterEventHandler _registerEventHandler;
    [SerializeField] private UnregisterEventHandler _unregisterEventHandler;
    
    [SerializeField] private UpgradeManager _upgradeManager;
    
    private Dictionary<EInventoryMode, ISlotEventHandler> _handlerDict;

    private IInventory _inventory;
    private IInventory _upgradeInventory;
    
    public void Initialize(IDataHandler data)
    {
        _inventory = data.Inventory;
        _upgradeInventory = data.UpgradeInventory;
        
        _inventory.Subscribe(RefreshInventory);
        _upgradeInventory.Subscribe(RefreshUpgradeInventory);
        _upgradeManager.Subscribe(RefreshUpgradeInfo);
        
        _handlerDict = new()
        {
            { EInventoryMode.Normal, _swapEventHandler },
            { EInventoryMode.Upgrade , _registerEventHandler }
        };
        
        ChangeInventoryMode(EInventoryMode.Closed);
        _upgradeEventHandler.SetMode(_unregisterEventHandler);
        
        _inventoryToggleButton.OnModeChanged += ChangeInventoryMode;
        _upgradeToggleButton.OnModeChanged += ChangeInventoryMode;
    }

    private void OnDestroy()
    {
        _inventory.Unsubscribe(RefreshInventory);
        _upgradeInventory.Unsubscribe(RefreshUpgradeInventory);
        _upgradeManager.Unsubscribe(RefreshUpgradeInfo);
        
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
        _inventoryUI.Refresh(_inventory.Items);
        _upgradeUI.Hide();
        _upgradeManager.UnregisterAll();
        _inventoryFilter.ResetFilter(_inventoryUI.Slots);
    }
    
    private void SetUpgradeMode()
    {
        _upgradeUI.Show();
        _upgradeUI.Refresh(_upgradeInventory.Items);
        _inventoryFilter.RefreshFilter(_upgradeInventory, _inventoryUI.Slots);
        _upgradeInfo.Refresh(_upgradeManager.UpgradeData, _upgradeUI.Slots);
    }

    private void RefreshInventory()
    {
        _inventoryUI.Refresh(_inventory.Items);
    }

    private void RefreshUpgradeInventory()
    {
        _upgradeUI.Refresh(_upgradeInventory.Items);
    }

    private void RefreshUpgradeInfo()
    {
        _inventoryFilter.RefreshFilter(_upgradeInventory, _inventoryUI.Slots);
        _upgradeInfo.Refresh(_upgradeManager.UpgradeData, _upgradeUI.Slots);
    }
}
