using UnityEngine;
using TMPro;

public class UI_Tooltip : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private RectTransform _tooltip;
    [SerializeField] private TextMeshProUGUI _nameTextUI;
    [SerializeField] private TextMeshProUGUI _tooltipTextUI;

    [Header("오프셋 설정")]
    [SerializeField] private float _widthOffsetRatio = 0.6f;
    [SerializeField] private float _heightOffsetRatio = 0.4f;
    
    private void Awake()
    {
        Hide();
    }

    public void Show(ItemSO info, Transform icon)
    {
        _nameTextUI.text = info.name;
        _tooltipTextUI.text = info.Tooltip;

        SetPositionNextToIcon(icon);
        
        gameObject.SetActive(true);
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
