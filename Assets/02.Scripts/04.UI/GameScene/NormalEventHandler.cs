using UnityEngine;

public class NormalEventHandler : ISlotEventHandler
{
    private UI_Tooltip _tooltip;
    private UI_DragIcon _dragIcon;
    private UI_Background[] _backgrounds;

    private UI_Slot _selectedSlot;

    public NormalEventHandler(UI_Tooltip tooltip, UI_DragIcon dragIcon, UI_Background[] backgrounds)
    {
        _tooltip = tooltip;
        _dragIcon = dragIcon;
        _backgrounds = backgrounds;
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
            SwapSlot(_selectedSlot, slot);
            DeselecteSlot();
        }
    }

    public void OnHoverSlot(UI_Slot slot)
    {
        if (slot == null)
        {
            _tooltip.Hide();
            return;
        }
        _tooltip.Show(slot.Data.Info, slot.transform);
    }

    public void OnEnter()
    {
        SetBackgroundsActive(true);
        foreach (var background in _backgrounds)
        {
            background.OnBackgroundClicked += OnClickBackground;
        }
    }

    public void OnExit()
    {
       DeselecteSlot();
       foreach (var background in _backgrounds)
       {
           background.OnBackgroundClicked -= OnClickBackground;
       }
    }
    
    private void SwapSlot(UI_Slot left, UI_Slot right)
    {
        SlotData data = left.Data;
        left.SetItem(right.Data);
        right.SetItem(data);
    }

    private void DeselecteSlot()
    {
        _selectedSlot = null;
        _dragIcon.Hide();
        SetBackgroundsActive(false);
    }
    
    private void OnClickBackground()
    {
        if (_selectedSlot == null) return;
        DeselecteSlot();
    }

    private void SetBackgroundsActive(bool active)
    {
        foreach (var background in _backgrounds)
        {
            background.gameObject.SetActive(active);
        }
    }
}
