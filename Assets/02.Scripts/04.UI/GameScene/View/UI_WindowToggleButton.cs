using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_WindowToggleButton : MonoBehaviour, IPointerClickHandler
{
    public event Action OnModeChanged;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnModeChanged?.Invoke();
    }
}
