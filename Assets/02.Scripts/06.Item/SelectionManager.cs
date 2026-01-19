using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;
    [SerializeField] private Texture2D _sellCursorTexture;
    
    private ItemData _selectedItem;
    public ItemData SelectedItem => _selectedItem;
    
    private void Awake()
    {
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

    public void SelectItem(ItemData item)
    {
        _selectedItem = item;
        _dragIcon.Show(item.Icon);
        SetBackgroundsActive(true);
    }
    
    public void DeselectItem()
    {
        _selectedItem = null;
        _dragIcon.Hide();
        SetBackgroundsActive(false);
    }

    public void ShowTooltip(UI_Slot slot, int gold = 0)
    {
        if (slot == null)
        {
            _tooltip.Hide();
            return;
        }
        _tooltip.Show(slot.Item.Info, slot.transform, gold);
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
        if (_selectedItem == null) return;
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
