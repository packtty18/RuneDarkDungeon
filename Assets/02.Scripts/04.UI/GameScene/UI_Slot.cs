using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI 연결")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _outlineImage;
    [SerializeField] private GameObject _iconCover;

    private SlotData _data;

    private int _index;
    public int Index => _index;
    
    public ItemData Item => _data.Item;
    
    public ItemSO Info => _data.Info;
    public Sprite Icon => _iconImage.sprite;
    public bool IsEmpty =>  _data.Item == null;
    private bool _isInteractable = true;

    public event Action<UI_Slot> OnSlotClicked;
    public event Action<UI_Slot> OnSlotHovered;

    public void SetIndex(int index)
    {
        _index = index;
    }
    
    public void SetItem(SlotData data)
    {
        if (data.Item == null)
        {
            Clear();
            return;
        }
        
        _data = data;
        _iconImage.sprite = data.Info.Icon;
        _outlineImage.color = data.Color;
    }

    public void Clear()
    {
        _data = SlotData.Empty;
        _iconImage.sprite = null;
        _outlineImage.color = Color.white;
    }
    
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
    
    public void SetInteractable(bool isOn)
    {
        _iconCover.SetActive(!isOn);
        _isInteractable = isOn;
    }

    public bool CanUpgrade(ItemData item)
    {
        return Item.CanUpgrade(item);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEmpty) return;
        OnSlotHovered?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnSlotHovered?.Invoke(null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isInteractable) return;
        OnSlotClicked?.Invoke(this);
    }
}
