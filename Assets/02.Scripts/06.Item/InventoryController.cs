using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_Base _inventory;
    [SerializeField] private UI_Base _upgrade;
    [SerializeField] private UI_Base _inventoryFilter;
    [SerializeField] private UI_Base _upgradeFilter;
    
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
    
    private void Awake()
    {
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
        _inventory.Hide();
    }

    private void SetNormalMode()
    {
        _inventory.Show();
        _upgrade.Hide();
        _upgradeManager.UnregisterAll();
        _inventoryFilter.Hide();
        _upgradeFilter.Hide();
    }
    
    private void SetUpgradeMode()
    {
        _upgrade.Show();
        _inventoryFilter.Show();
        _upgradeFilter.Show();
    }
}
