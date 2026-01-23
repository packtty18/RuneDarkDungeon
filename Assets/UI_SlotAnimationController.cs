using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UI_BasicAnimation))]
public class UI_SlotAnimationController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField]
    private bool _active = true;

    [Header("Hover Settings")]
    [SerializeField] private float _hoverScale = 1.15f;
    [SerializeField] private float _hoverDuration = 0.25f;
    [SerializeField] private Ease _hoverEase = Ease.OutBack;

    [Header("Click Settings")]
    [SerializeField] private float _clickScale = 0.85f;
    [SerializeField] private float _clickDuration = 0.15f;

    private UI_BasicAnimation _animator;

    private void Awake()
    {
        _animator = GetComponent<UI_BasicAnimation>();
    }

    public void SetActive(bool active)
    {
        _active = active;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_active) return;
        _animator.Scale(_hoverScale, _hoverDuration, _hoverEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_active) return;
        _animator.ResetScale(_hoverDuration, Ease.OutQuad);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_active) return;
        _animator.ScaleDownAndRecover(_clickScale, _clickDuration);
    }

    public void Reset()
    {
        _animator.ResetScale(0, 0);
    }
}