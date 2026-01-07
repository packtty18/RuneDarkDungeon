using UnityEngine;
using UnityEngine.UI;

public class UI_SlotOutline : MonoBehaviour
{
    private Image _outlineImage;
    
    [Header("등급별 색상 설정")]
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _rareColor;
    [SerializeField] private Color _uniqueColor;
    [SerializeField] private Color _legendaryColor;

    private void Awake()
    {
        _outlineImage = GetComponent<Image>();
    }
    
    public void SetSlotUI(EItemGrade grade)
    {
        _outlineImage.color = GetColor(grade);
        Debug.Log(GetColor(grade));
    }

    public void ClearSlotUI()
    {
        _outlineImage.color = Color.white;
    }
    
    private Color GetColor(EItemGrade grade) => grade switch
    {
        EItemGrade.Normal    => _normalColor,
        EItemGrade.Rare      => _rareColor,
        EItemGrade.Unique    => _uniqueColor,
        EItemGrade.Legendary => _legendaryColor,
        _                    => Color.white
    };
}
