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

    public ItemData Item => _data.Item;
    
    public ItemSO Info => _data.Info;
    public Sprite Icon => _iconImage.sprite;
    public bool IsEmpty =>  _data.Item == null;
    private bool _isInteractable = true;

    public event Action<UI_Slot> OnSlotClicked;
    public event Action<UI_Slot> OnSlotHovered;
    
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

    public void SwapItem(UI_Slot slot)
    {
        SlotData data = _data;
        SetItem(slot._data);
        slot.SetItem(data);
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
        if (item != null) return Item.TypeEquals(item);
        return !Item.Grade.IsMaxGrade();
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
