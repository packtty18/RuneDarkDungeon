using DG.Tweening;
using UnityEngine;

public class UIAutoHide : UIBase
{
    [SerializeField] private float _autoHideDelay = 3f;

    private Tween _hideTween;

    private void OnDisable()
    {
        CancelTimer();
    }

    /// <summary>
    /// 타이머 리셋 및 시작 (3초 후 자동 숨김)
    /// </summary>
    public void ResetTimer()
    {
        CancelTimer();

        // 3초 후 자동으로 숨김
        _hideTween = DOVirtual.DelayedCall(_autoHideDelay, () =>
        {
            Hide();
        });
    }

    /// <summary>
    /// 타이머 취소
    /// </summary>
    public void CancelTimer()
    {
        _hideTween?.Kill();
        _hideTween = null;
    }

    private void OnDestroy()
    {
        CancelTimer();
    }
}
