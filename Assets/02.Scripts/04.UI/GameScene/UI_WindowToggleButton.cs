using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_WindowToggleButton : MonoBehaviour, IPointerClickHandler
{
    [Header("UI 연결")]
    [SerializeField] private GameObject _upgradeUI;
    
    public event Action<bool> OnUpgradeMode;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }
    
    private void Toggle()
    {
        _upgradeUI.SetActive(!_upgradeUI.activeSelf);
        OnUpgradeMode?.Invoke(_upgradeUI.activeSelf);
    }
}
