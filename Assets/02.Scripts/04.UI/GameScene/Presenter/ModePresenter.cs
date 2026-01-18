using System;
using UnityEngine;

public class ModePresenter : MonoBehaviour
{
    [Header("버튼 UI")]
    [SerializeField] private UI_WindowToggleButton _inventoryToggleButton;
    [SerializeField] private UI_WindowToggleButton _upgradeToggleButton;
    [SerializeField] private UI_WindowToggleButton _equipmentToggleButton;
    [SerializeField] private UI_WindowToggleButton _shopToggleButton;
    
    [Header("Presenter 연결")]
    [SerializeField] private InventoryPresenter _inventory;
    [SerializeField] private UpgradePresenter _upgrade;
    [SerializeField] private EquipmentPresenter _equipment;
    
    private EInventoryMode _mode = EInventoryMode.Closed;
    
    private void Awake()
    {
        _inventoryToggleButton.OnModeChanged += OnClickInventoryButton;
        _upgradeToggleButton.OnModeChanged += OnClickUpgradeButton;
        _equipmentToggleButton.OnModeChanged += OnClickEquipmentButton;
        _shopToggleButton.OnModeChanged += OnClickShopButton;
    }

    private void OnDestroy()
    {
        _inventoryToggleButton.OnModeChanged -= OnClickInventoryButton;
        _upgradeToggleButton.OnModeChanged -= OnClickUpgradeButton;
        _equipmentToggleButton.OnModeChanged -= OnClickEquipmentButton;
        _shopToggleButton.OnModeChanged -= OnClickShopButton;
    }

    private void OnClickInventoryButton()
    {
        ChangeMode(_mode == EInventoryMode.Closed ? EInventoryMode.Normal : EInventoryMode.Closed);
    }

    public void OnClickUpgradeButton()
    {
        ChangeMode(_mode == EInventoryMode.Upgrade ? EInventoryMode.Normal : EInventoryMode.Upgrade);
    }

    private void OnClickEquipmentButton()
    {
        ChangeMode(_mode == EInventoryMode.Equipment ? EInventoryMode.Normal : EInventoryMode.Equipment);
    }

    private void OnClickShopButton()
    {
        ChangeMode(_mode == EInventoryMode.Sell ? EInventoryMode.Normal : EInventoryMode.Sell);
    }
    
    private void ChangeMode(EInventoryMode mode)
    {
        _mode = mode;
        _inventory.HandleModeChanged(mode);
        _upgrade.HandleModeChanged(mode);
        _equipment.HandleModeChanged(mode);
    }
}
