using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI_Slot ui_slot;
    
    private ItemData _item;
    private ItemSO _info;
    
    public bool IsEmpty =>  _item == null;

    private void Awake()
    {
        ui_slot = GetComponent<UI_Slot>();
    }

    public void SetItem(ItemData item, ItemSO info)
    {
        _item = item;
        _info = info;
    }

    public void Clear()
    {
        _item = null;
        _info = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI_Tooltip.Instance.Show(_info);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_Tooltip.Instance.Hide();   
    }
}
