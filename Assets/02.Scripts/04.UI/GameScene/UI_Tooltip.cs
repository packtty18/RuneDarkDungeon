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
    [SerializeField] private float _heightPaddingRate = 0.5f;
    private Vector3 _offset;
    
    protected override void OnInit()
    {
        CalculatePaddingOffset();
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
        transform.position = icon.position + _offset;
    }

    private void CalculatePaddingOffset()
    {
        var tooltip = GetComponent<RectTransform>();
        
        float width = tooltip.rect.width * _widthPaddingRate;
        float height = tooltip.rect.height * _heightPaddingRate;
        
        _offset = new Vector3(width, -height);
    }
}
