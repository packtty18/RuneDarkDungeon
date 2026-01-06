using UnityEngine;
using TMPro;

public class UI_Tooltip : LocalSingleton<UI_Tooltip>
{
    [Header("툴팁 UI")]
    [SerializeField] private TextMeshProUGUI _nameTextUI;
    [SerializeField] private TextMeshProUGUI _tooltipTextUI;

    protected override void OnInit()
    {
        Hide();
    }

    public void Show(ItemSO info)
    {
        _nameTextUI.text = info.name;
        _tooltipTextUI.text = info.Tooltip;
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
