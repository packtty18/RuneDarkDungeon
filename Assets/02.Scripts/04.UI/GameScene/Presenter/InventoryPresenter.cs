using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private UpgradePresenter _upgradePresenter;
    [SerializeField] private EquipmentPresenter _equipmentPresenter;
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

    private EInventoryMode _mode = EInventoryMode.Closed;
    
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
        
        ChangeInventoryMode(_mode);
        
        _inventoryToggleButton.OnModeChanged += OnClickInventoryButton;
        _upgradeToggleButton.OnModeChanged += OnClickUpgradeButton;
        _equipmentToggleButton.OnModeChanged += OnClickEquipmentButton;
    }

    private void OnDestroy()
    {
        _inventory.Unsubscribe(RefreshInventory);
        
        _inventoryToggleButton.OnModeChanged -= OnClickInventoryButton;
        _upgradeToggleButton.OnModeChanged -= OnClickUpgradeButton;
        _equipmentToggleButton.OnModeChanged -= OnClickEquipmentButton;
    }
    
    private void OnClickInventoryButton()
    {
        ChangeInventoryMode(_mode == EInventoryMode.Closed ? EInventoryMode.Normal : EInventoryMode.Closed);
    }

    private void OnClickUpgradeButton()
    {
        ChangeInventoryMode(_mode == EInventoryMode.Upgrade ? EInventoryMode.Normal : EInventoryMode.Upgrade);
    }

    private void OnClickEquipmentButton()
    {
        ChangeInventoryMode(_mode == EInventoryMode.Equipment ? EInventoryMode.Normal : EInventoryMode.Equipment);
    }
    
    // Todo: 모드 관리의 책임 분리 필요
    private void ChangeInventoryMode(EInventoryMode mode)
    {
        if (_handlerDict.TryGetValue(mode, out var handler))
        {
            _eventHandler.SetMode(handler);
        }

        _mode = mode;
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
            case EInventoryMode.Equipment:
                SetEquipmentMode();
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
        _equipmentPresenter.Hide();
    }
    
    private void SetUpgradeMode()
    {
        _upgradePresenter.Show();
        _equipmentPresenter.Hide();
    }

    private void SetEquipmentMode()
    {
        _equipmentPresenter.Show();
        _upgradePresenter.Hide();
    }
    
    private void RefreshInventory()
    {
        _inventoryUI.Refresh(_inventory.Items);
    }
}
