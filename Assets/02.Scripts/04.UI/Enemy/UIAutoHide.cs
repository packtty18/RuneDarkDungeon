using DG.Tweening;
using UnityEngine;

public class UIAutoHide : UIBase
{
    [SerializeField] private float _autoHideDelay = 3f;

    private Tween _hideTween;
    private UIBase _ui;

    private void Awake()
    {
        _ui = GetComponent<UIBase>();
    }

    private void OnDisable()
    {
        _hideTween?.Kill();
    }

    public void ResetTimer()
    {
        _hideTween?.Kill();

        _hideTween = DOVirtual.DelayedCall(_autoHideDelay, Hide);
    }

}
