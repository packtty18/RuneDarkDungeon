using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UI_SlotContainer _upgradeUI;
    [SerializeField] private UI_UpgradeInfo _upgradeInfoUI;
    
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

    private IReadOnlyInventory _inventory;
    private IReadOnlyInventory _upgradeInventory;
    
    public void Initialize(IReadOnlyInventory inventory, IReadOnlyInventory upgradeInventory)
    {
        _inventory = inventory;
        _upgradeInventory = upgradeInventory;
        
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
        _upgradeManager.UnregisterAll();
        _inventoryUI.Show();
        RefreshInventory();
        _upgradeUI.Hide();
        _inventoryUI.RefreshFilter();
    }
    
    private void SetUpgradeMode()
    {
        _upgradeUI.Show();
        RefreshUpgradeInventory();
        RefreshUpgradeInfo();
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
        _inventoryUI.RefreshFilter(_upgradeInventory);
        _upgradeUI.SetSlotCount(_upgradeManager.UpgradeData.Count);
        _upgradeInfoUI.Refresh(_upgradeManager.UpgradeData);
    }
}
