using UnityEngine;
using UnityEngine.UI;

public class UI_SlotIcon : MonoBehaviour
{
    [SerializeField] private Image _iconImage;

    public void SetSlotUI(Sprite icon)
    {
        _iconImage.sprite = icon;
    }

    public void ClearSlotUI()
    {
        _iconImage.sprite = null;
    }
}
