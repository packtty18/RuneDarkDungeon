using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_WindowToggleButton : MonoBehaviour, IPointerClickHandler
{
    [Header("UI 모드 설정")]
    [SerializeField] private EInventoryMode _onMode;
    [SerializeField] private EInventoryMode _offMode;

    private bool _isOn;
    public event Action<EInventoryMode> OnModeChanged;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        ToggleMode();
    }
    
    private void ToggleMode()
    {
        _isOn = !_isOn;
        OnModeChanged?.Invoke(_isOn ? _onMode : _offMode);
    }
}
