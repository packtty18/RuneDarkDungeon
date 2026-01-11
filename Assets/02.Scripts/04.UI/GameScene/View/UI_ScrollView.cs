using UnityEngine;
using UnityEngine.UI;

public class UI_ScrollView : MonoBehaviour
{
    [Header("연결")]
    [SerializeField] private RectTransform _content;

    [Header("수동 높이 설정")]
    [SerializeField] private int _columns = 5;
    [SerializeField] private float _slotHeight = 100f;
    [SerializeField] private float _spacing = 50f;
    [SerializeField] private float _topPadding = 32f;
    [SerializeField] private float _bottomPadding = 32f;

    public void UpdateLayout(int itemCount)
    {
        int rowCount = Mathf.CeilToInt((float)itemCount / _columns);

        float totalHeight = 0;
        if (rowCount > 0)
        {
            totalHeight = (rowCount * _slotHeight) + 
                          (Mathf.Max(0, rowCount - 1) * _spacing) + 
                          _topPadding + _bottomPadding;
        }

        _content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
        _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, 0);
    }
}
