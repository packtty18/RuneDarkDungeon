using System;
using UnityEngine;

public class ModePresenter : MonoBehaviour
{
    [Header("버튼 UI")]
    [SerializeField] private UI_WindowToggleButton _inventoryToggleButton;
    [SerializeField] private UI_WindowToggleButton _upgradeToggleButton;
    [SerializeField] private UI_WindowToggleButton _equipmentToggleButton;

    private EInventoryMode _mode = EInventoryMode.Closed;
    private SafeEvent<EInventoryMode> _onModeChanged;
    
    private void Awake()
    {
        _inventoryToggleButton.OnModeChanged += OnClickInventoryButton;
        _upgradeToggleButton.OnModeChanged += OnClickUpgradeButton;
        _equipmentToggleButton.OnModeChanged += OnClickEquipmentButton;
    }

    private void OnDestroy()
    {
        _inventoryToggleButton.OnModeChanged -= OnClickInventoryButton;
        _upgradeToggleButton.OnModeChanged -= OnClickUpgradeButton;
        _equipmentToggleButton.OnModeChanged -= OnClickEquipmentButton;
    }

    private void OnClickInventoryButton()
    {
        ChangeMode(_mode == EInventoryMode.Normal ? EInventoryMode.Closed : EInventoryMode.Normal);
    }

    private void OnClickUpgradeButton()
    {
        ChangeMode(_mode == EInventoryMode.Upgrade ? EInventoryMode.Normal : EInventoryMode.Upgrade);
    }

    private void OnClickEquipmentButton()
    {
        ChangeMode(_mode == EInventoryMode.Equipment ? EInventoryMode.Normal : EInventoryMode.Equipment);
    }

    private void ChangeMode(EInventoryMode mode)
    {
        _mode = mode;
        _onModeChanged?.Invoke(_mode);
    }
    
    public void Subscribe(Action<EInventoryMode> action)
    {
        _onModeChanged.Subscribe(action);
    }

    public void Unsubscribe(Action<EInventoryMode> action)
    {
        _onModeChanged.Unsubscribe(action);
    }
}
