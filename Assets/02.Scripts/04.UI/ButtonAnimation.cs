using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(UIBasicAnimation))]
public class ButtonAnimation : MonoBehaviour
{

    [Header("Hover Settings")]
    [SerializeField] private Vector3 _hoverMoveOffset = new Vector3(15f, 0f, 0f);
    [SerializeField] private float _hoverScale = 1.15f;
    [SerializeField] private float _hoverDuration = 0.25f;
    [SerializeField] private Ease _hoverEase = Ease.OutBack;

    [Header("Click Settings")]
    [SerializeField] private float _clickScale = 0.85f;
    [SerializeField] private float _clickDuration = 0.15f;

    private UIBasicAnimation _animator;

    private void Awake()
    {
        _animator = GetComponent<UIBasicAnimation>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.MoveAndScale(_hoverMoveOffset, _hoverScale, _hoverDuration, _hoverEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.ResetPositionAndScale(_hoverDuration, Ease.OutQuad);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _animator.ScaleDownAndRecover(_clickScale, _clickDuration);
    }
}