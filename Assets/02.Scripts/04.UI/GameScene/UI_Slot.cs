using UnityEngine;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour
{
    [Header("슬롯 UI")]
    [SerializeField] private Image _iconImage;

    public void SetSlotUI(Sprite icon)
    {
        _iconImage.sprite = icon;
        gameObject.SetActive(true);
    }

    public void ClearSlotUI()
    {
        _iconImage.sprite = null;
        gameObject.SetActive(false);
    }
}
