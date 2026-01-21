using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI 연결")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _frameImage;
    [SerializeField] private Sprite _defaultFrameImage;
    [SerializeField] private GameObject _iconCover;
    
    private UI_SlotAnimationController _animation;

    private ItemData _item;
    public ItemData Item => _item;
    public bool IsEmpty => _item == null;
    private bool _isInteractable = true;

    public event Action<UI_Slot> OnSlotClicked;
    public event Action<UI_Slot> OnSlotHovered;
    public event Action<UI_Slot> OnSlotDoubleClicked;

    private void Awake()
    {
        TryGetComponent<UI_SlotAnimationController>(out _animation);
    }

    public void SetItem(ItemData item)
    {
        if (item == null)
        {
            Clear();
            return;
        }
        _item = item;
        _iconImage.sprite = item.Icon;
        _iconImage.gameObject.SetActive(true);
    }

    public void SetItem(ItemData item, Sprite frame)
    {
        _frameImage.sprite = frame;
        SetItem(item);
    }

    public void Clear()
    {
        _item = null;
        _iconImage.sprite = null;
        _iconImage.gameObject.SetActive(false);
        _frameImage.sprite = _defaultFrameImage;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
    
    public void SetFilter(bool isOn)
    {
        _iconCover.SetActive(!isOn);
        _isInteractable = isOn;
        _animation?.Reset();
        _animation?.SetActive(isOn);
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
        
        if (eventData.clickCount == 2)
        {
            OnSlotDoubleClicked?.Invoke(this);
        }
        else
        {
            OnSlotClicked?.Invoke(this);
        }
    }
}
