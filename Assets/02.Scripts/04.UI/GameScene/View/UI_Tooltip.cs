using UnityEngine;
using TMPro;

public class UI_Tooltip : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private RectTransform _tooltip;
    [SerializeField] private TextMeshProUGUI _nameTextUI;
    [SerializeField] private TextMeshProUGUI _tooltipTextUI;
    [SerializeField] private TextMeshProUGUI _goldTextUI;

    [Header("오프셋 설정")]
    [SerializeField] private float _widthOffsetRatio = 0.6f;
    [SerializeField] private float _heightOffsetRatio = 0.4f;

    public void Show(ItemSO info, Transform icon, int gold = 0)
    {
        _nameTextUI.text = info.Name;
        _tooltipTextUI.text = info.Tooltip;
        _goldTextUI.SetText("{0} 골드", gold);
        
        SetPositionNextToIcon(icon);
        
        gameObject.SetActive(true);
        _goldTextUI.gameObject.SetActive(gold != 0);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void SetPositionNextToIcon(Transform icon)
    {
        float offsetX = _tooltip.rect.width * _widthOffsetRatio * _tooltip.lossyScale.x;
        float offsetY = _tooltip.rect.height * _heightOffsetRatio * _tooltip.lossyScale.y;
        
        transform.position = icon.position + new Vector3(offsetX, -offsetY);
    }
}
