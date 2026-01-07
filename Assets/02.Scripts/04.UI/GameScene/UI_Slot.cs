using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UI_SlotIcon _uiSlotIcon;
    [SerializeField] private UI_SlotOutline _uiSlotOutline;
    
    private ItemData _item;
    private ItemSO _info;
    
    public bool IsEmpty =>  _item == null;

    public void SetItem(ItemData item, ItemSO info)
    {
        _item = item;
        _info = info;
        _uiSlotIcon.SetSlotUI(_info.Icon);
        _uiSlotOutline.SetSlotUI(item.Grade);
    }

    public void Clear()
    {
        _item = null;
        _info = null;
        _uiSlotIcon.ClearSlotUI();
        _uiSlotOutline.ClearSlotUI();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEmpty) return;
        UI_Tooltip.Instance.Show(_info, transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_Tooltip.Instance.Hide();   
    }
}
