using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BasicAnimation : MonoBehaviour
{
    private Vector3 _originalScale;
    private Vector3 _originalPosition;
    private Vector3 _previousScale;

    private CanvasGroup _canvasGroup;
    private Sequence _currentSequence;

    private void Awake()
    {
        _originalScale = transform.localScale;
        _originalPosition = transform.localPosition;
        TryGetComponent<CanvasGroup>(out _canvasGroup);
    }

    private void OnDestroy()
    {
        _currentSequence?.Kill();
    }

    #region Popup

    public void PopUp(float duration = 0.2f, float startScale = 0f)
    {
        Show();
        transform.localScale = Vector3.one * startScale;

        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void PopDown(float duration = 0.2f, float startScale = 0f)
    {
        if (!this || !gameObject.activeInHierarchy)
            return;

        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOScale(Vector3.one * startScale, duration).SetEase(Ease.InBack)).OnComplete(() => Hide());

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    #endregion


    public void Scale(float targetScale = 0.8f, float duration = 0.2f, Ease ease = Ease.OutBack)
    {
        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOScale(_originalScale * targetScale, duration * 0.5f).SetEase(ease));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    /// 작아졌다가 원래 크기로 돌아오기.
    public void ScaleDownAndRecover(float targetScale = 0.8f, float duration = 0.2f)
    {
        _currentSequence?.Kill();
        _previousScale = transform.localScale;

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOScale(_originalScale * targetScale, duration * 0.5f).SetEase(Ease.InQuad));
        _currentSequence.Append(transform.DOScale(_previousScale, duration * 0.5f).SetEase(Ease.OutQuad));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    /// 커졌다가 원래 크기로 돌아오기 (펀치 효과 - 콤보용).
    public void ScalePunch(float punchScale = 0.2f, float duration = 0.3f, int vibrato = 10, float elasticity = 1f)
    {
        _currentSequence?.Kill();
        Vector3 punchVector = _originalScale * punchScale;

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOPunchScale(punchVector, duration, vibrato, elasticity));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    // 커졌다가 작아지기 반복.
    public void ScaleLoop(float scaleMultiplier = 1.15f, float duration = 1f, int loops = -1, Ease ease = Ease.InOutQuad)
    {
        _currentSequence?.Kill();

        Vector3 targetScale = _originalScale * scaleMultiplier;

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOScale(targetScale, duration * 0.5f).SetEase(ease));
        _currentSequence.Append(transform.DOScale(_originalScale, duration * 0.5f).SetEase(ease));
        _currentSequence.SetLoops(loops, LoopType.Restart);

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    // 커졌다가 작아지기 페이드인 페이드 아웃 반복
    public void ScaleAndAlphaLoop(float duration = 1f, float scaleMultiplier = 1.15f, float minAlpha = 0.8f, int loops = -1, Ease ease = Ease.InOutQuad)
    {
        _currentSequence?.Kill();

        Vector3 targetScale = _originalScale * scaleMultiplier;

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOScale(targetScale, duration * 0.5f).SetEase(ease));
        _currentSequence.Join(_canvasGroup.DOFade(minAlpha, duration * 0.5f).SetEase(ease));
        _currentSequence.Append(transform.DOScale(_originalScale, duration * 0.5f).SetEase(ease));     
        _currentSequence.Join(_canvasGroup.DOFade(1f, duration * 0.5f).SetEase(ease));
        _currentSequence.SetLoops(loops, LoopType.Restart);

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }


    /// 지정한 방향으로 움직이기.
    public void MoveTo(Vector3 moveOffset, float duration = 0.3f, Ease ease = Ease.OutQuad)
    {
        _currentSequence?.Kill();

        Vector3 targetPosition = _originalPosition + moveOffset;
        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOLocalMove(targetPosition, duration).SetEase(ease));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    /// 움직이면서 동시에 커지기.
    public void MoveAndScale(Vector3 moveOffset, float targetScale = 1.2f, float duration = 0.3f, Ease ease = Ease.OutBack)
    {
        _currentSequence?.Kill();

        Vector3 targetPosition = _originalPosition + moveOffset;
        Vector3 scaleTarget = _originalScale * targetScale;

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOLocalMove(targetPosition, duration).SetEase(ease));

        _currentSequence.Join(transform.DOScale(scaleTarget, duration).SetEase(ease));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    /// 원래 위치로 돌아오기.
    public void ResetPosition(float duration = 0.3f, Ease ease = Ease.OutQuad)
    {
        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOLocalMove(_originalPosition, duration).SetEase(ease));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    /// 원래 위치와 스케일로 동시에 돌아오기.
    public void ResetPositionAndScale(float duration = 0.3f, Ease ease = Ease.OutQuad)
    {
        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOLocalMove(_originalPosition, duration).SetEase(ease));
        _currentSequence.Join(transform.DOScale(_originalScale, duration).SetEase(ease));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    /// 원래 스케일로 돌아오기.
    public void ResetScale(float duration = 0.3f, Ease ease = Ease.OutQuad)
    {
        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOScale(_originalScale, duration).SetEase(ease));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    #region Shake Animations

    public void ShakePosition(float strength = 10f, float duration = 0.5f, int vibrato = 10, float randomness = 90f)
    {
        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOShakePosition(duration, strength, vibrato, randomness));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void ShakeRotation(float strength = 30f, float duration = 0.5f, int vibrato = 10, float randomness = 90f)
    {
        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOShakeRotation(duration, strength, vibrato, randomness));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void ShakeScale(float strength = 0.3f, float duration = 0.5f, int vibrato = 10, float randomness = 90f)
    {
        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOShakeScale(duration, strength, vibrato, randomness));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void ShakeAll(float positionStrength = 10f, float rotationStrength = 20f, float duration = 0.5f, int vibrato = 10)
    {
        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOShakePosition(duration, positionStrength, vibrato, 90f));
        _currentSequence.Join(transform.DOShakeRotation(duration, rotationStrength, vibrato, 90f));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    /// 진동 무한 루프.
    public void ShakeLoop(int vibrato = 5, float rotationStrength = 10f, float positionStrength = 5f, float duration = 0.5f)
    {
        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(transform.DOShakePosition(duration, positionStrength, vibrato, 90f));
        _currentSequence.Join(transform.DOShakeRotation(duration, rotationStrength, vibrato, 90f));
        _currentSequence.SetLoops(-1, LoopType.Restart);

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    #endregion


    public Tween FadeIn(float duration = 0.3f, Ease ease = Ease.OutQuad)
    {
        if (_canvasGroup == null) return null;
        if (_canvasGroup.alpha == 1) return null;

        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(_canvasGroup.DOFade(1f, duration).SetEase(ease));

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        return _currentSequence;

    }

    public void FadeLoop(float duration = 0.3f, float minValue =0, Ease ease = Ease.OutQuad, int loops = -1)
    {
        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(_canvasGroup.DOFade(minValue, duration * 0.5f).SetEase(ease));
        _currentSequence.Append(_canvasGroup.DOFade(1f, duration * 0.5f).SetEase(ease));
        _currentSequence.SetLoops(loops, LoopType.Restart);

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void PunchFadeIn(float duration = 0.3f, float punchScale = 0.2f, Ease ease = Ease.OutQuad,  int vibrato = 10, float elasticity = 1f)
    {
        if (_canvasGroup == null) return;
        if (_canvasGroup.alpha == 1) return;

        Vector3 punchVector = _originalScale * punchScale;

        _currentSequence = DOTween.Sequence();
        

        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(_canvasGroup.DOFade(1f, duration).SetEase(ease));
        _currentSequence.Join(transform.DOPunchScale(punchVector, duration, vibrato, elasticity)); ;

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void Hide()
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
        StopAndReset();
    }
    public void Show()
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }
    }

    public Tween FadeOut(float duration = 0.3f, Ease ease = Ease.OutQuad)
    {
        if (_canvasGroup == null) return null;
        if (_canvasGroup.alpha < 0.1f) return null;

        _currentSequence?.Kill();

        _currentSequence = DOTween.Sequence();
        _currentSequence.Append(_canvasGroup.DOFade(0f, duration).SetEase(ease)).OnComplete(() => StopAndReset());

        _currentSequence.SetUpdate(true);
        _currentSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        return _currentSequence;
    }

    public void StopAndReset()
    {
        _currentSequence?.Kill();
        transform.localScale = _originalScale;
        transform.localPosition = _originalPosition;
        transform.localRotation = Quaternion.identity;
    }

    public void Stop()
    {
        _currentSequence?.Kill();
    }

    public void UpdateOriginalScale()
    {
        _originalScale = transform.localScale;
    }

    public void UpdateOriginalPosition()
    {
        _originalPosition = transform.localPosition;
    }
}