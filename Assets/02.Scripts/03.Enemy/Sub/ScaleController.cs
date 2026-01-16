using UnityEngine;
using DG.Tweening;

public class ScaleController : MonoBehaviour
{
    [Header("Scale Settings")]
    [SerializeField] private Vector3 _startScale = Vector3.zero;
    [SerializeField] private Vector3 _targetScale = Vector3.one;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Ease _ease = Ease.OutCubic;

    [Header("Options")]
    [SerializeField] private bool _destroyOnComplete = false;

    private Tween _scaleTween;

    private void Awake()
    {
        transform.localScale = _startScale;
    }

    private void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        transform.localScale = _startScale;
        _scaleTween?.Kill();

        _scaleTween = transform
            .DOScale(_targetScale, _duration)
            .SetEase(_ease)
            .OnComplete(OnScaleComplete);
    }

    private void OnScaleComplete()
    {
        if (_destroyOnComplete)
        {
            Util.ObjectDestroy(gameObject);
        }
    }

    private void OnDisable()
    {
        _scaleTween?.Kill();
    }
}
