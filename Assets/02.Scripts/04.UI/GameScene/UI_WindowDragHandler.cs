using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_WindowDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private RectTransform _window;
    private Vector2 _beginDragPosition;
    private Vector2 _startPosition;

    private void Awake()
    {
        _window = transform.parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _beginDragPosition = eventData.position;
        _startPosition = _window.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 moveOffset = eventData.position - _beginDragPosition;
        _window.anchoredPosition = _startPosition + moveOffset;
    }
}
