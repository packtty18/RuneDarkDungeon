using UnityEngine;
using UnityEngine.EventSystems;

public class UI_WindowDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private Vector2 _beginDragPosition;
    private Vector2 _startPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _beginDragPosition = eventData.position;
        _startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 moveOffset = eventData.position - _beginDragPosition;
        transform.position = _startPosition + moveOffset;
    }
}
