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

    public SlotData Data => _data;
    public Sprite Icon => _iconImage.sprite;
    public bool IsEmpty =>  _data.Item == null;
    private bool _isCovered = false;

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

    public void SetUpgradeMode(bool isUpgrade)
    {
        if (_data.Item.Grade != EItemGrade.Legendary) return;
        _iconCover.SetActive(isUpgrade);
        _isCovered = isUpgrade;
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
        if (_isCovered) return;
        OnSlotClicked?.Invoke(this);
    }
}
