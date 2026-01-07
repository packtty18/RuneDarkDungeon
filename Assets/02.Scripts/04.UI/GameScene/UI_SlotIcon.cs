using UnityEngine;
using UnityEngine.UI;

public class UI_SlotIcon : MonoBehaviour
{
    private Image _iconImage;

    private void Awake()
    {
        _iconImage = GetComponent<Image>();
    }

    public void SetSlotUI(Sprite icon)
    {
        _iconImage.sprite = icon;
    }

    public void ClearSlotUI()
    {
        _iconImage.sprite = null;
    }
}
