using UnityEngine;

public class SwapEventHandler : MonoBehaviour, ISlotEventHandler
{
    [Header("UI 연결")]
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;

    private IInventory _inventory;
    private UI_Slot _selectedSlot;

    public void Initialize(IInventory inventory)
    {
        _inventory = inventory;
        
        foreach (var background in _backgrounds)
        {
            background.OnBackgroundClicked += OnClickBackground;
        }
    }

    private void OnDestroy()
    {
        foreach (var background in _backgrounds)
        {
            if (background == null) continue;
            background.OnBackgroundClicked -= OnClickBackground;
        }
    }

    public void OnClickSlot(UI_Slot slot)
    {
        if (_selectedSlot == null)
        {
            if (slot.IsEmpty) return;
            _selectedSlot = slot;
            _dragIcon.Show(slot.Icon);
            SetBackgroundsActive(true);
        }
        else
        {
            _inventory.Swap(_selectedSlot.Item, slot.Item);
            DeselectSlot();
        }
    }

    public void OnHoverSlot(UI_Slot slot)
    {
        if (slot == null)
        {
            _tooltip.Hide();
            return;
        }
        _tooltip.Show(slot.Info, slot.transform);
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
        DeselectSlot();
    }
    
    private void DeselectSlot()
    {
        _selectedSlot = null;
        _dragIcon.Hide();
        SetBackgroundsActive(false);
    }
    
    private void OnClickBackground()
    {
        if (_selectedSlot == null) return;
        DeselectSlot();
    }

    private void SetBackgroundsActive(bool active)
    {
        foreach (var background in _backgrounds)
        {
            background.gameObject.SetActive(active);
        }
    }
}
