using System.Collections.Generic;
using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _inventoryUI;
    
    private Dictionary<EInventoryMode, ISlotEventHandler> _handlerDict;
    private ISlotEventHandler _eventHandler;

    private IReadOnlyInventory _inventory;
    
    public void Initialize(IReadOnlyInventory inventory, Dictionary<EInventoryMode, ISlotEventHandler> handlerDict)
    {
        _inventory = inventory;
        _handlerDict = handlerDict;

        RefreshInventory();
        HandleModeChanged(EInventoryMode.Closed);
        
        _inventoryUI.OnSlotClicked += HandleSlotClicked;
        _inventoryUI.OnSlotHovered += HandleSlotHovered;
        _inventory.Subscribe(RefreshInventory);
    }
    
    private void OnDestroy()
    {
        _inventoryUI.OnSlotClicked -= HandleSlotClicked;
        _inventoryUI.OnSlotHovered -= HandleSlotHovered;
        _inventory.Unsubscribe(RefreshInventory);
    }
    
    private void RefreshInventory()
    {
        _inventoryUI.Refresh(_inventory.Items);
    }
    
    private void HandleSlotClicked(UI_Slot slot)
    {
        _eventHandler.OnClickSlot(slot);
    }

    private void HandleSlotHovered(UI_Slot slot)
    {
        _eventHandler.OnHoverSlot(slot);
    }
    
    public void HandleModeChanged(EInventoryMode mode)
    {
        if (_handlerDict.TryGetValue(mode, out var handler))
        {
            _eventHandler?.OnExit();
            _eventHandler = handler;
            _eventHandler.OnEnter();
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
}
