using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Tooltip : LocalSingleton<UI_Tooltip>
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _nameTextUI;
    [SerializeField] private TextMeshProUGUI _tooltipTextUI;

    [Header("오프셋 설정")]
    [SerializeField] private float _widthPaddingRate = 0.6f;
    [SerializeField] private float _heightPaddingRate = 0.4f;
    private RectTransform _tooltip;
    
    protected override void OnInit()
    {
        _tooltip = GetComponent<RectTransform>();
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
        float offsetX = _tooltip.rect.width * _widthPaddingRate * _tooltip.lossyScale.x;
        float offsetY = _tooltip.rect.height * _heightPaddingRate * _tooltip.lossyScale.y;
        
        transform.position = icon.position + new Vector3(offsetX, -offsetY);
    }
}
