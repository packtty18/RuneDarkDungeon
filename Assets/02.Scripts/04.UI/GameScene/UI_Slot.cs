using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI 연결")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _outlineImage;

    private SlotData _data;

    public SlotData Data => _data;
    public Sprite Icon => _iconImage.sprite;
    public bool IsEmpty =>  _data.Item == null;
    
    public event Action<UI_Slot> OnSlotClicked;
    
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEmpty) return;
        UI_Tooltip.Instance.Show(_data.Info, transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_Tooltip.Instance.Hide();   
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSlotClicked?.Invoke(this);
    }
}
