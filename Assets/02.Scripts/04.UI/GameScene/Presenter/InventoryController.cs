using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UpgradePresenter _upgradePresenter;
    
    [Header("버튼 UI")]
    [SerializeField] private UI_WindowToggleButton _inventoryToggleButton;
    [SerializeField] private UI_WindowToggleButton _upgradeToggleButton;
    
    [Header("이벤트 핸들러")]
    [SerializeField] private SlotEventHandler _eventHandler;
    
    [SerializeField] private SwapEventHandler _swapEventHandler;
    [SerializeField] private RegisterEventHandler _registerEventHandler;
    
    [SerializeField] private UpgradeManager _upgradeManager;
    
    private Dictionary<EInventoryMode, ISlotEventHandler> _handlerDict;

    private IReadOnlyInventory _inventory;
    
    public void Initialize(IReadOnlyInventory inventory)
    {
        _inventory = inventory;
        _inventory.Subscribe(RefreshInventory);
        
        _handlerDict = new()
        {
            { EInventoryMode.Normal, _swapEventHandler },
            { EInventoryMode.Upgrade , _registerEventHandler }
        };
        
        ChangeInventoryMode(EInventoryMode.Closed);
        
        _inventoryToggleButton.OnModeChanged += ChangeInventoryMode;
        _upgradeToggleButton.OnModeChanged += ChangeInventoryMode;
    }

    private void OnDestroy()
    {
        _inventory.Unsubscribe(RefreshInventory);
        
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
        
        _upgradePresenter.Hide();
    }
    
    private void SetUpgradeMode()
    {
        _upgradePresenter.Show();
    }

    private void RefreshInventory()
    {
        _inventoryUI.Refresh(_inventory.Items);
    }
}
