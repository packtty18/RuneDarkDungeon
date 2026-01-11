using UnityEngine;

public class SlotEventHandler : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SlotContainer _container;
    private ISlotEventHandler _eventHandler;
    
    private void Awake()
    {
        _container.OnSlotClicked += OnClickSlot;
        _container.OnSlotHovered += OnHoverSlot;
    }

    private void OnDestroy()
    {
        if (_container == null) return;
        _container.OnSlotClicked -= OnClickSlot;
        _container.OnSlotHovered -= OnHoverSlot;
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
