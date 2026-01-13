using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;
    [SerializeField] private Texture2D _sellCursorTexture;
    
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
            background.OnBackgroundClicked -= OnClickBackground;
        }
    }

    public void SelectItem(UI_Slot slot)
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
            DeselectItem();
        }
    }

    public ItemData GetSelectedItem()
    {
        if (_selectedSlot == null) return null;
        
        var item = _selectedSlot.Item;
        DeselectItem();
        return item;
    }

    public void ShowTooltip(UI_Slot slot, bool showGold = false)
    {
        if (slot == null)
        {
            _tooltip.Hide();
            return;
        }
        _tooltip.Show(slot.Info, slot.transform, showGold);
    }
    
    public void DeselectItem()
    {
        _selectedSlot = null;
        _dragIcon.Hide();
        SetBackgroundsActive(false);
    }

    public void SetSellCursor()
    {
        Cursor.SetCursor(_sellCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    
    private void OnClickBackground()
    {
        if (_selectedSlot == null) return;
        DeselectItem();
    }
    
    private void SetBackgroundsActive(bool active)
    {
        foreach (var bg in _backgrounds)
        {
            bg.gameObject.SetActive(active);
        }
    }
}
