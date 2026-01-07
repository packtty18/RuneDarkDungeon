using UnityEngine;
using UnityEngine.UI;

public class UI_SlotOutline : MonoBehaviour
{
    [SerializeField] private Image _outlineImage;
    [SerializeField] private GradeColorSO _colorDB;
    
    public void SetSlotUI(EItemGrade grade)
    {
        _outlineImage.color = _colorDB.GetColor(grade);
    }

    public void ClearSlotUI()
    {
        _outlineImage.color = Color.white;
    }
}
