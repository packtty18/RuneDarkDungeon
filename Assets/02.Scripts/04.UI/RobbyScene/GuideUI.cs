using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideUI : MonoBehaviour
{
    [SerializeField] private Image _imageUI;
    [SerializeField] private TextMeshProUGUI _textUI;

    [SerializeField] private Sprite[] _guideImages;
    [SerializeField, TextArea(3, 10)] private string[] _descriptions;

    private int _currentIndex = 0;

    private void OnEnable()
    {
        _currentIndex = 0;
        UpdateGuide();
    }

    public void OnValueDecrease()
    {
        _currentIndex = (_currentIndex - 1 + _guideImages.Length) % _guideImages.Length;
        UpdateGuide();
    }

    public void OnValueIncrease()
    {
        _currentIndex = (_currentIndex + 1) % _guideImages.Length;
        UpdateGuide();
    }



    private void UpdateGuide()
    {
        if (_guideImages != null && _guideImages.Length > 0)
        {
            _imageUI.sprite = _guideImages[_currentIndex];
        }

        if (_descriptions != null && _descriptions.Length > 0)
        {
            _textUI.text = _descriptions[_currentIndex];
        }
    }
}
