using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Background : MonoBehaviour, IPointerClickHandler
{
    public event Action OnBackgroundClicked;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        OnBackgroundClicked?.Invoke();
    }
}
