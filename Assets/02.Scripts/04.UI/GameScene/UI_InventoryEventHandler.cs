using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryEventHandler : MonoBehaviour
{
    [Header("UI 연결")]
    [Space]
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;

    private List<UI_Slot> _slots;
    private UI_Slot _selectedSlot;

    public void Initialize(List<UI_Slot> slots)
    {
        _slots = slots;
        Bind();
    }

    private void Bind()
    {
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked += OnClickSlot;
            slot.OnSlotHovered += OnHoverSlot;
        }

        foreach (var background in _backgrounds)
        {
            background.OnBackgroundClicked += OnClickBackground;
        }
    }

    private void OnDestroy()
    {
        if (_slots != null)
        {
            foreach (var slot in _slots)
            {
                slot.OnSlotClicked -= OnClickSlot;
                slot.OnSlotHovered -= OnHoverSlot;
            }
        }

        if (_backgrounds == null) return;
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
    
    private void OnClickSlot(UI_Slot slot)
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

    private void OnHoverSlot(UI_Slot slot)
    {
        if (slot == null)
        {
            _tooltip.Hide();
            return;
        }
        _tooltip.Show(slot.Data.Info, slot.transform);
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
    
    private void DeselecteSlot()
    {
        _selectedSlot = null;
        _dragIcon.Hide();
        SetBackgroundsActive(false);
    }
}
