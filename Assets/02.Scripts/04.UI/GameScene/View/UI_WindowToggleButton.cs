using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_WindowToggleButton : MonoBehaviour, IPointerClickHandler
{
    [Header("UI 모드 설정")]
    [SerializeField] private EInventoryMode _mode;

    public event Action<EInventoryMode> OnModeChanged;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnModeChanged?.Invoke(_mode);
    }
}
