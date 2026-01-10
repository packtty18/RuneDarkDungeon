using UnityEngine;

public class SlotEventHandler : MonoBehaviour
{
    private ISlotEventHandler _eventHandler;
    private UI_SlotContainer _container;
    
    public void Initialize(UI_SlotContainer container)
    {
        _container = container;
        foreach (var slot in _container.Slots)
        {
            slot.OnSlotClicked += OnClickSlot;
            slot.OnSlotHovered += OnHoverSlot;
        }
        _container.OnSlotAdded += RegisterSlot;
    }

    private void OnDestroy()
    {
        if (_container == null) return;
        foreach (var slot in _container.Slots)
        {
            slot.OnSlotClicked -= OnClickSlot;
            slot.OnSlotHovered -= OnHoverSlot;
        }
        _container.OnSlotAdded -= RegisterSlot;
    }

    private void RegisterSlot(UI_Slot slot)
    {
        slot.OnSlotClicked += OnClickSlot;
        slot.OnSlotHovered += OnHoverSlot;
    }

    public void SetMode(ISlotEventHandler eventHandler)
    {
        _eventHandler?.OnExit();
        _eventHandler = eventHandler;
        _eventHandler.OnEnter();
    }

    private void OnClickSlot(UI_Slot slot)
    {
        _eventHandler.OnClickSlot(slot);
    }

    private void OnHoverSlot(UI_Slot slot)
    {
        _eventHandler.OnHoverSlot(slot);
    }
}
