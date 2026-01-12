using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UpgradePresenter _upgradePresenter;
    [SerializeField] private InventoryManager _inventoryManager;

    [Header("버튼 UI")]
    [SerializeField] private UI_WindowToggleButton _inventoryToggleButton;
    [SerializeField] private UI_WindowToggleButton _upgradeToggleButton;
    [SerializeField] private UI_WindowToggleButton _equipmentToggleButton;
    
    [Header("로직 연결")]
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private SlotEventHandler _eventHandler;
    
    private Dictionary<EInventoryMode, ISlotEventHandler> _handlerDict;

    private IReadOnlyInventory _inventory;
    
    public void Initialize(IReadOnlyInventory inventory)
    {
        _inventory = inventory;
        _inventory.Subscribe(RefreshInventory);
        RefreshInventory();

        _handlerDict = new()
        {
            { EInventoryMode.Normal, new NormalEventHandler(_inventoryManager) },
            { EInventoryMode.Upgrade , new RegisterEventHandler(_upgradeManager) }
        };
        
        ChangeInventoryMode(EInventoryMode.Closed);
        
        _inventoryToggleButton.OnModeChanged += ChangeInventoryMode;
        _upgradeToggleButton.OnModeChanged += ChangeInventoryMode;
        _equipmentToggleButton.OnModeChanged += ChangeInventoryMode;
    }

    private void OnDestroy()
    {
        _inventory.Unsubscribe(RefreshInventory);
        
        _inventoryToggleButton.OnModeChanged -= ChangeInventoryMode;
        _upgradeToggleButton.OnModeChanged -= ChangeInventoryMode;
        _equipmentToggleButton.OnModeChanged -= ChangeInventoryMode;
    }
    
    private void ChangeInventoryMode(EInventoryMode mode)
    {
        if (_handlerDict.TryGetValue(mode, out var handler))
        {
            _eventHandler.SetMode(handler);
        }
     
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
    }

    private void CloseInventory()
    {
        _inventoryUI.Hide();
    }

    private void SetNormalMode()
    {
        _inventoryUI.Show();
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
