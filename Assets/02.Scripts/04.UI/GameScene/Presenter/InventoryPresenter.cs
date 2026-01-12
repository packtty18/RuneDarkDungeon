using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private ModePresenter _modePresenter;
    
    [Header("로직 연결")]
    [SerializeField] private SlotEventHandler _eventHandler;
    
    private Dictionary<EInventoryMode, ISlotEventHandler> _handlerDict;

    private IReadOnlyInventory _inventory;

    private bool _isOn;
    
    public void Initialize(IReadOnlyInventory inventory, IForge forge)
    {
        _inventory = inventory;
        _inventory.Subscribe(RefreshInventory);
        RefreshInventory();

        _handlerDict = new()
        {
            { EInventoryMode.Normal, new NormalEventHandler(_inventoryManager) },
            { EInventoryMode.Upgrade , new RegisterEventHandler(forge) },
            { EInventoryMode.Equipment, new EquipEventHandler(_inventoryManager) },
        };

        HandleModeChanged(EInventoryMode.Closed);
        _modePresenter.Subscribe(HandleModeChanged);
    }
    
    private void OnDestroy()
    {
        _inventory.Unsubscribe(RefreshInventory);
        _modePresenter.Unsubscribe(HandleModeChanged);
    }
    
    private void HandleModeChanged(EInventoryMode mode)
    {
        if (_handlerDict.TryGetValue(mode, out var handler))
        {
          _eventHandler.SetMode(handler);
        }

        if (mode == EInventoryMode.Closed)
        {
            _inventoryUI.Hide();
        }
        else if (mode == EInventoryMode.Normal)
        {
            _inventoryUI.Show();
        }
    }
    
    private void RefreshInventory()
    {
        _inventoryUI.Refresh(_inventory.Items);
    }
}
